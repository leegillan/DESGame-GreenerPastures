using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RadialButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{ 
    public Image circle;
    public Image symbol;
    public string title;
    public float speed = 8f;
    public RadialMenu myMenu;
    
    Color defaultColor;

    //animate buttons
    public void Anim()
    {
        StartCoroutine(AnimateButtonIn());
    }

    //animation
    IEnumerator AnimateButtonIn()
     {
        transform.localScale = Vector3.zero;
        float timer = 0f;

        while (timer < (1 / speed)) 
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.one * timer * speed;
            yield return null;
        }

        transform.localScale = Vector3.one;
     }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        //when mouse/tap chooses this button
        myMenu.selected = this;
        defaultColor = circle.color;
        circle.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //when mouse/tap chooses this button
        if (InputScript.instance.GetControlType() == true)
        {
            if (Time.time - InputScript.instance.touch0StartTime <= InputScript.instance.maxDurationForTap)
            {
                myMenu.RadialOption();
            }
            else
            {
                myMenu.selected = null;
            }
        }
        else
        {
            myMenu.RadialOption();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myMenu.selected = null;
        circle.color = defaultColor;
    }
}


