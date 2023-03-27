using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InGameMenu : MonoBehaviour
{
    public GameObject inGameMenuObj;

    public AudioMixer audioMixer;

    // Update is called once per frame
    void Update()
    {
        //Bring up the menu by pressing Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            inGameMenuObj.SetActive(!inGameMenuObj.activeInHierarchy);
            if (GameManagerScript.instance.isShowingInGameMenu == false)
            {
                //Open the menu
                OpenInGameMenu();
                return;
            }

            if (GameManagerScript.instance.isShowingInGameMenu == true)
            {
                //Close the menu
                CloseInGameMenu();
                return;
            }
        }
    }

    public void OpenInGameMenu()
    {
        Debug.Log("open");
        //Open the menu
        GameManagerScript.instance.ChangeCursorLockedState(false);
        GameManagerScript.instance.isShowingInGameMenu = true;
    }

    public void CloseInGameMenu()
    {
        Debug.Log("close");
        //Close the menu
        GameManagerScript.instance.ChangeCursorLockedState(true);
        GameManagerScript.instance.isShowingInGameMenu = false;
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

    public void QuitToMenu()
    {
        GameManagerScript.instance.levelLoader.LoadLevel(0);
        Time.timeScale = 1;
    }

    public void QuitToDesktop()
    {
        Application.Quit();
        Time.timeScale = 1;
    }
}
