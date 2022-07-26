using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;
    private void Start()
    {
        audioMixer.SetFloat("musicVolume", -10);
    }
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);

        if (volume <= -40)
        {
            audioMixer.SetFloat("musicVolume", -80);
        }
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);

        if (volume <= -40)
        {
            audioMixer.SetFloat("sfxVolume", -80);
        }
    }
}
