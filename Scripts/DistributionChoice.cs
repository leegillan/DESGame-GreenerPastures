using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributionChoice : MonoBehaviour
{
    //Instance of class
    public static DistributionChoice instance;

    //Public variables to be set in inspector
    public GameObject gameManager;
    public GameObject enterButton;

    public Button BFB;
    public Button PB;
    public Button GGB;

    //variables used in script
    string distributerChoice;
    bool distributionOpen;

    //getters
    public string GetDistributionChoice() { return distributerChoice; }
    public bool GetDistributionOpen() { return distributionOpen; }

    //setters
    public void SetDefDistributionChoice() { distributerChoice = "P"; }
    public void SetDistributionChoice(string d) { distributerChoice = d; }
    public void SetDistributionOpen(bool dO) { distributionOpen = dO; }

    //Sets buttons when loading from a save
    public void SetDistribubuterButtons(string distributer)
    {
        //checks loaded distributer to change buttons
        if (distributer == "BF")
        {
            BFB.interactable = false;
            PB.interactable = true;
            GGB.interactable = true;
        }
        else if(distributer == "P")
        {
            BFB.interactable = true;
            PB.interactable = false;
            GGB.interactable = true;
        }
        else if(distributer == "GG")
        {
            BFB.interactable = true;
            PB.interactable = true;
            GGB.interactable = false;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        //sets instance
        instance = this;

        //sets default distributer
        distributerChoice = "P";
    }

    void Update()
    {
        //checks if distribution is open and if selecting is true
        if (gameManager.GetComponent<InputScript>().GetAllowSelecting() == false && enterButton.active == true)
        {
            enterButton.SetActive(false);
        }
        else if (gameManager.GetComponent<InputScript>().GetAllowSelecting() == true && enterButton.active == false)
        {
            enterButton.SetActive(true);
        }
       
        //checks if distribution id open and if selecting is true
        if (gameManager.GetComponent<InputScript>().GetAllowSelecting() == true && distributionOpen == true)
        {
            gameManager.GetComponent<InputScript>().SetAllowSelecting(false);
        }
    }

    //changing current Distributor
    public void ChangeDistributer()
    {
        //checks what button 
        if (BFB.interactable == false)
        {
            //if in tutorial then trigger event
            if (TutorialManager.instance != null && TutorialManager.instance.GetTutorial() == true)
            {
                TutorialEvents.instance.SetChosenDistributor(true);
            }

            //sets distributer if chosen
            distributerChoice = "BF";

            //sets multiplier to distributer pollution effect
            gameManager.GetComponent<SustainabilityScript>().SetMultiplier(1.2f);
        }
        else if (PB.interactable == false)
        {
            //sets distributer if chosen
            distributerChoice = "P";

            //sets multiplier to distributer pollution effect
            gameManager.GetComponent<SustainabilityScript>().SetMultiplier(1.0f);
        }
        else if (GGB.interactable == false)
        {
            //sets distributer if chosen
            distributerChoice = "GG";

            //sets multiplier to distributer pollution effect
            gameManager.GetComponent<SustainabilityScript>().SetMultiplier(0.8f);
        }
    }
}
