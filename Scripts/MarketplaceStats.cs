using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MarketplaceStats : MonoBehaviour
{
    //Public variables to be set in inspector
    public MarketplaceOptions mO;
    public GameObject tileInfo;

    public GameObject badPol;
    public GameObject goodPol;

    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI purchaseText;

    //variables used in script
    GameObject gameManager;

        //money
    public int[] money;
    public int moneyOut;
        
        //food
    public int[] food;
    public int foodOut;

        //sustainability
    public float[] sustValues;
    public float sustainability;

        //purchase price
    public int purchasePrice;

    //Get stats
    public void GetStats()
    {
        //get data relating to target
        ObjectData selectedData = gameManager.GetComponent<GameInfo>().GetTypeInfo(mO.GetOptType(), mO.GetOptFill());

        //sets values to be parsed into strings
        money = tileInfo.GetComponent<ObjectOutput>().GetTileMoneyOutput();
        food = tileInfo.GetComponent<ObjectOutput>().GetTileFoodOutput();

        //sets purachase cost of selected tile
        purchasePrice = selectedData.purchaseCost;

        //Parses numeric values into string to show money and food and cost
        moneyText.text = money[0].ToString();
        foodText.text = food[0].ToString();
        purchaseText.text = purchasePrice.ToString();
        
        //checks if the parent ahs an object output component
        if (tileInfo.TryGetComponent<ObjectPollution>(out ObjectPollution pol))
        {
            sustainability = pol.pol_lvl1;
        }
        else if (tileInfo.GetComponentInChildren<ObjectPollution>().GetPolValues() != null) //checks if the child has an object output component
        {
            //in child
            sustValues = tileInfo.GetComponentInChildren<ObjectPollution>().GetPolValues();
        }
       
        //gets current sustainability value
        float currentSust = gameManager.GetComponent<SustainabilityScript>().GetSustainability();

        //takes overall sustainability and adds the fields sustainability
        float futureSustainability = currentSust + sustainability;

        if (futureSustainability <= (sustainability * 3)) //Checks how much pollution this field is producing by checking to see                                                
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

    void Start()
    {
        //finds game manager
        gameManager = GameObject.FindWithTag("GameController");

        gameManager.GetComponent<InputScript>().SetNewMarketplaceMenu(true);
    }

    void Update()
    {
        //checks if there has been a new market menu spawned
        if (gameManager.GetComponent<InputScript>().GetNewMarketplaceMenu() == true)
        {
            //sets boolean to false so that new radial menus dont get spawned
            gameManager.GetComponent<InputScript>().SetNewMarketplaceMenu(false);

            //Gets stats to show
            GetStats();
        }
    }
}
