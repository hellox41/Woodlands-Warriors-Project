using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mortar : MonoBehaviour
{
    MixingBar mixingBar;
    [SerializeField] float progressToAdd;

    public string prepType;

    public GameObject meatGO;
    public Transform dropPoint;

    public Skewer skewerScript;

    public void Grind()
    {
        if (GameManagerScript.instance.radialMenu.prepType == "Grinding Ingredients" || GameManagerScript.instance.radialMenu.prepType == "Grinding Sambal Ingredients")
        {
            if (mixingBar == null)
            {
                mixingBar = GameObject.Find("grindingBar").GetComponent<MixingBar>();
            }

            mixingBar.progressBar.AddProgress(progressToAdd);
        }
    }

    public void DropMeat()
    {
        if (GameManagerScript.instance.radialMenu.prepType == "Mixing Meat")
        {
            if (GameManagerScript.instance.orders.satayPrep.meatType == "Beef Cubes")
            {
                meatGO = GameObject.Find("beefCubes");
            }

            meatGO.transform.position = dropPoint.position;
            meatGO.transform.parent = null;

            foreach (Transform child in meatGO.transform)
            {
                Rigidbody childRb = child.GetComponent<Rigidbody>();
                childRb.isKinematic = false;
                childRb.useGravity = true;

                StartCoroutine(WaitASec(meatGO.transform, transform));
            }
        }
    }

    IEnumerator WaitASec(Transform childTrans, Transform parentTrans)
    {
        yield return new WaitForSeconds(1f);
        childTrans.SetParent(parentTrans);
    }

    public void MixMeat()
    {
        if (GameManagerScript.instance.radialMenu.prepType == "Mixing Meat" && GameManagerScript.instance.orders.satayPrep.isMeatInMortar)
        {
            GameManagerScript.instance.orders.satayPrep.isMeatMixed = true;
            GameManagerScript.instance.orders.prepProgressBar.SetProgress(GameManagerScript.instance.orders.prepProgressBar.slider.maxValue);
            GameManagerScript.instance.orders.prepProgressBar.UpdateProgress();

            //Color the meat red
            foreach (Transform child in meatGO.transform)
            {
                MeshRenderer mr = child.GetComponent<MeshRenderer>();
                mr.material.color = new Color32(236, 144, 70, 255);
            }

            Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
        }
    }

    public void ThreadCube()
    {
        Skewers skewersScript = GameObject.Find("Skewers").GetComponent<Skewers>();
        if (!skewersScript.isSkewerMoving)
        {
            if (GameManagerScript.instance.radialMenu.prepType == "Threading Cubes" && GameManagerScript.instance.orders.satayPrep.isMeatMixed)
            {
                if (GameManagerScript.instance.orders.satayPrep.meatType == "Beef")
                {
                    skewerScript.ThreadMeatOntoSkewer("Beef");
                    GameManagerScript.instance.orders.satayPrep.cubesOnSkewer++;
                }

                if (GameManagerScript.instance.orders.satayPrep.meatType == "Chicken")
                {
                    skewerScript.ThreadMeatOntoSkewer("Chicken");
                    GameManagerScript.instance.orders.satayPrep.cubesOnSkewer++;
                }

                if (GameManagerScript.instance.orders.satayPrep.meatType == "Mutton")
                {
                    skewerScript.ThreadMeatOntoSkewer("Mutton");
                    GameManagerScript.instance.orders.satayPrep.cubesOnSkewer++;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "meatCube")
        {
            if (!GameManagerScript.instance.orders.satayPrep.isMeatInMortar)
            {
                GameManagerScript.instance.orders.satayPrep.isMeatInMortar = true;
            }
            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
        }
    }
}
