using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveGame
{
    //saves game data 
    public static void SaveGameData(int moneyData, float foodData, List<GameObject> gridData, float quotaTimerData, int quotaData, float sustainabilityLevelData, string distributerChoiceData, int totalMoneyEarnedData, int totalMoneySpentData, float totalFoodData, float totalTimePlayedData, float totalPeopleFedData)
    { 
        //creates a new formatter
        BinaryFormatter formatter = new BinaryFormatter();

        //sets path to save to
        string path = Application.persistentDataPath + "/saveData.SaveData";
        
        //sets stream to save from
        FileStream stream = new FileStream(path, FileMode.Create);

        //saves new data by sending through passed getters
        SaveData data = new SaveData(moneyData, foodData, gridData, quotaTimerData, quotaData, sustainabilityLevelData, distributerChoiceData, totalMoneyEarnedData, totalMoneySpentData, totalFoodData, totalTimePlayedData, totalPeopleFedData);

        //formats data into binary to be saved as
        formatter.Serialize(stream, data);

        //closes stream to stop transferring
        stream.Close();
    }

    //load game data
    public static SaveData LoadGameData()
    {
        //sets path to check
        string path = Application.persistentDataPath + "/saveData.SaveData";

        //checks if path exists
        if (File.Exists(path))
        {
            //creates a new formatter
            BinaryFormatter formatter = new BinaryFormatter();

            //sets stream to read from
            FileStream stream = new FileStream(path, FileMode.Open);

            //Cast as save data type and formats data back to original
            SaveData data = formatter.Deserialize(stream) as SaveData;

            //close file stream
            stream.Close();

            //returns data to be used
            return data;
        }
        else
        {
            //return null if no save path found
            return null;
        }
    }

    //delete game data 
    public static void DeleteGameData()
    {
        //set path to check
        string path = Application.persistentDataPath + "/saveData.SaveData";

        //if path foudn then delete data
        if (File.Exists(path))
        {
            //deletes fle at path
            File.Delete(path);
        }
        else
        {
            //returns if no path found
            return;
        }
    }
}

