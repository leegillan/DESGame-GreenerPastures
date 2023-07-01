using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RadialStats : MonoBehaviour
{
    //public variables to be set in inspector
    public GameObject gameManager;

    public GameObject badPol;
    public GameObject goodPol;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI nextMoneyText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI nextFoodText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI moneyPlusSymbol;
    public TextMeshProUGUI foodPlusSymbol;

    GameObject selectedTile;

    //variables used in script
        //money
    public int[] money;
    public int moneyCurrent;
    public int nextMoney;
    
        //food
    public int[] food;
    public int foodCurrent;
    public float nextFood;

        //level
    int level;

        //Sustainability
    public float[] sustValues;
    public float sustainability;
    public int upgradePrice;

    //Gets stats
    public void GetStats()
    {
        //Gets selceted tile to be used to gather data
        selectedTile = gameManager.GetComponent<GameLoop>().GetSelectedTile();

        //Gets object data of the selected tile
        ObjectData selectedData = gameManager.GetComponent<GameInfo>().GetTypeInfo(selectedTile.GetComponent<ObjectInfo>().GetObjectType(), selectedTile.GetComponent<ObjectFill>().GetFillType()); //get data relating to target

        //Money and food for stats radial menu
        if (selectedTile.TryGetComponent(out ObjectOutput output))
        {
            //in parent object
            money = output.GetComponent<ObjectOutput>().GetTileMoneyOutput();
            food = output.GetComponent<ObjectOutput>().GetTileFoodOutput();
        }
        else if(selectedTile.GetComponent<ObjectInfo>().GetObjectType() != ObjectInfo.ObjectType.FARMHOUSE 
                || selectedTile.GetComponent<ObjectInfo>().GetObjectType() != ObjectInfo.ObjectType.BARN 
                || selectedTile.GetComponent<ObjectInfo>().GetObjectType() != ObjectInfo.ObjectType.RESEARCH) //checks if the selected tile is not one of the defaults
        {
            //in child object
            money = selectedTile.GetComponentInChildren<ObjectOutput>().GetTileMoneyOutput();
            food = selectedTile.GetComponentInChildren<ObjectOutput>().GetTileFoodOutput();
        }

        //Sustainability for radial stats menu
        if (selectedTile.TryGetComponent(out ObjectPollution pol))
        {
            //in parent
            sustValues = pol.GetPolValues();
        }
        else if (selectedTile.GetComponentInChildren<ObjectPollution>().GetPolValues() != null)
        {
            //in child
            sustValues = selectedTile.GetComponentInChildren<ObjectPollution>().GetPolValues();
        }
        else
        {
            sustainability = 0;
        }

        //Sets values to show in menu depending on level of field
        if (selectedTile.GetComponent<ObjectInfo>().GetObjectLevel() == 1) // level 1 values
        {
            //sets money and next to level 1 values
            moneyCurrent = money[0];
            nextMoney = money[1] - moneyCurrent;

            //sets food and next to level 1 values
            foodCurrent = food[0];
            nextFood = food[1] - foodCurrent;

            //sets level
            level = 1;

            //sets sustainability to level 1 values
            sustainability = sustValues[0];

            //sets upgrade price to next cost value
            upgradePrice = selectedData.level2Cost;
        }
        else if (selectedTile.GetComponent<ObjectInfo>().GetObjectLevel() == 2) // level 2 values
        {
            //sets money and next to level 2 values
            moneyCurrent = money[1];
            nextMoney = money[2] - moneyCurrent;

            //sets food and next to level 21 values
            foodCurrent = food[1];
            nextFood = food[2] - foodCurrent;

            //sets level
            level = 2;

            //sets sustainability to level 2 values
            sustainability = sustValues[1];

            //sets upgrade price to next cost value
            upgradePrice = selectedData.level3Cost;
        }
        else // level 3 values
        {
            //sets money and next to level 3 values
            moneyCurrent = money[2];
            foodCurrent = food[2];

            //sets level
            level = 3;

            //sets sustainability to level 2 values
            sustainability = sustValues[2];
        }
       
        //Parses numeric values into string to show money and food
        moneyText.text = moneyCurrent.ToString();
        foodText.text = foodCurrent.ToString();
        levelText.text = level.ToString();

        //Checks if there is another level to show
        if (selectedTile.GetComponent<ObjectInfo>().GetObjectLevel() != 3)
        {
            nextMoneyText.text = nextMoney.ToString();
            nextFoodText.text = nextFood.ToString();
            upgradeText.text = upgradePrice.ToString();
        }
        else //if not then the text is set to blank
        {
            nextMoneyText.text = "";
            nextFoodText.text = "";
            moneyPlusSymbol.text = "";
            foodPlusSymbol.text = "";
            upgradeText.text = "Completed";
        }

        //if selected grid is default buidlings then the values are set to no output if they are 0
        if(selectedTile.GetComponent<ObjectInfo>().GetObjectType() == ObjectInfo.ObjectType.FARMHOUSE || selectedTile.GetComponent<ObjectInfo>().GetObjectType() == ObjectInfo.ObjectType.BARN || selectedTile.GetComponent<ObjectInfo>().GetObjectType() == ObjectInfo.ObjectType.RESEARCH)
        {
            //checks money output and if its 0 then sets text to say no output
            if(moneyCurrent == 0)
            {
                moneyText.text = "No output";
                nextMoneyText.text = "";
                moneyPlusSymbol.text = "";
            }

            //checks food output and if its 0 then sets text to say no output
            if (foodCurrent == 0)
            {
                foodText.text = "No output";
                nextFoodText.text = "";
                foodPlusSymbol.text = "";
            }
        }

        //sets current sustainability value
        float currentSust = gameManager.GetComponent<SustainabilityScript>().GetSustainability();

        //takes overall sustainability and adds the fields sustainability
        float futureSustainability = currentSust + sustainability;

        //if 0 then the sustainability shos as good as there is no impact
        if (sustainability == 0)
        {
            goodPol.SetActive(true);
            badPol.SetActive(false);

            goodPol.GetComponent<TextMeshProUGUI>().text = "+ + +";
        }
        else if (futureSustainability <= (sustainability * 3))//Checks how much pollution this field is producing by checking to see                                                
        {                                                       //if adding this farms value to current sustainabilty value would have a small impact on if we were to add 3 of that field
            //Sets what pollution to show
            badPol.SetActive(false);
            goodPol.SetActive(true);

            //changes colour of the text
            goodPol.GetComponent<TextMeshProUGUI>().color = new Color(0.1370509f, 0.6132076f, 0.03181738f);

            //Checks value against a range of the current sustainability and chooses how much impact to show the field has on the farm
            if (futureSustainability * 0.6 < currentSust)
            {
                goodPol.GetComponent<TextMeshProUGUI>().text = "+ + +";
            }
            else if (futureSustainability * 0.8 < currentSust)
            {
                goodPol.GetComponent<TextMeshProUGUI>().text = "+ +";
            }
            else
            {
                goodPol.GetComponent<TextMeshProUGUI>().text = "+";
            }

        }
        else
        {
            //Sets what pollution to show
            badPol.SetActive(true);
            goodPol.SetActive(false);

            //changes colour of the text
            badPol.GetComponent<TextMeshProUGUI>().color = new Color(1.0f, 0.0f, 0.0f);

            //Checks value against a range of the current sustainability and chooses how much impact to show the field has on the farm
            if (futureSustainability * 0.6 > currentSust)
            {
                badPol.GetComponent<TextMeshProUGUI>().text = "-";
            }
            else if (futureSustainability * 0.8 > currentSust)
            {
                badPol.GetComponent<TextMeshProUGUI>().text = "- -";
            }
            else
            {
                badPol.GetComponent<TextMeshProUGUI>().text = "- - -";
            }

        }
    }

    private void Update()
    {
        //checks if there has been a new radial menu spawned
        if(gameManager.GetComponent<InputScript>().GetNewRadialMenu() == true)
        {
            //Gets stats to show
            GetStats();

            //sets boolean to false so that new radial menus dont get spawned
            gameManager.GetComponent<InputScript>().SetNewRadialMenu(false);
        }
    }
}
