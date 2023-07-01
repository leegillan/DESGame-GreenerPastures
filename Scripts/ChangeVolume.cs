using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolume : MonoBehaviour
{
    //Public variables to be set in inspector
    public Slider thisSlider;
    public float masterVolume;
    public float musicVolume;
    public float SFXVolume;
 
    //Changes sets volume of slider being used
    public void SetSpecificVolume(string valueType)
    {
        //slider value that gets changed
        float sliderValue = thisSlider.value;

        //checks which slider is being changed - Master
        if (valueType == "Master")
        {
            //sets volume to slider volume
            masterVolume = thisSlider.value;

            //sets volume for wwise
            AkSoundEngine.SetRTPCValue("MasterVolume", masterVolume);
        }

        //checks which slider is being changed - Music
        if (valueType == "Music")
        {
            //sets volume to slider volume
            musicVolume = thisSlider.value;

            //sets volume for wwise
            AkSoundEngine.SetRTPCValue("MusicVolume", musicVolume);
        }

        //checks which slider is being changed - Sounds
        if (valueType == "Sounds")
        {
            //sets volume to slider volume
            SFXVolume = thisSlider.value;

            //sets volume for wwise
            AkSoundEngine.SetRTPCValue("SFXVolume", SFXVolume);
        }
    }
}
