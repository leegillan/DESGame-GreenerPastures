using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhileAwake : MonoBehaviour
{
    //variable used only in script
    GameObject gameManager;

    private void Start()
    {
        //sets gameManager to the game manager in the scene
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        //checks if the selecting is true and if the gameobject the script is attached to is active
        if (gameManager.GetComponent<InputScript>().GetAllowSelecting() == true && gameObject.activeSelf == true)
        {
            //sets selecting to false to ovveride any changes made
            gameManager.GetComponent<InputScript>().SetAllowSelecting(false);
        }
    }
}
