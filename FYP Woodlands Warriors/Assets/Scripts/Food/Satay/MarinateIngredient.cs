using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarinateIngredient : MonoBehaviour
{
    SatayPrep satayPrep;
    NasiLemakPrep nasiLemakPrep;

    public GameObject marinateLevel;
    public GameObject marinateChickenObj;
    float marinateLevelIncrement = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        satayPrep = GameManagerScript.instance.orders.satayPrep;
        nasiLemakPrep = GameManagerScript.instance.orders.nasiLemakPrep;
    }

    public void AddSatayPrepIngredient(string ingredientType)
    {
        if (GameManagerScript.instance.radialMenu.prepType == "Adding Ingredients")
        {
            if (ingredientType == "shallots")
            {
                satayPrep.areShallotsAdded = true;
                Destroy(gameObject);
            }

            if (ingredientType == "tumericPowder")
            {
                satayPrep.isTumericPowderAdded = true;
                Destroy(gameObject);
            }

            if (ingredientType == "mincedGarlic")
            {
                satayPrep.isMincedGarlicAdded = true;
                Destroy(gameObject);
            }

            if (ingredientType == "chilliPowder")
            {
                satayPrep.isChilliPowderAdded = true;
                Destroy(gameObject);
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

    public void AddNasiLemakPrepIngredient(string ingredientType)
    {
        if (GameManagerScript.instance.radialMenu.prepType == "Adding Ingredients")
        {

            if (ingredientType == "tumericPowder")
            {
                nasiLemakPrep.isTumericPowderAdded = true;
                Destroy(gameObject);
            }

            if (ingredientType == "mincedGarlic")
            {
                nasiLemakPrep.isMincedGarlicAdded = true;
                Destroy(gameObject);
            }

            if (ingredientType == "chicken")
            {
                nasiLemakPrep.isChickenAdded = true;
                marinateChickenObj.GetComponent<MeshRenderer>().enabled = true;
                Destroy(gameObject);
            }

            if (ingredientType == "saltShaker")
            {
                nasiLemakPrep.isSaltAdded = true;
                Destroy(gameObject);
            }

            if (nasiLemakPrep.isTumericPowderAdded && nasiLemakPrep.isMincedGarlicAdded && nasiLemakPrep.isChickenAdded && nasiLemakPrep.isSaltAdded)
            {
                nasiLemakPrep.areAllAdded = true;
                Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
            }

            if (!marinateLevel.activeInHierarchy)
            {
                marinateLevel.SetActive(true);
            }

            if (ingredientType != "chicken")
            {
                marinateLevel.transform.Translate(Vector3.up * marinateLevelIncrement);
            }

            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
            nasiLemakPrep.savedAddingProgress++;
        }
    }
}
