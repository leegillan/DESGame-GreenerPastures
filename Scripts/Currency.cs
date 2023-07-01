using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Currency : MonoBehaviour
{
    //public variable to set in inspector
    public GameObject gameManager;

    //Declare money variable
    public int money;

    //getter and setter
    public int GetMoney() { return money; }
    public void SetMoney(int mon) { money = mon; }

    //Add to current money
    public void AddMoney(int m) 
    {
        //checks if scene is not tutorial
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("TutorialScene"))
        {
            //if money is greater than 0
            if (m > 0)
            {
                gameManager.GetComponent<GameLoop>().AddToTotalMoneyEarned(m);
            }
            else
            {
                gameManager.GetComponent<GameLoop>().AddToTotalMoneySpent(m);
            }
        }

        //add money value
        money = money + m; 
    }
}
