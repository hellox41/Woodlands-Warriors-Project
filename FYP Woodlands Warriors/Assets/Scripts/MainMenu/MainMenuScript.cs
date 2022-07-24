using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    public GameObject settingsObj;

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
    }

    public void CloseUI(string buttonType)
    {
        if (buttonType == "SETTINGS")
        {
            settingsObj.SetActive(false);
        }
    }
}
