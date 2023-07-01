using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using TMPro;

public class FieldStats : MonoBehaviour
{
    //Public variables to be assigned in inspector
    public GameObject gameManager;

    public TextMeshProUGUI fieldCountText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI foodText;

    public ObjectInfo.ObjectType tileType;
    public ObjectFill.FillType fillType;

    //variables used only in script
        //money
    int[] money;
    int moneyPer;
    int totalMoneyOut;

        //food
    int[] food;
    int foodPer;
    int totalFoodOut;

        //count
    int tileCount;

    //Gathers farm stats by looping through grid
    public void GatherStats()
    {
        //gets grid data and stores it in a list
        List<GameObject> grid = gameManager.GetComponent<GridScript>().GetGrid();

        //sets values to 0 to assure values are correct
        tileCount = 0;
        totalFoodOut = 0;
        totalMoneyOut = 0;

        //for each grid square check the data
        for (int i = 0; i < grid.Count; i++)
        {
            //checks if grid equals the desired type and fill of the script
            if(grid[i].GetComponent<ObjectInfo>().GetObjectType() == tileType && grid[i].GetComponent<ObjectFill>().GetFillType() == fillType)
            {
                //adds to amount of this field
                tileCount++;

                //checks if the fields parent has an output
                if(grid[i].TryGetComponent<ObjectOutput>(out ObjectOutput output))
                {
                    //get money from the parent
                    money = output.GetTileMoneyOutput();
                    food = output.GetTileFoodOutput();
                }
                else //get money from the child
                {
                    money = grid[i].GetComponentInChildren<ObjectOutput>().GetTileMoneyOutput();
                    food = grid[i].GetComponentInChildren<ObjectOutput>().GetTileFoodOutput();
                }

                //sets the money/food values to the correct value using the grid tile's level
                moneyPer = money[grid[i].GetComponent<ObjectInfo>().GetObjectLevel() - 1];
                foodPer = food[grid[i].GetComponent<ObjectInfo>().GetObjectLevel() - 1];

                //adds to the total of money to show in the stats menu
                totalMoneyOut += moneyPer;
                totalFoodOut += foodPer;
            }
        }

        //set text values to the values
        fieldCountText.text = tileCount.ToString();
        moneyText.text = totalMoneyOut.ToString();
        foodText.text = totalFoodOut.ToString();

        //save the field stats for the end screen to have the correct stats
        gameManager.GetComponent<GameLoop>().SaveFieldStats(tileType, fillType, tileCount, totalMoneyOut, totalFoodOut);
    }
}
