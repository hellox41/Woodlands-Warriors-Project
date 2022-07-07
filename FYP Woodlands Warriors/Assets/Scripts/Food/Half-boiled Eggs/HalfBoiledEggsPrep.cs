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

    [SerializeField] float eggsCookingTimer = 10f;

    public string[] eggTypes = {"BROWN", "WHITE"};

    public string eggsType;

    public void StartFillingWater()
    {
        GameManagerScript.instance.orders.progressBar.ResetProgress();
        GameManagerScript.instance.orders.progressBar.SetMaxProgress(1000);
    }

    public void StartBoilingEggs()
    {
        GameManagerScript.instance.orders.progressBar.ResetProgress();
        GameManagerScript.instance.orders.progressBar.SetMaxProgress(5);
    }

    private void Update()
    {
        if (eggsInsidePot == 2 && isWaterBoiled && eggsCookingTimer > 0)
        {
            eggsCookingTimer -= Time.deltaTime;

            if (eggsCookingTimer <= 0)
            {
                areEggsBoiled = true;
                GameManagerScript.instance.orders.progressBar.AddProgress(1);
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
    }
}
