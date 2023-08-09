using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider, sfxSlider;

    void Start()
    {
        // Default music volume settings
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
    }

    public void SetVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
        // Save music volume settings
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

}
