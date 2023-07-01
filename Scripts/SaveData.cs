using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//sets class to be serilisable so that it can be formatted into binary
[System.Serializable]
public class SaveData
{
    //Variables to be saved
    public int money;
    public float food;
    public int totalMoneyEarned;
    public int totalMoneySpent;
    public float totalFood;
    public float totalPeopleFed;
    public float totalTimePlayed;
    public float quotaTimer;
    public int quota;
    public float sustainabilityLevel;
    public string distributerChoice;
   
    //grid data to be saved
    public string[] gridType;
    public int[] gridLevel;
    public int[] gridID;
    public string[] gridFill;
    public float[] gridPos;

    //saves data
    public SaveData(int moneyData, float foodData, List<GameObject> gridData, float quotaTimerData, int quotaData, float sustainabilityLevelData, string distributerChoiceData, int totalMoneyEarnedData, int totalMoneySpentData, float totalFoodData, float totalTimePlayedData, float totalPeopleFedData)
    {
        //sets variables with passed information
        money = moneyData;
        food = foodData;
        totalMoneyEarned = totalMoneyEarnedData;
        totalMoneySpent = totalMoneySpentData;
        totalFood = totalFoodData;
        totalPeopleFed = totalPeopleFedData;
        totalTimePlayed = totalTimePlayedData;
        quotaTimer = quotaTimerData;
        quota = quotaData;
        sustainabilityLevel = sustainabilityLevelData;
        distributerChoice = distributerChoiceData;

        //sets up arrays to be stored in
        gridType = new string[25];
        gridLevel = new int[25];
        gridID = new int[25];
        gridFill = new string[25];
        gridPos = new float[75];

        //sets count to 0
        int count = 0;

        //loops through each grid tile to save the data about each grid in the game
        for (int i = 0; i < 25; i++)
        {
            //saves current grid tiles information
            gridType[i] = gridData[i].GetComponent<ObjectInfo>().GetObjectType().ToString();
            gridLevel[i] = gridData[i].GetComponent<ObjectInfo>().GetObjectLevel();
            gridID[i] = gridData[i].GetComponent<ObjectInfo>().GetObjectID();
            gridFill[i] = gridData[i].GetComponent<ObjectFill>().GetFillType().ToString();
            gridPos[count] = gridData[i].transform.localPosition.x;
            gridPos[count + 1] = gridData[i].transform.localPosition.y;
            gridPos[count + 2] = gridData[i].transform.localPosition.z;

            //adds 3 as to not save over the x, y and z values of each grid tile
            count += 3;
        }
    }
}
