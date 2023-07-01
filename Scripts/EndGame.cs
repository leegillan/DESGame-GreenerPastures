using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    //public variables to be set in inspector
    public GameObject BadEnd;
    public GameObject GoodEnd;
    public GameObject Stats;

    //field stats
    public TextMeshProUGUI[] fieldCountText;
    public TextMeshProUGUI[] moneyPerText;
    public TextMeshProUGUI[] foodPerText;

    //game stats
    public TextMeshProUGUI totalMoneyEarnedText;
    public TextMeshProUGUI totalMoneySpentText;
    public TextMeshProUGUI totalTimePlayedText;
    public TextMeshProUGUI totalFoodProducedText;
    public TextMeshProUGUI totalPeopleFedText;

    public TextMeshProUGUI reasonOfEndText;

    //variable used in script
    bool gathered;

    // Start is called before the first frame update
    void Awake()
    {
        //Checks waht ending to video to play
        if (PlayerPrefs.GetInt("Ending") == 0)
        {
            BadEnd.SetActive(true);
            GoodEnd.SetActive(false);
        }
        else
        {
            GoodEnd.SetActive(true);
            BadEnd.SetActive(false);
        }

        //gather stats to show in menu
        GatherStats();
    }

    void GatherStats() 
    {
        //if stats havent been gathered yet
        if (gathered == false)
        {
            //gets time since the scene was loaded
            System.TimeSpan t = System.TimeSpan.FromSeconds(PlayerPrefs.GetFloat("TotalTimePlayed"));

            //Sets text to values
            totalMoneyEarnedText.text = PlayerPrefs.GetInt("TotalMoneyEarned").ToString();
            totalMoneySpentText.text = PlayerPrefs.GetInt("TotalMoneySpent").ToString();
            totalFoodProducedText.text = PlayerPrefs.GetFloat("TotalFoodProduced").ToString();
            totalTimePlayedText.text = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
            totalPeopleFedText.text = PlayerPrefs.GetInt("PeopleFed").ToString();
            reasonOfEndText.text = PlayerPrefs.GetString("ReasonOfEnd");

            //Loops though all fields to show values for each
            for (int i = 0; i < 15; i++)
            {
                switch (i)
                {
                    case 0:
                        fieldCountText[0].text = PlayerPrefs.GetInt("WheatFieldCount").ToString();
                        moneyPerText[0].text = PlayerPrefs.GetInt("WheatFieldFood").ToString();
                        foodPerText[0].text = PlayerPrefs.GetInt("WheatFieldMoney").ToString();
                        break;

                    case 1:
                        fieldCountText[1].text = PlayerPrefs.GetInt("RiceFieldCount").ToString();
                        moneyPerText[1].text = PlayerPrefs.GetInt("RiceFieldFood").ToString();
                        foodPerText[1].text = PlayerPrefs.GetInt("RiceFieldMoney").ToString();
                        break;

                    case 2:
                        fieldCountText[2].text = PlayerPrefs.GetInt("CornFieldCount").ToString();
                        moneyPerText[2].text = PlayerPrefs.GetInt("CornFieldFood").ToString();
                        foodPerText[2].text = PlayerPrefs.GetInt("CornFieldMoney").ToString();
                        break;

                    case 3:
                        fieldCountText[3].text = PlayerPrefs.GetInt("CarrotFieldCount").ToString();
                        moneyPerText[3].text = PlayerPrefs.GetInt("CarrotFieldFood").ToString();
                        foodPerText[3].text = PlayerPrefs.GetInt("CarrotFieldMoney").ToString();
                        break;

                    case 4:
                        fieldCountText[4].text = PlayerPrefs.GetInt("CabbageFieldCount").ToString();
                        moneyPerText[4].text = PlayerPrefs.GetInt("CabbageFieldFood").ToString();
                        foodPerText[4].text = PlayerPrefs.GetInt("CabbageFieldMoney").ToString();
                        break;

                    case 5:
                        fieldCountText[5].text = PlayerPrefs.GetInt("TurnipFieldCount").ToString();
                        moneyPerText[5].text = PlayerPrefs.GetInt("TurnipFieldFood").ToString();
                        foodPerText[5].text = PlayerPrefs.GetInt("TurnipFieldMoney").ToString();
                        break;

                    case 6:
                        fieldCountText[6].text = PlayerPrefs.GetInt("CowFieldCount").ToString();
                        moneyPerText[6].text = PlayerPrefs.GetInt("CowFieldFood").ToString();
                        foodPerText[6].text = PlayerPrefs.GetInt("CowFieldMoney").ToString();
                        break;

                    case 7:
                        fieldCountText[7].text = PlayerPrefs.GetInt("ChickenFieldCount").ToString();
                        moneyPerText[7].text = PlayerPrefs.GetInt("ChickenFieldFood").ToString();
                        foodPerText[7].text = PlayerPrefs.GetInt("ChickenFieldMoney").ToString();
                        break;

                    case 8:
                        fieldCountText[8].text = PlayerPrefs.GetInt("PigFieldCount").ToString();
                        moneyPerText[8].text = PlayerPrefs.GetInt("PigFieldFood").ToString();
                        foodPerText[8].text = PlayerPrefs.GetInt("PigFieldMoney").ToString();
                        break;

                    case 9:
                        fieldCountText[9].text = PlayerPrefs.GetInt("SugarFieldCount").ToString();
                        moneyPerText[9].text = PlayerPrefs.GetInt("SugarFieldFood").ToString();
                        foodPerText[9].text = PlayerPrefs.GetInt("SugarFieldMoney").ToString();
                        break;

                    case 10:
                        fieldCountText[10].text = PlayerPrefs.GetInt("SunflowerFieldCount").ToString();
                        moneyPerText[10].text = PlayerPrefs.GetInt("SunflowerFieldFood").ToString();
                        foodPerText[10].text = PlayerPrefs.GetInt("SunflowerFieldMoney").ToString();
                        break;

                    case 11:
                        fieldCountText[11].text = PlayerPrefs.GetInt("CocoaFieldCount").ToString();
                        moneyPerText[11].text = PlayerPrefs.GetInt("CocoaFieldFood").ToString();
                        foodPerText[11].text = PlayerPrefs.GetInt("CocoaFieldMoney").ToString();
                        break;

                    case 12:
                        fieldCountText[12].text = PlayerPrefs.GetInt("BatteryCount").ToString();
                        moneyPerText[12].text = PlayerPrefs.GetInt("BatteryFood").ToString();
                        foodPerText[12].text = PlayerPrefs.GetInt("BatterybMoney").ToString();
                        break;

                    case 13:
                        fieldCountText[13].text = PlayerPrefs.GetInt("MeatLabCount").ToString();
                        moneyPerText[13].text = PlayerPrefs.GetInt("MeatLabFood").ToString();
                        foodPerText[13].text = PlayerPrefs.GetInt("MeatLabMoney").ToString();
                        break;

                    case 14:
                        fieldCountText[14].text = PlayerPrefs.GetInt("HydroCount").ToString();
                        moneyPerText[14].text = PlayerPrefs.GetInt("HydroFieldFood").ToString();
                        foodPerText[14].text = PlayerPrefs.GetInt("HydroFieldMoney").ToString();
                        break;

                    default:
                        return;
                }
            }

            gathered = true;
        }
    }
}
