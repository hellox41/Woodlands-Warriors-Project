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
    public GameObject powerButton;
    public Stove stove;
    [SerializeField] int timesFlippedCorrectly = 0;
    public bool isFlippedOnThisColor = false;

    [Space]

    [Header("Spreading Prep Values")]
    public bool isKayaAppliedToKnife = false;
    public bool isButterAppliedToKnife = false;

    public TMP_Text condimentText;

    private void Update()
    {
        
    }

    public void CutBread()  //Cutting minigame
    {
        cutCanvas.gameObject.SetActive(true);
        GameManagerScript.instance.orders.progressBar.ResetProgress();
        GameManagerScript.instance.orders.progressBar.SetMaxProgress(5);
    }

    public void OnCutButtonPressed(int buttonIndex)
    {
        cutButtons[buttonIndex].SetActive(false);
        GameManagerScript.instance.orders.progressBar.AddProgress(1);

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

    public void FlipBread()
    {
        if (GameManagerScript.instance.accessedApparatus != "SPATULA")
        {
            Debug.Log("You need the Spatula to flip the bread!");
        }

        else if (GameManagerScript.instance.accessedApparatus == "SPATULA" && isFlippedOnThisColor)
        {
            Debug.Log("You've already flipped the bread on this colour!");
        }

        else if (GameManagerScript.instance.accessedApparatus == "SPATULA" && !isFlippedOnThisColor)
        {
            if (breadType == "MULTIGRAIN" && stove.stoveLightMat.color == stove.purpleColor)
            {
                timesFlippedCorrectly++;
                GameManagerScript.instance.orders.progressBar.AddProgress(1);
            }

            if (breadType == "WHOLEWHEAT" && stove.stoveLightMat.color == stove.pinkColor)
            {
                timesFlippedCorrectly++;
                GameManagerScript.instance.orders.progressBar.AddProgress(1);
            }

            if (breadType == "HONEYOAT" && stove.stoveLightMat.color == Color.red)
            {
                timesFlippedCorrectly++;
                GameManagerScript.instance.orders.progressBar.AddProgress(1);
            }

            if ((breadType == "MULTIGRAIN" || breadType == "HONEYOAT") && timesFlippedCorrectly == 2)
            {
                isBreadToasted = true;
            }

            if (breadType == "WHOLEWHEAT" && timesFlippedCorrectly == 3)
            {
                isBreadToasted = true;
            }

            isFlippedOnThisColor = true;
        }
    }

    public void SpreadBread()
    {
        Spread spread = GameObject.Find("Spread").GetComponent<Spread>();

        if (!isBreadSpreadKaya && !isBreadSpreadButter)
        {
            spread.mr.enabled = true;
        }

        if (isKayaAppliedToKnife)
        {
            isBreadSpreadKaya = true;
            spread.gameObject.SetActive(true);

            spread.UpdateSpread("Kaya");
            isKayaAppliedToKnife = false;
            GameManagerScript.instance.orders.progressBar.AddProgress(1);
        }

        if (isButterAppliedToKnife)
        {
            isBreadSpreadButter = true;
            spread.gameObject.SetActive(true);

            spread.UpdateSpread("Butter");
            isButterAppliedToKnife = false;
            GameManagerScript.instance.orders.progressBar.AddProgress(1);
        }

        if ((isBreadSpreadKaya && isButterAppliedToKnife) || (isBreadSpreadButter && isKayaAppliedToKnife))
        {
            spread.UpdateSpread("Kaya and Butter");
            isBreadSpreadKaya = true;
            isBreadSpreadButter = true;
            GameManagerScript.instance.orders.progressBar.AddProgress(1);
        }

        GameManagerScript.instance.orders.CheckIfCooked();
    }

    public void ApplyKaya()
    {
        isKayaAppliedToKnife = true;
        condimentText.text = "Knife Condiment: Kaya";
    }

    public void ApplyButter()
    {
        isButterAppliedToKnife = true;
        condimentText.text = "Knife Condiment: Butter";
    }
}
