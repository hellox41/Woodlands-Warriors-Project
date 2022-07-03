using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KayaToastPrep : MonoBehaviour
{
    public string[] breadTypes = { "MULTIGRAIN", "WHOLEWHEAT", "HONEYOAT" };

    // public string[] 

    [Header("Bread Values")]
    public bool isBreadCut = false;
    public bool isBreadToasted = false;
    public bool isBreadSpreadButter = false;
    public bool isBreadSpreadKaya = false;

    public bool isBeingToasted = false;

    public float toastedTime = 0f;

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

    public void CutBread()  //Cutting minigame
    {
        cutCanvas.gameObject.SetActive(true);
        GameManagerScript.instance.orders.progressBar.ResetProgress();
        GameManagerScript.instance.orders.progressBar.SetMaxProgress(5);
    }

    public void ToastBread()  //Toasting minigame
    {
        GameManagerScript.instance.orders.progressBar.ResetProgress();

        if (breadType == "MULTIGRAIN" || breadType == "HONEYOAT")
        {
            GameManagerScript.instance.orders.progressBar.SetMaxProgress(2);
        }

        if (breadType == "WHOLEWHEAT")
        {
            GameManagerScript.instance.orders.progressBar.SetMaxProgress(3);
        }
    }
}
