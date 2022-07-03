using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : MonoBehaviour
{
    public string breadType;

    public GameObject cutcanvas;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;

        foreach (Transform child in cutcanvas.transform)
        {
            GameManagerScript.instance.orders.kayaToastPrep.cutButtons[i] = child.gameObject;
            i++;
        }
    }

    public void UpdateTooltipActionType()
    {
        GameManagerScript.instance.playerControl.raycastActionTooltipText.text = "Cut (Requires Knife)";
    }

    public void OnCutButtonPressed(int buttonIndex)
    {
        GameManagerScript.instance.orders.kayaToastPrep.cutButtons[buttonIndex].SetActive(false);
        GameManagerScript.instance.orders.progressBar.AddProgress(1);

        if (buttonIndex == 4)
        {
            GameManagerScript.instance.orders.kayaToastPrep.isBreadCut = true;
            GameManagerScript.instance.orders.kayaToastPrep.cutCanvas.gameObject.SetActive(false);
        }

        else
        {
            GameManagerScript.instance.orders.kayaToastPrep.cutButtons[buttonIndex + 1].SetActive(true);
        }
    }

    public void SpreadBread()
    {
        Spread spread = GameObject.Find("Spread").GetComponent<Spread>();

        if (!GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadKaya && !GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadButter &&
            (GameManagerScript.instance.orders.kayaToastPrep.isKayaAppliedToKnife || GameManagerScript.instance.orders.kayaToastPrep.isButterAppliedToKnife))
        {
            spread.mr.enabled = true;
        }

        if (GameManagerScript.instance.orders.kayaToastPrep.isKayaAppliedToKnife)
        {
            GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadKaya = true;
            spread.gameObject.SetActive(true);

            spread.UpdateSpread("Kaya");
            GameManagerScript.instance.orders.kayaToastPrep.isKayaAppliedToKnife = false;
            GameManagerScript.instance.orders.progressBar.AddProgress(1);
        }

        if (GameManagerScript.instance.orders.kayaToastPrep.isButterAppliedToKnife)
        {
            GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadButter = true;
            spread.gameObject.SetActive(true);

            spread.UpdateSpread("Butter");
            GameManagerScript.instance.orders.kayaToastPrep.isButterAppliedToKnife = false;
            GameManagerScript.instance.orders.progressBar.AddProgress(1);
        }

        if ((GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadKaya && GameManagerScript.instance.orders.kayaToastPrep.isButterAppliedToKnife) || 
            (GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadButter && GameManagerScript.instance.orders.kayaToastPrep.isKayaAppliedToKnife))
        {
            spread.UpdateSpread("Kaya and Butter");
            GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadKaya = true;
            GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadButter = true;
            GameManagerScript.instance.orders.progressBar.AddProgress(1);
        }

        GameManagerScript.instance.orders.CheckIfCooked();
    }
}
