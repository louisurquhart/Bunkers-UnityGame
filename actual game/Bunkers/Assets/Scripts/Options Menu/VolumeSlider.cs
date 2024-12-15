using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider VolSlider; // creates an instants of the slider class so slider stuff can be accessed
    [SerializeField] TextMeshProUGUI VolSliderText; // creates an instant of textmeshprougui class to change the text

    private void Start()
    {
        Debug.Log("Loading saved volume value"); // outputs that the start functions being executed for testing purposes
        float initialvolume = PlayerPrefs.GetFloat("GameVolume", 50); // if player had a previous volume value it loads it
        VolSliderText.text = initialvolume.ToString();  // sets the text to previous value
        VolSlider.value = initialvolume; // sets the slider to previous value
    }

    public void VolumeChange()
    {
        VolSliderText.text = VolSlider.value.ToString(); // changes text next to volume slider to the current volume
        AudioListener.volume = VolSlider.value/100; // sets the current volume to the value
        Console.WriteLine("Current volume is: ", VolSlider.value); // debugging message
        PlayerPrefs.SetFloat("GameVolume", VolSlider.value); // saves the selected volume for next time
    }
}
