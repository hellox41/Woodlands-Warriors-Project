using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SambalPasteIngredient : MonoBehaviour
{
    public GameObject skilletSambal;
    public GameObject skilletOil;

    public Container skilletContainer;

    public void ScoopPaste()
    {
        if (GameManagerScript.instance.accessedApparatus == "SPATULA")
        {
            gameObject.SetActive(false);
            skilletSambal.SetActive(true);
            GameManagerScript.instance.orders.nasiLemakPrep.isSambalAddedToSkillet = true;
            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
            GameManagerScript.instance.orders.nasiLemakPrep.savedFryingSambalProgress++;
            skilletContainer.itemContained = skilletSambal;
            Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
            CheckIfAllAdded();
        }
    }

    public void AddAnchovies()
    {
        if (!GameManagerScript.instance.orders.nasiLemakPrep.areAnchoviesAddedToSkillet && GameManagerScript.instance.orders.nasiLemakPrep.isSambalAddedToSkillet)
        {
            gameObject.SetActive(false);

            MeshRenderer sambalMr = skilletSambal.GetComponent<MeshRenderer>();
            sambalMr.material.color = new Color32(176, 106, 85, 255);

            GameManagerScript.instance.orders.nasiLemakPrep.areAnchoviesAddedToSkillet = true;
            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
            GameManagerScript.instance.orders.nasiLemakPrep.savedFryingSambalProgress++;
            CheckIfAllAdded();
        }
    }

    public void AddOil()
    {
        if (!GameManagerScript.instance.orders.nasiLemakPrep.isOilAddedToSkillet && GameManagerScript.instance.orders.nasiLemakPrep.isSambalAddedToSkillet)
        {
            gameObject.SetActive(false);
            skilletOil.SetActive(true);

            GameManagerScript.instance.orders.nasiLemakPrep.isOilAddedToSkillet = true;
            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
            GameManagerScript.instance.orders.nasiLemakPrep.savedFryingSambalProgress++;
            CheckIfAllAdded();
        }
    }  
    
    void CheckIfAllAdded()
    {
        if (GameManagerScript.instance.orders.nasiLemakPrep.isSambalAddedToSkillet && GameManagerScript.instance.orders.nasiLemakPrep.areAnchoviesAddedToSkillet &&
            GameManagerScript.instance.orders.nasiLemakPrep.isOilAddedToSkillet)
        {
            GameManagerScript.instance.orders.nasiLemakPrep.areAllSambalAddedToSkillet = true;
        }
    }
}
