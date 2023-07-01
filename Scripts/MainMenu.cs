using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class MainMenu : MonoBehaviour
{
    //public variables to be set in inspector
    public GameObject NewGameConfirm;
    public GameObject NoSaveNotice;

    //checks if there is a save on the devices local game files
    public void CheckNewGame()
    {
        //sets path to file to check
        string path = Application.persistentDataPath + "/saveData.SaveData";

        //if the path exists then the new game warning will appear, prompting the player o choose confirm or not
        if (File.Exists(path))
        {
            //set warning to show
            NewGameConfirm.SetActive(true);
        }
        else //will automatically load new game as there is no save to be deleted
        {
            //loads new game into tutorial to show player around
            SceneLoader.instance.LoadScene(2);

            //sets the player prefs to be able to transfer data between scenes so that the game knows not to load
            PlayerPrefs.SetInt("loadGame", 0);
        }
    }

    //plays new game when confirm button in warning is pressed
    public void PlayNewGame()
    {
        //loads new game into tutorial to show player around
        SceneLoader.instance.LoadScene(2);

        //sets the player prefs to be able to transfer data between scenes so that the game knows not to load
        PlayerPrefs.SetInt("loadGame", 0);
    }

    //load saved file
    public void LoadSavedGame()
    {
        //sets path to file to check
        string path = Application.persistentDataPath + "/saveData.SaveData";

        //if the path exists then the game will load into saved game
        if (File.Exists(path))
        {
            //loads game into actual game scene
            SceneLoader.instance.LoadScene(3);

            //sets loading so that the game knows to the save file 
            PlayerPrefs.SetInt("loadGame", 1);
        }
        else //path not found meaning the player will get a notice saying no save file found
        {
            //shows no save notice
            NoSaveNotice.SetActive(true);
        }
    }

    //goes back to menu scene
    public void BackToMenu()
    {
        //loads menu scene
        SceneLoader.instance.LoadScene(1);
    }

    //Quits application
    public void QuitGame()
    {
        //Quits
        Application.Quit();
    }
}
