using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameStatisticsScript : MonoBehaviour
{
    //Public variables to be assigned in inspector
    public GameObject gameManager;

    public TextMeshProUGUI earnedMoneyText;
    public TextMeshProUGUI spentMoneyText;
    public TextMeshProUGUI producedFoodText;
    public TextMeshProUGUI peopleFedText;
    public TextMeshProUGUI timePlayedText;

    //variables only used in script
        //money
    int moneyEarned;
    int moneySpent;
    float foodProduced;

        //food
    int peopleFed;
    int curPeopleFed;
    int prevPeopleFed;

        //time
    float curTimePlayed;
    float prevTimePlayed;
    float timePlayedSinceLast;

    //Gathers game stats to be shown in menu
    public void GatherGameStats()
    {
        //gets values to be converted
        moneyEarned = gameManager.GetComponent<GameLoop>().GetTotalMoneyEarned();
        moneySpent = gameManager.GetComponent<GameLoop>().GetTotalMoneySpent();
        foodProduced = gameManager.GetComponent<GameLoop>().GetTotalFood();

        //sets previous value of people fed
        prevPeopleFed = curPeopleFed;
        //sets value of people fed
        curPeopleFed = (int)Mathf.Pow(foodProduced, 1.01f);
        //sets people fed to add to total
        peopleFed = (curPeopleFed - prevPeopleFed);

        //Adds to total amount 
        gameManager.GetComponent<GameLoop>().AddToTotalPeopleFed(peopleFed);

        //sets previous value of time played
        prevTimePlayed = curTimePlayed;
        //sets value of time played
        curTimePlayed = Time.timeSinceLevelLoad;
        //sets time played to add to total
        timePlayedSinceLast = (curTimePlayed - prevTimePlayed);

        //Adds to total time played
        gameManager.GetComponent<GameLoop>().AddToTotalTimePlayed(timePlayedSinceLast);

        //gets time since the scene was loaded
        System.TimeSpan t = System.TimeSpan.FromSeconds(gameManager.GetComponent<GameLoop>().GetTotalTimePlayed());

        //Sets all values to strings to be displayed
        earnedMoneyText.text = moneyEarned.ToString();
        spentMoneyText.text = moneySpent.ToString();
        producedFoodText.text = foodProduced.ToString();
        timePlayedText.text = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", t.Hours, t.Minutes, t.Seconds);
        peopleFedText.text = gameManager.GetComponent<GameLoop>().GetTotalPeopleFed().ToString();

        //saves game stats to be coorect when used in end screen
        gameManager.GetComponent<GameLoop>().SaveGameStats(moneyEarned, moneySpent, foodProduced, gameManager.GetComponent<GameLoop>().GetTotalTimePlayed(), gameManager.GetComponent<GameLoop>().GetTotalPeopleFed());
    }
}
