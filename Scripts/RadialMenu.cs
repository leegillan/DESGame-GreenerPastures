using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    //public variables to be set in inspector
    public Text label;
    public RadialButtonScript buttonPrefab;
    public RadialButtonScript selected;

    //variables used in script
    GameObject gameManager;

    //instance of class
    GameLevelStorer gls = new GameLevelStorer();

    //stores levels
    int[] levels = new int[3];

    //wWise variables
    public AK.Wwise.Bank MyBank = null;
    public AK.Wwise.Event MyEvent = null;

    public void Start()
    {
        //loads bank
        MyBank.Load();
    }

    public void SpawnButtons(RadialPressable obj)
    {
        //finds game manager in scene
        gameManager = GameObject.FindWithTag("GameController");

        //starts animate buttons 
        StartCoroutine(AnimateButtons(obj));
    }

    //Animates buttons popping in
    IEnumerator AnimateButtons(RadialPressable obj)
    {
        //loops through radial buttons length
        for (int i = 0; i < obj.options.Length; i++)
        {
            //creates new instance of radial button script
            RadialButtonScript newButton = Instantiate(buttonPrefab) as RadialButtonScript;

            //sets objetc as child of radial
            newButton.transform.SetParent(transform, false);

            //math varaibles for placement of buttons in a circle
            float theta = (2 * Mathf.PI / obj.options.Length) * i;
            float xPos = Mathf.Sin(theta);
            float yPos = Mathf.Cos(theta);

            //data for new button being set
            newButton.transform.localPosition = new Vector3(xPos, yPos, 0f) * 100f;
            newButton.circle.color = obj.options[i].Color;
            newButton.symbol.sprite = obj.options[i].Symbol;
            newButton.title = obj.options[i].Title;

            //checks if name of button is demolish to give it a tag so that we can find it to play the destruction sound fro wWise
            if (newButton.title == "Demolish")
            {
                //sets tag
                newButton.gameObject.tag = "DemolishButton";

                //sets event and sound bank
                MyBank = gameManager.GetComponent<GameLoop>().MyBank;
                MyEvent = gameManager.GetComponent<GameLoop>().MyEvent;

            }

            //checks if in tutorial and in the radial flash event
            if (TutorialEvents.instance != null && TutorialEvents.instance.GetCurrentEvent() == TutorialEvents.TutEvents.RadialFlash)
            {
                //checks if the button is the demolish button
                if (newButton.title == "Demolish")
                {
                    //Gives button an animator compnent to flash only if its the destroy button
                    newButton.gameObject.AddComponent<Animator>();

                    //sets new animator component to buttons component
                    Animator butAnim = newButton.GetComponent<Animator>();

                    //sets animator
                    butAnim.runtimeAnimatorController = TutorialManager.instance.GetAnimContr();

                    //plays animation
                    butAnim.Play("UIFlashAnim");
                }
            }

            //sets menu data to this script
            newButton.myMenu = this;

            //paying animation
            newButton.Anim();

            //waits a bit to loop again
            yield return new WaitForSeconds(0.06f);
        }
    }

    //radial options
    public void RadialOption()
    {
        //instance a new gameobject
        GameObject gridTile;

        //gets levels of default buildings
        gls.GetLevels();

        //sets new level values
        levels = gls.GetLvls();

        //sets current levels of buildings
        int currentFarmLevel = levels[0];
        int currentBarnLevel = levels[1];
        int currentResearchLevel = levels[2];

        //set fail to upgrade to false
        bool failToUpgrade = false;

        //gets tile currently selected
        int selectedID = gameManager.GetComponent<InputScript>().GetSelectedID();

        //get selected tile and set it to the new gameobject
        gridTile = gameManager.GetComponent<GridScript>().GetGridTile(selectedID);

        //checks if something has been selected
        if (selected)
        {
            //button function goes here
            Debug.Log(selected.title + "was selected");

            //Disables stats menus that are not needed anymore
            RadialMenuSpawner.instance.DisableStatsMenus();

            //checks title of selected button
            if (selected.title == "Upgrade")
            {
                //Checks if the player has right level of farmhouse
                if ((currentFarmLevel > gridTile.GetComponent<ObjectInfo>().GetObjectLevel() || gridTile.GetComponent<ObjectInfo>().GetObjectType() == ObjectInfo.ObjectType.FARMHOUSE || gridTile.GetComponent<ObjectInfo>().GetObjectType() == ObjectInfo.ObjectType.BARN || gridTile.GetComponent<ObjectInfo>().GetObjectType() == ObjectInfo.ObjectType.RESEARCH))
                {
                    gameManager.GetComponent<InputScript>().AttemptUpgrade(selectedID);
                }
                else
                {
                    //sets up for pop up to tell them they need to upgrade farmhouse
                    failToUpgrade = true;

                    //shows warning message about upgrading
                    gameManager.GetComponent<GameLoop>().GetUpgradeWarning().gameObject.SetActive(true);
                    gameManager.GetComponent<InputScript>().SetAllowSelecting(false);
                }
            }
            else if (selected.title == "Build") // will build assets
            {
                gameManager.GetComponent<InputScript>().AttemptBuild(ObjectInfo.ObjectType.FIELD, ObjectFill.FillType.NONE);
            }
            else if (selected.title == "Demolish") //will destroy selected tile
            {
                //Plays destruction sound
                AkSoundEngine.RegisterGameObj(GameObject.FindGameObjectWithTag("DemolishButton"));
                AkSoundEngine.PostEvent("Destruction", GameObject.FindGameObjectWithTag("DemolishButton"));
                AkSoundEngine.UnregisterGameObj(GameObject.FindGameObjectWithTag("DemolishButton"));

                //Attempts to demolish the field
                gameManager.GetComponent<InputScript>().AttemptDemolish(selectedID);

                //If in the tutorial and at the carrot event it will set up the next event
                if (TutorialEvents.instance != null && TutorialEvents.instance.GetCurrentEvent() == TutorialEvents.TutEvents.RadialFlash)
                {
                    TutorialEvents.instance.SetDestroyedCarrot(true);
                    TutorialEvents.instance.RunEvent(3);
                }
            }

            //Destroys and wait to be able to spawn next radial menu
            RadialMenuSpawner.instance.DestroyRadial(gameObject);

            //Sets radial menu to false
            RadialMenuSpawner.instance.SetAwake(false);


        }
    }
}
