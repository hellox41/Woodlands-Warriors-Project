using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    public string buttonType;

    public Food food;

    public PlayerControl playerControl;
    public MeowtiToolInteraction meowtiToolInteraction;

    Orders orders;
    public GameObject pickupButton;
    public GameObject inventoryGO;

    // Start is called before the first frame update
    void Start()
    {
        orders = GameObject.Find("StageHandler").GetComponent<Orders>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckPickupButton(bool isPickup)
    {
        if (isPickup)
        {
            pickupButton.SetActive(true);
        }
        else
        {
            pickupButton.SetActive(false);
        }

    }

    public void RadialMenuButtonClicked(string buttonType)
    {
        if (buttonType == "PREPARE")
        {
            PrepSetup(GameManagerScript.instance.accessedApparatus, GameManagerScript.instance.interactedFood.foodType);
        }

        if (buttonType == "PICKUP")
        {
            Pickup();
            playerControl.HideRadialMenu();
            playerControl.interactable = null;
            playerControl.outline.enabled = false;
            playerControl.outline = null;
        }
    }

    void PrepSetup(string apparatusType, string foodType)
    {
        if (foodType == "BREAD")
        {
            if (apparatusType == "KNIFE")
            {
                if (orders.currentStage == 1)
                {
                    Debug.Log("Initiate cutting bread cooking event");
                    orders.kayaToastPrep.CutBread();
                }
            }
        }
    }

    public void Pickup()
    {
        GameManagerScript.instance.playerInventory.AddItem(GameManagerScript.instance.interactedFood.gameObject);
        GameManagerScript.instance.interactedFood.gameObject.transform.parent = inventoryGO.transform;
        GameManagerScript.instance.interactedFood.transform.localPosition = new Vector3(0.264f, -0.1479999f, 0.26f);
        if (GameManagerScript.instance.interactedFood.GetComponent<Rigidbody>() != null)
        {
            GameManagerScript.instance.interactedFood.GetComponent<Rigidbody>().isKinematic = true;
        }
        GameManagerScript.instance.interactedFood.gameObject.SetActive(false);
    }
}
