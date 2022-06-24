using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KayaToastPrep : MonoBehaviour
{
    public string[] breadTypes = { "MULTIGRAIN", "WHOLEWHEAT", "HONEYOAT" };

    // public string[] 

    [Header("Bread Values")]
    public bool isBreadCut = false;
    public bool isBreadToasted = false;
    public bool isBreadSpreadButter = false;
    public bool isBreadSpreadKaya = false;

    public string breadType;

    [Header("Cut Prep Values")]
    public Canvas cutCanvas;
    public GameObject[] cutButtons;

    [Space]

    [Header("Toasting Prep Values")]
    public GameObject powerButton;

    public void CutBread()  //Cutting minigame
    {
        cutCanvas.gameObject.SetActive(true);
    }

    public void OnCutButtonPressed(int buttonIndex)
    {
        cutButtons[buttonIndex].SetActive(false);

        if (buttonIndex == 4)
        {
            isBreadCut = true;
            cutCanvas.gameObject.SetActive(false);
        }

        else
        {
            cutButtons[buttonIndex + 1].SetActive(true);
        }
    }

    public void ToastBread()  //Toasting minigame
    {
        powerButton.layer = LayerMask.NameToLayer("Default");
    }
}
