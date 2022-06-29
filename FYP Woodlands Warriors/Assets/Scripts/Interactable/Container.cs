﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public Transform placePoint;
    public bool isContainingItem = false;
    public bool isShowingPreview = false;
    public bool canContainContainers = false;

    public GameObject itemContained;

    [Range(0f, 1f)]
    [SerializeField] float previewAlpha = 0.5f;

    public GameObject previewedFood;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPreview()
    {
        if (!isShowingPreview && GameManagerScript.instance.playerInventory.currentItemHeld.name != "Meow-ti Tool")
        {
            previewedFood = Instantiate(GameManagerScript.instance.playerInventory.currentItemHeld, placePoint.position, placePoint.rotation);
            previewedFood.transform.parent = transform;
            GameManagerScript.instance.playerInventory.SetLayerRecursively(previewedFood, LayerMask.NameToLayer("Ignore Raycast"));

            if (previewedFood.GetComponent<Collider>() != null)
            {
                previewedFood.GetComponent<Collider>().enabled = false;
            }
            Color previewColor = previewedFood.GetComponent<MeshRenderer>().material.color;
            previewColor.a = previewAlpha;
            previewedFood.GetComponent<MeshRenderer>().material.color = previewColor;
            isShowingPreview = true;
        }
    }

    public void HidePreview()
    {
        Destroy(previewedFood);
        isShowingPreview = false;
    }

    public void Contain(GameObject itemToContain)
    {
        Destroy(previewedFood);
        itemToContain.transform.position = placePoint.position;
        itemToContain.transform.rotation = placePoint.rotation;

        GameManagerScript.instance.playerInventory.SetLayerRecursively(itemToContain, LayerMask.NameToLayer("Default"));

        itemToContain.transform.parent = transform;
        itemContained = itemToContain;
        isContainingItem = true;
    }
}
