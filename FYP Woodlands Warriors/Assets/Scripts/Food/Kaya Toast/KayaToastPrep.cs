using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KayaToastPrep : MonoBehaviour
{
    public string[] breadTypes = { "MULTIGRAIN", "WHOLEWHEAT", "HONEYOAT" };

    public int[] dishTimes = { 75, 85, 100, 125 };

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
    public Stove stove;
    public int timesFlippedCorrectly = 0;
    public bool isFlippedOnThisColor = false;

    [Space]

    [Header("Spreading Prep Values")]
    public bool isKayaAppliedToKnife = false;
    public bool isButterAppliedToKnife = false;

    public TMP_Text condimentText;

    [Header("Progress")]
    public float savedCutBreadProgress;
    public float savedToastBreadProgress;

    public int preparedCount = 0;

    public void CutBread()  //Cutting minigame
    {
        cutCanvas.gameObject.SetActive(true);
        GameManagerScript.instance.orders.prepProgressBar.ResetValue();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(5);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedCutBreadProgress);
    }

    public void ToastBread()  //Toasting minigame
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetValue();

        if (breadType == "MULTIGRAIN" || breadType == "HONEYOAT")
        {
            GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(2);
        }

        if (breadType == "WHOLEWHEAT")
        {
            GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(3);
        }

        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedToastBreadProgress);
    }

    public void ResetVariables()
    {
        isBreadCut = false;
        isBreadToasted = false;
        isBreadSpreadButter = false;
        isBreadSpreadKaya = false;
        timesFlippedCorrectly = 0;

        savedCutBreadProgress = 0;
        savedToastBreadProgress = 0;
    }
}
