using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    //public variables to be set in inspector
    public GameObject loadingScreen;
    public Slider loadSlider;
    public TextMeshProUGUI progressText;
    public Animator EndScreenLoader;

    //Static instance of the class
    public static SceneLoader instance;

    void Start()
    {
        //sets instance
        instance = this;
    }

    //Starts loading scene bar 
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    //laods end screen
    public void LoadEndScene(int sceneIndex)
    {
        StartCoroutine(LoadEndScreen(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        //sets operation to use for the loading bar
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        //sets laoding screen to show
        loadingScreen.SetActive(true);

        //loops while loading neext scene
        while(!operation.isDone)
        {
            //sets preogress value to the operation prgress (loading scene)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            //sets value to progress
            loadSlider.value = progress;

            //sets value to progress and multiplies to 100 to show 0-100
            progressText.text = progress * 100 + "%";


            //returns bull until condition is met
            yield return null;
        }
    }

    IEnumerator LoadEndScreen(int sceneIndex)
    {
        //starts fade anim
        EndScreenLoader.SetTrigger("Start");

        //waits for the amount of time the animation takes (1s)
        yield return new WaitForSeconds(1.0f);

        //loads end scene
        SceneManager.LoadSceneAsync(sceneIndex);
    }

}
