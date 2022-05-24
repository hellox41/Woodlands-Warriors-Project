using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public int maxItems = 3;
    public int currentItems = 1;

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
        var d = Input.GetAxis("Mouse ScrollWheel");
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

            }
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
            itemsHeld.Add(objToAdd);
            currentItems = itemsHeld.Count;
            Debug.Log("Added the " + objToAdd);
        }
    }
}
