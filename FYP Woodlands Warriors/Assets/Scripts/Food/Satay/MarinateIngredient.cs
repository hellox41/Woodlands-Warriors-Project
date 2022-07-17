using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarinateIngredient : MonoBehaviour
{
    SatayPrep satayPrep;

    public GameObject marinateLevel;
    float marinateLevelIncrement = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        satayPrep = GameManagerScript.instance.orders.satayPrep;   
    }

    public void AddSatayPrepIngredient(string ingredientType)
    {
        if (GameManagerScript.instance.radialMenu.prepType == "Adding Ingredients")
        {
            if (ingredientType == "shallots")
            {
                satayPrep.areShallotsAdded = true;
            }

            if (ingredientType == "tumericPowder")
            {
                satayPrep.isTumericPowderAdded = true;
            }

            if (ingredientType == "mincedGarlic")
            {
                satayPrep.isMincedGarlicAdded = true;
            }

            if (ingredientType == "chilliPowder")
            {
                satayPrep.isChilliPowderAdded = true;
            }

            if (satayPrep.areShallotsAdded && satayPrep.isTumericPowderAdded && satayPrep.isMincedGarlicAdded && satayPrep.isChilliPowderAdded)
            {
                satayPrep.areAllAdded = true;
                Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
            }

            if (!marinateLevel.activeInHierarchy)
            {
                marinateLevel.SetActive(true);
            }

            marinateLevel.transform.Translate(Vector3.up * marinateLevelIncrement);
            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
            satayPrep.savedAddingProgress++;
        }
    }
}
