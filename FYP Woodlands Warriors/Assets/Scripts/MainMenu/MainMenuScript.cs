using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject settingsObj;
    public GameObject creditsObj;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenUI(string buttonType)
    {
        if (buttonType == "SETTINGS")
        {
            settingsObj.SetActive(true);
        }

        if (buttonType == "CREDITS")
        {
            creditsObj.SetActive(true);
        }
    }

    public void CloseUI(string buttonType)
    {
        if (buttonType == "SETTINGS")
        {
            settingsObj.SetActive(false);
        }

        if (buttonType == "CREDITS")
        {
            creditsObj.SetActive(false);
        }
    }

    public void ButtonOpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
