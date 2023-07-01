using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FoodScript : MonoBehaviour
{
    //public variables to be set in inspector
    public GameObject gameManager;
    public TextMeshProUGUI quotaFailCountText;

    //Declare food variable
    public float food;
    float foodOver;
    public float currentFood;
    public Image foodBar;

    //Declare time variables 
    public float[] quotaTime;
    bool timerStart = false;
    public float currentTime;
    public Image timeBar;

    //Amount required for quota
    public float[] quotaAmount;
    int currentQuota;
    int quotaCount = 0;
    bool overQuota = false;

    //money gained after food extracted
    int moneyGain;

    //money gained after food extracted
    int failToFill;

    //distributer choice
    public string Distributer;

    //getters
    public float GetFood() { return currentFood; }
    public int GetCurrentQuota() { return quotaCount; }
    public float GetQuotaTimer() { return currentTime; }

    //setters
    public void SetFood(float f) { currentFood = f; }
    public void SetCurrentQuota(int q) { quotaCount = q; }
    public void SetQuotaTimer(float t) { currentTime = t; }

    //Add to current food
    public void AddFood(float f)
    {
        //checks if in tutorial to not add to total counter
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("TutorialScene"))
        {   
            gameManager.GetComponent<GameLoop>().AddToTotalFood(f);
        }

        //adds to food count
        food = food + f; 
    }

    private void Start()
    {
        //Delay starting the functions
        InvokeRepeating("UpdateTimeBar", 5.0f, 0.1f);
    }

    //Update time bar
    void UpdateTimeBar()
    {
        //checks of timer to start has been started
        if (timerStart == false)
        {
            timerStart = true;
        }

        //updates quota time bar
        timeBar.fillAmount = currentTime / quotaTime[quotaCount];
    }

    void Update()
    {
        //checks what distibuter the player has chosen
        if (DistributionChoice.instance.GetDistributionChoice() == "BF")
        {
            //updates valoue based on choice
            currentFood += (food * 1.5f);
            food = 0;
        }
        else if (DistributionChoice.instance.GetDistributionChoice() == "P")
        {
            currentFood += food;
            food = 0;
        }
        else if (DistributionChoice.instance.GetDistributionChoice() == "GG")
        {
            currentFood += (food * 0.5f);
            food = 0;
        }

        //updates food bar 
        foodBar.fillAmount = currentFood / quotaAmount[quotaCount];

        //checks if curretn food if greater than the quota amount to update boolean
        if (currentFood > quotaAmount[quotaCount])
        {
            overQuota = true;
        }
        else
        {
            overQuota = false;
        }

        //if the player has not reached the quota then the colour of the bar updates depending on how full it is
        if (overQuota == false)
        {
            if (currentFood < quotaAmount[quotaCount] * 0.2f)
            {
                //Red Bar
                foodBar.color = Color.Lerp(foodBar.color, new Color(0.8773585f, 0.05067572f, 0.02069241f), Time.deltaTime * 0.8f);
            }
            else if (currentFood < quotaAmount[quotaCount] * 0.4f)
            {
                //Red-Orangey Bar
                foodBar.color = Color.Lerp(foodBar.color, new Color(0.9215686f, 0.2095846f, 0.126f), Time.deltaTime * 0.8f);
            }
            else if (currentFood < quotaAmount[quotaCount] * 0.6f)
            {
                //Orangey Bar
                foodBar.color = Color.Lerp(foodBar.color, new Color(0.895f, 0.629f, 0.14f), Time.deltaTime * 0.8f);
            }
            else if (currentFood < quotaAmount[quotaCount] * 0.8f)
            {
                //Yellowy Bar
                foodBar.color = Color.Lerp(foodBar.color, new Color(0.79f, 0.8301887f, 0.1292275f), Time.deltaTime * 0.8f);
            }
        }
        else if (overQuota == true) //
        {
            //Blue bar
            foodBar.color = Color.Lerp(foodBar.color, new Color(0.2509804f, 0.654902f, 0.9490197f), Time.deltaTime * 0.8f);
        }

        //if timer has started
        if (timerStart == true)
        {
            //Updates time variables for the time bar
            currentTime += Time.deltaTime;

            //if timer is bigger greater than quota time
            if (currentTime >= quotaTime[quotaCount])
            {
                //checks if the players food has met the quota
                if (currentFood > quotaAmount[quotaCount])
                {
                    //get amount over by
                    foodOver = currentFood - quotaAmount[quotaCount];

                    //money gained by the player for going over
                    moneyGain = (int)foodOver;

                    //checks distributer choice and does the requiored effect
                    if (DistributionChoice.instance.GetDistributionChoice() == "BF")
                    {
                        moneyGain = Mathf.RoundToInt(moneyGain / 10) * 15;
                    }
                    else if (DistributionChoice.instance.GetDistributionChoice() == "P")
                    {
                        moneyGain = Mathf.RoundToInt(moneyGain / 10) * 10;
                    }
                    else if (DistributionChoice.instance.GetDistributionChoice() == "GG")
                    {
                        moneyGain = Mathf.RoundToInt(moneyGain / 10) * 5;
                    }

                    //round money gained
                    moneyGain = Mathf.RoundToInt(moneyGain / 10) * 15;

                    //adds money
                    gameManager.GetComponent<Currency>().AddMoney(moneyGain);

                    //if quota is not the last one
                    if (quotaCount != 5)
                    {
                        //add to count
                        quotaCount++;

                        //set quota
                        currentQuota = quotaCount;
                    }
                    else
                    {
                        //finish game if all quutas are met
                        gameManager.GetComponent<GameLoop>().FinishGame();
                    }
                }
                else if (currentFood < quotaAmount[quotaCount]) // quota not met
                {
                    foodOver = currentFood - quotaAmount[quotaCount];

                    moneyGain = (int)foodOver;

                    moneyGain = Mathf.RoundToInt(moneyGain / 10) * 10;

                    if (gameManager.GetComponent<Currency>().GetMoney() < Mathf.Abs(moneyGain))
                    {
                        gameManager.GetComponent<Currency>().SetMoney(0); //money cant go below 0
                    }
                    else
                    {
                        gameManager.GetComponent<Currency>().AddMoney(moneyGain);
                    }

                    //failure count
                    failToFill++;

                    //shows warning message about quota
                    quotaFailCountText.text = "" + failToFill;

                    //if failure count become 3 end game
                    if (failToFill == 3)
                    {
                        currentQuota = quotaCount;
                        Failure();
                        failToFill = 0;
                        quotaCount = currentQuota;
                    }
                    else 
                    {
                        //show quota warning
                        gameManager.GetComponent<GameLoop>().GetQuotaWarning().gameObject.SetActive(true);
                        gameManager.GetComponent<InputScript>().SetAllowSelecting(false);
                    }
                }

                //set values to 0
                currentTime = 0;
                currentFood = 0;
            }
        }
    }

    void Failure()
    {
        //fail game
        gameManager.GetComponent<GameLoop>().FailedGame();
    }
}