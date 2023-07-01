using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    //public variable to be set in inspector
    public VideoPlayer videoPlayer;

    //start is called before the first frame update
    void Start()
    {
        //plays video untile its finished and then loads the main menu
        videoPlayer.loopPointReached += LoadMainMenu;
    }

    //Loads menu scene
    void LoadMainMenu(VideoPlayer vp)
    {
        SceneManager.LoadSceneAsync(1);
    }
}
