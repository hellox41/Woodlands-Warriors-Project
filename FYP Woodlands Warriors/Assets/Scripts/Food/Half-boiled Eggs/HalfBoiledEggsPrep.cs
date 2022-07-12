using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfBoiledEggsPrep : MonoBehaviour
{
    public bool isPotFilledWithWater = false;
    public bool isHeatingWater = false;
    public bool isWaterBoiled = false;
    public bool areEggsBoiling = false;
    public bool areEggsBoiled = false;

    public int eggsInsidePot = 0;
    public int eggsStrained = 0;

    public float savedFillingWaterProgress;
    public float savedBoilingEggsProgress;

    public int preparedCount = 0;

    //For grading use, [0] is 3 star, [1] is 2 star, [2] is 1 star, [3] is the maximum time limit for the dish before failing the order
    public int[] dishTimes = { 60, 70, 85, 100 };

    [SerializeField] float eggsCookingTimer = 10f;
    float overcookedTimer;

    public string[] eggTypes = {"BROWN", "WHITE"};

    public string eggsType;

    public void StartFillingWater()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetValue();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(1000);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedFillingWaterProgress);
    }

    public void StartBoilingEggs()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetValue();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(5);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedBoilingEggsProgress);
    }

    private void Update()
    {
        if (eggsInsidePot == 2 && isWaterBoiled && eggsCookingTimer > 0)
        {
            eggsCookingTimer -= Time.deltaTime;

            if (eggsCookingTimer <= 0)
            {
                areEggsBoiled = true;
                GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
                savedBoilingEggsProgress++;
            }
        }

        if (areEggsBoiled && GameManagerScript.instance.playerControl.stove.isPoweredOn)
        {
            overcookedTimer += Time.deltaTime;

            if (overcookedTimer > 10)
            {
                GameManagerScript.instance.orders.dishQualityBar.AddProgress(-Time.deltaTime * 1.5f);
                GameManagerScript.instance.orders.dishQualityBar.UpdateProgress();
            }
        }
    }

    public void ResetVariables()
    {
        isPotFilledWithWater = false;
        isHeatingWater = false;
        isWaterBoiled = false;
        areEggsBoiling = false;
        areEggsBoiled = false;

        eggsInsidePot = 0;
        eggsStrained = 0;

        eggsCookingTimer = 10f;

        savedBoilingEggsProgress = 0;
        savedFillingWaterProgress = 0;
    }
}
