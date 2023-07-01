using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save : MonoBehaviour
{
    //public varaible to be set in inspector
    public GameObject gameManager;

    //saves current game data
    public void SaveGameData()
    {   
        //saves game data by passing through getter sof each thing needed saved
        SaveGame.SaveGameData(gameManager.GetComponent<Currency>().GetMoney(), gameManager.GetComponent<FoodScript>().GetFood(), gameManager.GetComponent<GridScript>().GetGrid(), gameManager.GetComponent<FoodScript>().GetQuotaTimer(), gameManager.GetComponent<FoodScript>().GetCurrentQuota(), gameManager.GetComponent<SustainabilityScript>().GetSustainability(), DistributionChoice.instance.GetDistributionChoice(), gameManager.GetComponent<GameLoop>().GetTotalMoneyEarned(), gameManager.GetComponent<GameLoop>().GetTotalMoneySpent(), gameManager.GetComponent<GameLoop>().GetTotalFood(), gameManager.GetComponent<GameLoop>().GetTotalTimePlayed(), gameManager.GetComponent<GameLoop>().GetTotalPeopleFed());
    }

    //loads data into variables to be used in game
    public void LoadGameData()
    {
        //gets data stored in save file
        SaveData data = SaveGame.LoadGameData();

        //checks if there is data to be loaded into the game
        if (data != null && SaveGame.LoadGameData() != null)
        {
            //Sets food and money data
            gameManager.GetComponent<Currency>().SetMoney(data.money);
            gameManager.GetComponent<FoodScript>().SetFood(data.food);

            //sets total values
            gameManager.GetComponent<GameLoop>().SetTotalMoneyEarned(data.totalMoneyEarned);
            gameManager.GetComponent<GameLoop>().SetTotalMoneySpent(data.totalMoneySpent);
            gameManager.GetComponent<GameLoop>().SetTotalFood(data.totalFood);
            gameManager.GetComponent<GameLoop>().SetTotalFood(data.totalPeopleFed);
            gameManager.GetComponent<GameLoop>().SetTotalTimePlayed(data.totalTimePlayed);

            //Loads quota data
            gameManager.GetComponent<FoodScript>().SetQuotaTimer(data.quotaTimer);
            gameManager.GetComponent<FoodScript>().SetCurrentQuota(data.quota);

            //loads sustainability data
            gameManager.GetComponent<SustainabilityScript>().SetSustainability(data.sustainabilityLevel);

            //load distributer data
            DistributionChoice.instance.SetDistributionChoice(data.distributerChoice);
            DistributionChoice.instance.SetDistribubuterButtons(data.distributerChoice);

            //loads grid data back into the required data sets
            Vector3[] pos;
            pos = new Vector3[25];

            //create new variables to save grid data into
            int[] level = new int[25];
            int[] id = new int[25];
            string[] type = new string[25];
            string[] fill = new string[25];

            //sets count to 0
            int count = 0;

            //loops through how many grid spaces there are
            for (int i = 0; i < 25; i++)
            {
                //sets grid data ro what has been loaded
                type[i] = data.gridType[i];
                level[i] = data.gridLevel[i];
                id[i] = data.gridID[i];
                fill[i] = data.gridFill[i];
                pos[i].x = data.gridPos[count];
                pos[i].y = data.gridPos[count + 1];
                pos[i].z = data.gridPos[count + 2];

                //skips ahead 3 to not save over x, y and z
                count += 3;
            }

            //calls load grid function and passes through new data to allow for the grid to be updated to saved version
            gameManager.GetComponent<GridScript>().LoadGrid(id, pos, type, level, fill);
        }
        else
        {
            //returns out of function if no data is found
            return;
        }
    }

    public void DeleteGameData()
    {
        //calls delete game function
        SaveGame.DeleteGameData();
    }
}
