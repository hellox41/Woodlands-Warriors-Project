using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingBowl : MonoBehaviour
{
    MixingBar mixingBar;
    [SerializeField] float progressToAdd;


    public GameObject meatGO;

    public void Mix()
    {
        if (GameManagerScript.instance.radialMenu.prepType == "Mixing Ingredients")
        {
            if (mixingBar == null)
            {
                mixingBar = GameObject.Find("grindingBar").GetComponent<MixingBar>();
            }

            mixingBar.progressBar.AddProgress(progressToAdd);

            if (mixingBar.progressBar.slider.value >= mixingBar.progressBar.slider.maxValue)
            {
                MeshRenderer meatMr = meatGO.GetComponent<MeshRenderer>();

                meatMr.material.color = new Color32(231, 208, 66, 255);
                Interactable interactable = GetComponent<Interactable>();
                interactable.isPickup = true;
            }
        }
    }
}
