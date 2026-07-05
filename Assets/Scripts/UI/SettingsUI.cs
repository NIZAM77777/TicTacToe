using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;


    private void Start()
    {
        volumeSlider.value = AudioListener.volume;
    }

    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    
  
}