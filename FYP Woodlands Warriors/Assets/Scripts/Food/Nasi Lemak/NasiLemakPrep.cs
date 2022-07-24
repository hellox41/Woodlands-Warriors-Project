using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NasiLemakPrep : MonoBehaviour
{
    public int[] dishTimes = { 200, 285, 390, 500 };

    public string[] riceTypes = { "Brown", "White" };
    public string riceType;

    public int preparedCount;
    [Header("Set Aside")]
    public GameObject cookedRiceObj;
    public GameObject cookedChickenObj;
    public GameObject cookedSambalObj;

    [Header("Rice Prep")]
    public float savedFillingWaterProgress;
    public float savedRiceCookingProgress;
    public bool isRiceInPot = false;
    public bool isPotFilledWithWater = false;
    public bool isPotInCooker = false;
    public bool isRiceCooked = false;
    public bool isRiceScooped = false;

    [Header("Chicken Prep")]
    public float savedAddingProgress;
    public float savedFryingChickenProgress;
    public bool isMincedGarlicAdded = false;
    public bool isTumericPowderAdded = false;
    public bool isChickenAdded = false;
    public bool isSaltAdded = false;

    public bool areAllAdded = false;

    public bool isMarinateMixed = false;
    public GameObject mixingBar;
    public TMP_Text barText;

    public bool isFlippedOnThisColor = false;
    public int timesChickenFlippedCorrectly = 0;
    public bool isChickenFried = false;

    [Header("Sambal Prep")]
    public float savedAddingSambalProgress;
    public float savedFryingSambalProgress;

    public bool isChilliPadiAdded = false;
    public bool areShallotsAdded = false;
    public bool isSambalGarlicAdded = false;
    public bool isShrimpPasteAdded = false;
    public bool isWaterAdded = false;
    public bool isSambalFried = false;

    public bool areAllSambalAdded = false;

    public bool isSambalGrinded = false;
    public bool isSambalAddedToSkillet = false;
    public bool areAnchoviesAddedToSkillet = false;
    public bool isOilAddedToSkillet = false;

    public bool areAllSambalAddedToSkillet = false;
    
    public void StartAddingRice()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetProgress();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(1);
    }
    public void StartFillingPotWithWater()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetProgress();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(1000);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedFillingWaterProgress);
    }

    public void StartCookingRice()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetProgress();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(5);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedRiceCookingProgress);
    }

    public void StartAddingIngredients()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetProgress();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(4);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedAddingProgress);
    }

    public void StartMixingIngredients()
    {
        mixingBar.GetComponent<ProgressBar>().ResetProgress();
        mixingBar.GetComponent<ProgressBar>().ResetValue();
        barText.text = "MIXING";
        mixingBar.SetActive(true);
        GameManagerScript.instance.orders.prepProgressBar.ResetProgress();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(1);
    }    

    public void StartFryingChicken()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetProgress();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(2);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedFryingChickenProgress);
    }

    public void StartAddingSambalIngredients()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetProgress();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(5);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedAddingSambalProgress);
    }

    public void StartGrindingSambalIngredients()
    {
        mixingBar.GetComponent<ProgressBar>().ResetProgress();
        mixingBar.GetComponent<ProgressBar>().ResetValue();
        barText.text = "GRINDING";
        mixingBar.SetActive(true);
        GameManagerScript.instance.orders.prepProgressBar.ResetProgress();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(1);
    }

    public void StartScoopingSambal()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetProgress();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(1);
    }

    public void StartCookingSambal()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetProgress();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(5);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedFryingSambalProgress);
    }
    public void ResetVariables()
    {
        savedAddingProgress = 0;
        savedFillingWaterProgress = 0;
        savedFryingChickenProgress = 0;
        savedFryingSambalProgress = 0;
        savedRiceCookingProgress = 0;

        isRiceInPot = false;
        isPotFilledWithWater = false;
        isPotInCooker = false;
        isRiceCooked = false;
        isRiceScooped = false;

        isMincedGarlicAdded = false;
        isTumericPowderAdded = false;
        isChickenAdded = false;
        isSaltAdded = false;
        areAllAdded = false;
        isMarinateMixed = false;

        isFlippedOnThisColor = false;
        isChickenFried = false;
    }
}
