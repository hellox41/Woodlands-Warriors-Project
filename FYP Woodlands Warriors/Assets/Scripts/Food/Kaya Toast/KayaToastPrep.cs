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

    [Header("Prep Values")]
    public Canvas prepCanvas;
    public GameObject[] cutButtons;


    public void CutBread()  //Cutting minigame
    {
        prepCanvas.gameObject.SetActive(true);
    }

    public void OnCutButtonPressed(int buttonIndex)
    {
        cutButtons[buttonIndex].SetActive(false);

        if (buttonIndex == 4)
        {
            isBreadCut = true;
            prepCanvas.gameObject.SetActive(false);
        }

        else
        {
            cutButtons[buttonIndex + 1].SetActive(true);
        }
    }
}
