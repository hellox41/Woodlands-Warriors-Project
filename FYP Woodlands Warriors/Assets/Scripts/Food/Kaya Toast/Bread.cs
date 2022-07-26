using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : MonoBehaviour
{
    public AudioClip cutSfx;
    public string breadType;

    public GameObject cutcanvas;

    Food food;

    public float burningTime = 25f;

    public Spread spread;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;

        foreach (Transform child in cutcanvas.transform)
        {
            GameManagerScript.instance.orders.kayaToastPrep.cutButtons[i] = child.gameObject;
            i++;
        }

        food = GetComponent<Food>();

        if (GameManagerScript.instance.orders.kayaToastPrep.breadType != "WHOLEWHEAT")
        {
            GetComponent<Interactable>().isPickup = true;
        }
    }

    private void Update()
    {
        if (food.isBeingHeated)
        {
            if (burningTime > 0)
            {
                burningTime -= Time.deltaTime;
            }

            if (burningTime <= 0)
            {
                GameManagerScript.instance.orders.dishQualityBar.AddProgress(-Time.deltaTime * 1.5f);
            }
        }

        if (!food.isBeingHeated && burningTime <= 0)
        {
            burningTime += Time.deltaTime * 0.5f;
        }
    }

    public void UpdateTooltipActionType()
    {
        GameManagerScript.instance.playerControl.raycastActionTooltipText.text = "Cut (Requires Knife)";
    }

    public void OnCutButtonPressed(int buttonIndex)
    {
        GameManagerScript.instance.orders.sfxAudioSource.PlayOneShot(cutSfx);
        GameManagerScript.instance.orders.kayaToastPrep.cutButtons[buttonIndex].SetActive(false);
        GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
        GameManagerScript.instance.orders.kayaToastPrep.savedCutBreadProgress++;

        if (buttonIndex == 4)
        {
            GameManagerScript.instance.orders.kayaToastPrep.isBreadCut = true;
            GameManagerScript.instance.orders.kayaToastPrep.cutCanvas.gameObject.SetActive(false);
            GetComponent<Interactable>().isPickup = true;
            Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
        }

        else
        {
            GameManagerScript.instance.orders.kayaToastPrep.cutButtons[buttonIndex + 1].SetActive(true);
        }
    }

    public void SpreadBread()
    {
        spread.mr.enabled = true;

        if (GameManagerScript.instance.orders.kayaToastPrep.isKayaAppliedToKnife)
        {
            GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadKaya = true;
            spread.gameObject.SetActive(true);

            spread.UpdateSpread("Kaya");
            GameManagerScript.instance.orders.kayaToastPrep.isKayaAppliedToKnife = false;
            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
        }

        if (GameManagerScript.instance.orders.kayaToastPrep.isButterAppliedToKnife)
        {
            GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadButter = true;
            spread.gameObject.SetActive(true);

            spread.UpdateSpread("Butter");
            GameManagerScript.instance.orders.kayaToastPrep.isButterAppliedToKnife = false;
            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
        }

        if ((GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadKaya && GameManagerScript.instance.orders.kayaToastPrep.isButterAppliedToKnife) || 
            (GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadButter && GameManagerScript.instance.orders.kayaToastPrep.isKayaAppliedToKnife))
        {
            spread.UpdateSpread("Kaya and Butter");
            GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadKaya = true;
            GameManagerScript.instance.orders.kayaToastPrep.isBreadSpreadButter = true;
            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
        }

        GameManagerScript.instance.orders.CheckIfCooked();
    }
}
