using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SambalIngredient : MonoBehaviour
{
    Interactable interactable;
    public GameObject sambalObj;

    private void Start()
    {
        interactable = GetComponent<Interactable>();
    }

    public void AddSambalIngredient()
    {
        if (interactable.objectName == "chilliPadi")
        {
            GameManagerScript.instance.orders.nasiLemakPrep.isChilliPadiAdded = true;
        }

        else if (interactable.objectName == "shallots")
        {
            GameManagerScript.instance.orders.nasiLemakPrep.areShallotsAdded = true;
        }

        else if (interactable.objectName == "mincedGarlic")
        {
            GameManagerScript.instance.orders.nasiLemakPrep.isSambalGarlicAdded = true;
        }

        else if (interactable.objectName == "shrimpPaste")
        {
            GameManagerScript.instance.orders.nasiLemakPrep.isShrimpPasteAdded = true;
        }

        else if (interactable.objectName == "waterCup")
        {
            GameManagerScript.instance.orders.nasiLemakPrep.isWaterAdded = true;
        }

        if (GameManagerScript.instance.orders.nasiLemakPrep.isChilliPadiAdded && GameManagerScript.instance.orders.nasiLemakPrep.areShallotsAdded &&
            GameManagerScript.instance.orders.nasiLemakPrep.isSambalGarlicAdded && GameManagerScript.instance.orders.nasiLemakPrep.isShrimpPasteAdded &&
            GameManagerScript.instance.orders.nasiLemakPrep.isWaterAdded)
        {
            GameManagerScript.instance.orders.nasiLemakPrep.areAllSambalAdded = true;
            Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
        }

        if (!sambalObj.activeInHierarchy)
        {
            sambalObj.SetActive(true);
        }
        sambalObj.transform.Translate(Vector3.up * 0.01f);

        GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);

        Destroy(gameObject);
    }
}
