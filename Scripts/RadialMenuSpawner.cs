using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuSpawner : MonoBehaviour
{
    //public variables to be set in inspector
    public static RadialMenuSpawner instance;
    public GameObject[] statsMenus;
    public RadialMenu menuPrefab;

    //variables used in script
    public bool awake = false;
    

    //getters
    public bool GetAwake() { return awake; }
    
    //setters
    public void SetAwake(bool a) { awake = a; }
   

    //loops through menus and disables them so that only the menu needed is on screen
    public void DisableStatsMenus() { for (int i = 0; i < statsMenus.Length; i++) { statsMenus[i].SetActive(false); } }

    private void Awake()
    {
        //sets instance of script
        instance = this; 
    }

    //Destroys radial
    public void DestroyRadial(GameObject gameObj)
    {
        //Destroys gameobject
        Destroy(gameObj);

        StartCoroutine(WaitForNewRadial());
    }

    //waits for a sepcified time to be able to open a new radial menu
    public IEnumerator WaitForNewRadial()
    {
        //variables
        float counter = 0;
        float waitTime = 0.5f;

        //Now, Wait until the current state is done playing
        while (counter < (waitTime))
        {
            //counter gets deltatime added
            counter += Time.deltaTime;

            //return null when condition not met
            yield return null;
        }

        //allow selecting again
        GameObject gameManager = GameObject.FindGameObjectWithTag("GameController");
        gameManager.GetComponent<InputScript>().AllowSelecting();
    }

    //spawns new radial menu
    public void SpawnMenu(RadialPressable obj)
    {
        //sets new instance of the radial menu
        RadialMenu newMenu = Instantiate(menuPrefab) as RadialMenu;

        //Gives radial menu a while awake script component
        newMenu.gameObject.AddComponent<WhileAwake>();

        //sets data for radial menu
        newMenu.transform.SetParent(transform, false);
        newMenu.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0.0f);
        newMenu.label.text = obj.title.ToUpper();

        //Checks if in tutorial and if the radial event is active
        if (TutorialEvents.instance != null && TutorialEvents.instance.GetCurrentEvent() == TutorialEvents.TutEvents.RadialFlash)
        {
            //sets tutorial box to be shown
            TutorialManager.instance.SetTutorialBox(true);
        }

        //calls functions to spawn needed buttons
        newMenu.SpawnButtons(obj);

        //sets position in canvas to be the first
        newMenu.transform.SetSiblingIndex(0);

        //sets awake to true
        SetAwake(true);
    }

    //spawns radial stats menu
    public void SpawnStats(ObjectInfo.ObjectType objType, ObjectFill.FillType objFill)
    {
        //calls disable function to loop through amount of menus and disable them
        DisableStatsMenus();
       
        //checks type and then depending on the type sets menu active
        switch (objType)
        {
            //if the type is a filed it checks which fill the field has
            case ObjectInfo.ObjectType.FIELD:

                switch(objFill)
                {
                    case ObjectFill.FillType.WHEAT:
                        statsMenus[0].SetActive(true);
                        break;

                    case ObjectFill.FillType.CORN:
                        statsMenus[2].SetActive(true);
                        break;

                    case ObjectFill.FillType.CARROT:
                        statsMenus[3].SetActive(true);
                        break;

                    case ObjectFill.FillType.POTATO:
                        statsMenus[4].SetActive(true);
                        break;

                    case ObjectFill.FillType.TURNIP:
                        statsMenus[5].SetActive(true);
                        break;

                    case ObjectFill.FillType.SUGARCANE:
                        statsMenus[9].SetActive(true);
                        break;

                    case ObjectFill.FillType.SUNFLOWER:
                        statsMenus[10].SetActive(true);
                        break;

                    case ObjectFill.FillType.COCCOA:
                        statsMenus[11].SetActive(true);
                        break;
                }

                break;

            case ObjectInfo.ObjectType.RICE:
                statsMenus[1].SetActive(true);
                break;

            case ObjectInfo.ObjectType.COW_FIELD:
                statsMenus[6].SetActive(true);
                break;

            case ObjectInfo.ObjectType.CHICKEN_COOP:
                statsMenus[7].SetActive(true);
                break;

            case ObjectInfo.ObjectType.PIG_PEN:
                statsMenus[8].SetActive(true);
                break;

            case ObjectInfo.ObjectType.BATTERY:
                statsMenus[12].SetActive(true);
                break;

            case ObjectInfo.ObjectType.MEAT_LAB:
                statsMenus[13].SetActive(true);
                break;

            case ObjectInfo.ObjectType.VERTICAL_FARM:
                statsMenus[14].SetActive(true);
                break;

            case ObjectInfo.ObjectType.FARMHOUSE:
                statsMenus[15].SetActive(true);
                break;

            case ObjectInfo.ObjectType.BARN:
                statsMenus[16].SetActive(true);
                break;

            case ObjectInfo.ObjectType.RESEARCH:
                statsMenus[17].SetActive(true);
                break;

            default:
                return;
        }

    }
}
  