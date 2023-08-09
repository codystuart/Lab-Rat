using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string volumeParameter = "MusicVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;

    private void Awake() 
    {
        slider.onValueChanged.AddListener(SetVolume);    
    }

    public void SetVolume(float value)
    {
        mixer.SetFloat(volumeParameter, Mathf.Log10(value) * 20f);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
