
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class FadeUIScript : MonoBehaviour
{
    //public variables to be set in inspector
    public float fadeSpeed;
    public float fadeTime;

    // Start is called before the first frame update
    void Start()
    {
        //sets new canvas group
        var canvGroup = GetComponent<CanvasGroup>();

        //sets alpha to 0 so that it fades in
        canvGroup.alpha = 0.0f;

        //starts coroutine so that it fades in with eveything else going on
        StartCoroutine(FadeToFullAlpha(canvGroup, canvGroup.alpha, 1.0f));
    }

    //fades UI to full alpha
    public IEnumerator FadeToFullAlpha(CanvasGroup canvGroup, float start, float end)
    {
        //sets counter
        float counter = 0.0f;

        //loops while counter is smaller that fade time
        while(counter < fadeTime)
        {
            //adds delta time (time between frames) to counter mul;tipled by the speed
            counter += Time.deltaTime * fadeSpeed;

            //canvas alpha will lerp to the next value
            canvGroup.alpha = Mathf.Lerp(start, end, counter / fadeTime);

            //will return null and wont finish function if the condition is not met
            yield return null;
        }
    }
}