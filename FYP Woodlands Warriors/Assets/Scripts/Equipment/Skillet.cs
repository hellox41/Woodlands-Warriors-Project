using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillet : MonoBehaviour
{
    Container container;

    Stove stove;

    public GameObject friedChickenAsideObj;
    public GameObject sambalCookedAsideObj;

    private void Start()
    {
        container = GetComponent<Container>();

        stove = GetComponent<Interactable>().holdingContainer.GetComponentInParent<Stove>();
    }

    public void CheckFlip()
    {
        if (GameManagerScript.instance.accessedApparatus != "SPATULA")
        {
            Debug.Log("You need the Spatula to flip the bread!");
        }

        else if ((GameManagerScript.instance.orders.currentOrder == "KAYATOAST" && GameManagerScript.instance.orders.kayaToastPrep.isFlippedOnThisColor) ||
            (GameManagerScript.instance.orders.currentOrder == "NASILEMAK" && GameManagerScript.instance.orders.nasiLemakPrep.isFlippedOnThisColor))
        {
            Debug.Log("You've already flipped on this colour!");
        }

        else
        {
            FlipBread();
        }
    }
    void FlipBread()
    {
        if (GameManagerScript.instance.orders.currentOrder == "KAYATOAST")
        {
            if (!GameManagerScript.instance.orders.kayaToastPrep.isFlippedOnThisColor)
            {
                GetComponent<Container>().itemContained.GetComponent<Bread>().burningTime += 8f;

                if (GameManagerScript.instance.orders.kayaToastPrep.breadType == "MULTIGRAIN")
                {
                    if (GameManagerScript.instance.playerControl.stove.stoveLightMat.color == GameManagerScript.instance.playerControl.stove.purpleColor)
                    {
                        GameManagerScript.instance.orders.kayaToastPrep.timesFlippedCorrectly++;
                        GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
                        GameManagerScript.instance.orders.kayaToastPrep.savedToastBreadProgress++;

                    }

                    else
                    {
                        GameManagerScript.instance.orders.dishQualityBar.AddProgress(-15f);
                        GameManagerScript.instance.orders.dishQualityBar.UpdateProgress();
                    }
                }

                if (GameManagerScript.instance.orders.kayaToastPrep.breadType == "WHOLEWHEAT")
                {
                    if (GameManagerScript.instance.playerControl.stove.stoveLightMat.color == GameManagerScript.instance.playerControl.stove.pinkColor)
                    {
                        GameManagerScript.instance.orders.kayaToastPrep.timesFlippedCorrectly++;
                        GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
                        GameManagerScript.instance.orders.kayaToastPrep.savedToastBreadProgress++;
                    }

                    else
                    {
                        GameManagerScript.instance.orders.dishQualityBar.AddProgress(-15f);
                        GameManagerScript.instance.orders.dishQualityBar.UpdateProgress();
                    }
                }

                if (GameManagerScript.instance.orders.kayaToastPrep.breadType == "HONEYOAT")
                {
                    if (GameManagerScript.instance.playerControl.stove.stoveLightMat.color == Color.red)
                    {
                        GameManagerScript.instance.orders.kayaToastPrep.timesFlippedCorrectly++;
                        GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
                        GameManagerScript.instance.orders.kayaToastPrep.savedToastBreadProgress++;
                    }

                    else
                    {
                        GameManagerScript.instance.orders.dishQualityBar.AddProgress(-15f);
                        GameManagerScript.instance.orders.dishQualityBar.UpdateProgress();
                    }
                }

                if ((GameManagerScript.instance.orders.kayaToastPrep.breadType == "MULTIGRAIN" || GameManagerScript.instance.orders.kayaToastPrep.breadType == "HONEYOAT") &&
                    GameManagerScript.instance.orders.kayaToastPrep.timesFlippedCorrectly == 2)
                {
                    GameManagerScript.instance.orders.kayaToastPrep.isBreadToasted = true;
                    stove.isPoweredOn = false;
                    stove.stoveIndicatorLight.enabled = false;
                    stove.stoveLightMat.DisableKeyword("_EMISSION");
                    stove.stoveLightMat.color = stove.originalColor;
                }

                if (GameManagerScript.instance.orders.kayaToastPrep.breadType == "WHOLEWHEAT" && GameManagerScript.instance.orders.kayaToastPrep.timesFlippedCorrectly == 3)
                {
                    GameManagerScript.instance.orders.kayaToastPrep.isBreadToasted = true;
                    stove.isPoweredOn = false;
                    stove.stoveIndicatorLight.enabled = false;
                    stove.stoveLightMat.DisableKeyword("_EMISSION");
                    stove.stoveLightMat.color = stove.originalColor;
                }

                GameManagerScript.instance.orders.kayaToastPrep.isFlippedOnThisColor = true;
            }
        }

        else if (GameManagerScript.instance.orders.currentOrder == "NASILEMAK")
        {
            if (!GameManagerScript.instance.orders.nasiLemakPrep.isFlippedOnThisColor)
            {
                if (container.itemContained.GetComponent<Food>().foodType == "CHICKEN" && (stove.stoveLightMat.color == stove.orangeColor))
                {
                    GameManagerScript.instance.orders.nasiLemakPrep.timesChickenFlippedCorrectly++;
                    GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
                    GameManagerScript.instance.orders.nasiLemakPrep.savedFryingChickenProgress++;
                }

                else if (container.itemContained.GetComponent<Food>().foodType == "SAMBAL" && (stove.stoveLightMat.color == stove.yellowColor))
                {
                    GameManagerScript.instance.orders.nasiLemakPrep.timesChickenFlippedCorrectly++;
                    GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
                    GameManagerScript.instance.orders.nasiLemakPrep.savedFryingSambalProgress++;
                }

                else
                {
                    GameManagerScript.instance.orders.dishQualityBar.AddProgress(-15f);
                }

                if (GameManagerScript.instance.orders.prepProgressBar.slider.value >= GameManagerScript.instance.orders.prepProgressBar.slider.maxValue)
                {
                    if (container.itemContained.GetComponent<Food>().foodType == "CHICKEN")
                    {
                        GameManagerScript.instance.orders.nasiLemakPrep.isChickenFried = true;
                        container.itemContained.SetActive(false);
                        friedChickenAsideObj.SetActive(true);
                    }

                    else if (container.itemContained.GetComponent<Food>().foodType == "SAMBAL")
                    {
                        GameManagerScript.instance.orders.nasiLemakPrep.isSambalFried = true;
                        container.itemContained.SetActive(false);
                        sambalCookedAsideObj.SetActive(true);
                    }

                    stove.isPoweredOn = false;
                    stove.stoveIndicatorLight.enabled = false;
                    stove.stoveLightMat.DisableKeyword("_EMISSION");
                    stove.stoveLightMat.color = stove.originalColor;

                    GameManagerScript.instance.orders.CheckIfCooked();
                    Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
                }

                GameManagerScript.instance.orders.nasiLemakPrep.isFlippedOnThisColor = true;
            }
        }
    }
}
