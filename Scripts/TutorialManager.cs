using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.XR;

public class TutorialManager : MonoBehaviour
{
    //Public variables to be set in inspector
    public GameObject gameManager;
    public GameObject cam;
    public GameObject TutorialBox;
    public RuntimeAnimatorController animContr;
    public Button[] tutMsgs;
    public TextMeshProUGUI countdownText;
    public static TutorialManager instance;

    //Tutorial buttons
    Button currentTut;
    Button prevTut;

    //Tutorial varibales
    bool inTutorial;
    bool updateTutorial = false;
    bool showTutorialBox;

    //End of tutorail varibales
    bool endOfTut = false;
    float endOfTutCountdown;
    float endCountdownTime;
    bool showCountdown;

    public bool GetTutorial() { return inTutorial; }
    public GameObject GetTutorialBox() { return TutorialBox; }
    public RuntimeAnimatorController GetAnimContr() { return animContr; }

    //Setters
    public void SetCurrentTut(Button c) { currentTut = c; }
    public void SetPreviousTut(Button p) { prevTut = p; }
    public void UpdateTutorialBox(bool b) { updateTutorial = b; }
    public void SetTutorialBox(bool t) { showTutorialBox = t; }
    public void SetEndOfTut(bool e) { endOfTut = e; }

    void Awake()
    {
        //sets instance
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //New animator
        Animator camAnim;

        //sets booleans
        InputScript.instance.SetCanMove(false);
        InputScript.instance.SetAllowSelecting(false);

        //sets in tutorail to true
        inTutorial = true;

        //tries to get an animator component from camera
        if (cam.TryGetComponent(out camAnim))
        {
            //sets message to show
            currentTut = tutMsgs[0];
            prevTut = currentTut;

            //Waits until animation is done to perform next task
            camAnim.enabled = true;
            camAnim.Play("TutorialStartPan");
            StartCoroutine(UpdateClipLength(camAnim, true));
        }

        //sets countdown time
        endCountdownTime = 20.0f;
    }

    // Update is called once per frame
    void Update()
    {
        //Updates button to be displayed
        if(updateTutorial)
        {
            ChangeTutMsg();
            updateTutorial = false;
        }

        //Displays tutorial box
        if(showTutorialBox == true)
        {
            InputScript.instance.SetAllowSelecting(false);
            TutorialBox.SetActive(true);
            showTutorialBox = false;
        }

        //End of tutorial
        if(endOfTut == true)
        {
            if(showCountdown == false)
            {
                showCountdown = true;
                countdownText.gameObject.SetActive(true);
            }
        
            endOfTutCountdown += Time.deltaTime;
            endCountdownTime -= 1 * endOfTutCountdown;

            countdownText.text = "Time until game starts:" + endCountdownTime;

            //if countdown has ended
            if (endCountdownTime <= 0)
            {
                //loads game
                EndTutorial();

                endOfTut = false;
            }
        }
    }

    public void ChangeTutMsg()
    {
        //Changes what is being shown in the tutorial box
        prevTut.gameObject.SetActive(false);

        //sets next message to show
        currentTut.gameObject.SetActive(true);
        currentTut.interactable = false;

        //waits to make the button interactive
        StartCoroutine(WaitForButton());
    }

    public IEnumerator WaitForAnimationToShowTutorialBox(Animator anim, bool showTutBox)
    {
        //sets value to count to
        float counter = 0;
        float waitTime = anim.GetCurrentAnimatorStateInfo(0).length;

        //Now, Wait until the current state is done playing
        while (counter < (waitTime))
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //shows tutorial box or not
        if(showTutBox == true)
        {
            TutorialBox.SetActive(true);
        }
        else
        {
            TutorialBox.SetActive(false);
        }

        updateTutorial = true;

        //sets component to false
        anim.enabled = false;
    }

    //waits for animation
    public IEnumerator WaitForAnimation(Animator anim)
    {
        //sets counter values
        float counter = 0;
        float waitTime = anim.GetCurrentAnimatorStateInfo(0).length;

        //Now, Wait until the current state is done playing
        while (counter < (waitTime))
        {
            counter += Time.deltaTime;
            yield return null;
        }

        updateTutorial = true;
    }

    //waits to make button interactable
    public IEnumerator WaitForButton()
    {
        //sets counter values
        float counter = 0;
        float waitTime = 1.0f;

        //Now, Wait until the current state is done playing
        while (counter < (waitTime))
        {
            counter += Time.deltaTime;
            yield return null;
        }

        currentTut.interactable = true;
    }

    //waits for frame to update animation length
    public IEnumerator UpdateClipLength(Animator anim, bool waitFortutBox)
    {
        bool newFrame = false;

        while (!newFrame)
        {
            newFrame = true;
            yield return new WaitForEndOfFrame();
        }

        //checks if waiting to show tutorial box is true
        if (waitFortutBox == true)
        {
            StartCoroutine(WaitForAnimationToShowTutorialBox(anim, true));
        }
        else
        {
            StartCoroutine(WaitForAnimation(anim));
        }
    }

    //ends tutorial
    public void EndTutorial()
    {
        //loads last scene
        SceneLoader.instance.LoadScene(3);
        inTutorial = false;
    }
}
