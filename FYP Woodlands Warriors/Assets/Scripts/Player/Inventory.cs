using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    private int maxItems = 3;
    private int currentItems = 1;

    public List<GameObject> itemsHeld = new List<GameObject>();
    public GameObject currentItemHeld;
    // Start is called before the first frame update
    void Start()
    {
        currentItemHeld = GameObject.Find("Meow-ti Tool");
        itemsHeld.Add(currentItemHeld);
    }

    // Update is called once per frame
    void Update()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");  //Swapping between items in inventory

        if (!GameManagerScript.instance.isInteracting && !GameManagerScript.instance.isZoomed && !GameManagerScript.instance.isPreparing)
        {
            if (currentItems > 1)
            {
                if (d > 0f) //scroll up
                {
                    int index = itemsHeld.IndexOf(currentItemHeld);

                    if (index == itemsHeld.Count - 1)
                    {
                        currentItemHeld.SetActive(false);
                        currentItemHeld = itemsHeld[0];
                        currentItemHeld.SetActive(true);
                    }

                    else if (index != itemsHeld.Count - 1)
                    {
                        currentItemHeld.SetActive(false);
                        currentItemHeld = itemsHeld[index + 1];
                        currentItemHeld.SetActive(true);
                    }

                    CheckPlaceable();
                }

                if (d < 0f) //scroll down
                {
                    int index = itemsHeld.IndexOf(currentItemHeld);

                    if (index == 0)
                    {
                        currentItemHeld.SetActive(false);
                        currentItemHeld = itemsHeld[itemsHeld.Count - 1];
                        currentItemHeld.SetActive(true);
                    }

                    else if (index > 0)
                    {
                        currentItemHeld.SetActive(false);
                        currentItemHeld = itemsHeld[index - 1];
                        currentItemHeld.SetActive(true);
                    }

                    CheckPlaceable();
                }
            }
        }
    }

    void CheckPlaceable()
    {
        if (currentItemHeld.name != "Meow-ti Tool")
        {
            GameManagerScript.instance.isPlaceable = true;

            if (GameManagerScript.instance.container != null)
            {
                GameManagerScript.instance.container.ShowPreview();
            }
        }

        else if (currentItemHeld.name == "Meow-ti Tool")
        {
            GameManagerScript.instance.isPlaceable = false;

            if (GameManagerScript.instance.container != null)
            {
                GameManagerScript.instance.container.HidePreview();
            };
        }
    }

    public void AddItem(GameObject objToAdd)
    {
        if (currentItems == maxItems)
        {
            Debug.Log("You are carrying the max amount of items!");
        }

        if (currentItems < maxItems)
        {
            SetLayerRecursively(objToAdd, LayerMask.NameToLayer("Holding"));

            objToAdd.GetComponent<Interactable>().isInInventory = true;
            objToAdd.SetActive(false);
            itemsHeld.Add(objToAdd);
            currentItems = itemsHeld.Count;
            Debug.Log("Picked up the " + objToAdd);
        }
    }

    public void RemoveItem(GameObject objToRemove)
    {
        //objToRemove's layers are recursively set in Container.cs
        if (objToRemove != GameObject.Find("Meow-ti Tool"))
        {
            itemsHeld.Remove(objToRemove);
            currentItems = itemsHeld.Count;

            SwapHeldItem();
        }
    }

    public void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = newLayer;

        foreach(Transform child in obj.transform)
        {
            if (child == null)
            {
                return;
            }

            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public void SwapHeldItem()
    {
        int index = itemsHeld.IndexOf(currentItemHeld);

        if (index == itemsHeld.Count - 1)
        {
            currentItemHeld.SetActive(false);
            currentItemHeld = itemsHeld[0];
            currentItemHeld.SetActive(true);
        }

        else if (index != itemsHeld.Count - 1)
        {
            currentItemHeld.SetActive(false);
            currentItemHeld = itemsHeld[index + 1];
            currentItemHeld.SetActive(true);
        }
    }
}
