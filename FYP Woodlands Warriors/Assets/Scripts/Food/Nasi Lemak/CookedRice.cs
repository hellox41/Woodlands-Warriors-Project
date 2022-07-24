using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookedRice : MonoBehaviour
{
    public GameObject potPandan;
    public GameObject asideRice;
    public void ScoopRice()
    {
        GameManagerScript.instance.orders.nasiLemakPrep.isRiceScooped = true;
        Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
        GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
        gameObject.SetActive(false);
        potPandan.transform.Translate(Vector3.down * 0.03f);
        asideRice.SetActive(true);
        GameManagerScript.instance.orders.CheckIfCooked();
    }
}
