using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RadialMenu : MonoBehaviour
{
    [SerializeField] Stove stove;
    public string buttonType;

    public Food food;

    public PlayerControl playerControl;
    public MeowtiToolInteraction meowtiToolInteraction;

    Orders orders;

    [Space]


    [Header("Radial Menu Buttons")]
    public GameObject prepareButton;
    public GameObject pickupButton;
    public GameObject placeButton;
    public GameObject inventoryGO;
    public GameObject prepUIGO;

    public TMP_Text prepTypeText;

    Camera mainCamera;
    CamTransition camTransition;

    private string prepFood;
    private string prepType;

    // Start is called before the first frame update
    void Start()
    {
        orders = GameObject.Find("StageHandler").GetComponent<Orders>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
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

    public void CheckPlaceButton(bool isPlace)
    {
        if (isPlace)
        {
            placeButton.SetActive(true);
        }

        else
        {
            placeButton.SetActive(false);
        }
    }

    public void CheckPrepareButton(bool isPreparable)
    {
        if (isPreparable)
        {
            prepareButton.SetActive(true);
        }

        else
        {
            prepareButton.SetActive(false);
        }
    }

    public void RadialMenuButtonClicked(string buttonType)
    {
        if (buttonType == "PREPARE")
        {
            PrepSetup();
        }

        if (buttonType == "PICKUP")
        {
            Pickup();
        }

        if (buttonType == "PLACE")
        {
            Place();
        }

        playerControl.HideRadialMenu();
        playerControl.interactable = null;
        GameManagerScript.instance.container = null;
        playerControl.outline.enabled = false;
        playerControl.outline = null;
    }

    void PrepSetup()
    {
        string objectName = GameManagerScript.instance.interactedItem.GetComponent<Interactable>().objectName;
        camTransition = mainCamera.GetComponent<CamTransition>();

        if (objectName == "bread")  //Cutting bread
        {
            prepFood = GameManagerScript.instance.interactedFood.GetComponent<Bread>().breadType + " Bread";
            if (GameManagerScript.instance.accessedApparatus == "KNIFE")
            {
                if (orders.currentStage == 1)
                {
                    prepType = "Slicing Edges";
                    camTransition.MoveCamera(GameManagerScript.instance.interactedFood.GetComponent<Food>().camTransitionTransform1);
                    orders.kayaToastPrep.CutBread();                   
                }
            }
        }

        if (objectName == "stove")
        {
            Stove stove = GameManagerScript.instance.interactedItem.GetComponent<Stove>();

            if (stove.isLaden && stove.ladenItem.name == "Skillet" && stove.ladenItem.GetComponent<Container>().itemContained.GetComponent<Food>().foodType == "BREAD")
            {
                prepType = "Toasting Bread";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Stove>().camTransitionTransform1);
                Debug.Log("Toasting da bread");
            }
        }

        if (GameManagerScript.instance.interactedFood.foodType != null && GameManagerScript.instance.accessedApparatus != null)
        {
            prepTypeText.text = prepFood + ", " + prepType;
            prepUIGO.SetActive(true);
        }
    }

    public void Pickup()  //Add an item to the player's inventory and set the item's transform to holding pos and rot
    {
        GameManagerScript.instance.playerInventory.AddItem(GameManagerScript.instance.interactedItem);

        GameManagerScript.instance.interactedItem.transform.parent = inventoryGO.transform;
        GameManagerScript.instance.interactedItem.transform.localPosition = GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingPos;
        GameManagerScript.instance.interactedItem.transform.localEulerAngles = GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingRot;

        if (GameManagerScript.instance.interactedItem.GetComponent<Rigidbody>() != null)
        {
            GameManagerScript.instance.interactedItem.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (GameManagerScript.instance.interactedItem.GetComponent<Outline>() != null && GameManagerScript.instance.interactedItem.GetComponent<Outline>().enabled)
        {
            GameManagerScript.instance.interactedItem.GetComponent<Outline>().enabled = false;
        }

        GameManagerScript.instance.interactedItem.gameObject.SetActive(false);
        stove.OnTriggerExit(GameManagerScript.instance.interactedItem.GetComponent<Collider>());
    }

    public void Place()
    {
        GameManagerScript.instance.container.Contain(GameManagerScript.instance.playerInventory.currentItemHeld);
        GameManagerScript.instance.playerInventory.RemoveItem(GameManagerScript.instance.playerInventory.currentItemHeld);
    }
}
