using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RicePacket : MonoBehaviour
{
    public GameObject riceObj;

    public void AddRice()
    {
        if (!GameManagerScript.instance.orders.nasiLemakPrep.isRiceInPot)
        {
            GameManagerScript.instance.orders.nasiLemakPrep.isRiceInPot = true;
            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
            riceObj.GetComponentInParent<Interactable>().isPickup = true;
            riceObj.SetActive(true);
            Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
            Destroy(gameObject);
        }
    }
}
