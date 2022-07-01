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

    Orders orders;

    [Space]


    [Header("Radial Menu Buttons")]
    public GameObject prepareButton;
    public GameObject pickupButton;
    public GameObject placeButton;
    public GameObject transferButton;
    public GameObject inventoryGO;
    public GameObject prepUIGO;

    public TMP_Text prepTypeText;
    public GameObject knifeStatusGO;

    Camera mainCamera;
    CamTransition camTransition;

    private string prepFood;
    private string prepType;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
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

    public void CheckTransferButton(bool isTransferable)
    {
        if (isTransferable)
        {
            transferButton.SetActive(true);
        }

        else
        {
            transferButton.SetActive(false);
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

        if (buttonType == "TRANSFER")
        {
            Transfer(GameManagerScript.instance.interactedItem.GetComponent<Container>());
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

        if (objectName == "bread")
        {
            prepFood = GameManagerScript.instance.interactedFood.GetComponent<Bread>().breadType + " Bread";

            
            if (GameManagerScript.instance.accessedApparatus == "KNIFE")
            {
                //Spreading butter n kaya
                if (orders.kayaToastPrep.isBreadCut && orders.kayaToastPrep.isBreadToasted)
                {
                    prepType = "Spreading Condiments";
                    GameManagerScript.instance.orders.progressBar.ResetProgress();
                    GameManagerScript.instance.orders.progressBar.SetMaxProgress(2);
                    camTransition.MoveCamera(GameManagerScript.instance.interactedFood.GetComponent<Food>().camTransitionTransform2);
                    ShowPrepUI();
                }

                //Cutting bread
                else
                {
                    prepType = "Slicing Edges";
                    camTransition.MoveCamera(GameManagerScript.instance.interactedFood.GetComponent<Food>().camTransitionTransform1);
                    orders.kayaToastPrep.CutBread();
                    ShowPrepUI();
                    knifeStatusGO.SetActive(true);
                }
            }
        }

        if (objectName == "stove")  //Toasting bread
        {
            Stove stove = GameManagerScript.instance.interactedItem.transform.parent.GetComponent<Stove>();

            if (stove.isLaden && stove.ladenItem.GetComponent<Interactable>().objectName == "skillet" && stove.ladenItem.GetComponent<Container>().itemContained.GetComponent<Food>().foodType == "BREAD")
            {
                prepFood = stove.ladenItem.GetComponent<Container>().itemContained.GetComponent<Bread>().breadType + " Bread";
                prepType = "Toasting Bread";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponentInParent<Stove>().camTransitionTransform1);
                orders.kayaToastPrep.ToastBread();
                ShowPrepUI();
            }
        }
    }

    public void ShowPrepUI()
    {
        prepTypeText.text = prepFood + ", " + prepType;
        prepUIGO.SetActive(true);
    }

    public void Pickup()  //Add an item to the player's inventory and set the item's transform to holding pos and rot
    {
        if ((GameManagerScript.instance.interactedItem.GetComponent<Food>() != null && GameManagerScript.instance.interactedItem.GetComponent<Food>().temperature < 47) ||
            GameManagerScript.instance.interactedItem.GetComponent<Food>() == null)
        {
            GameManagerScript.instance.playerInventory.AddItem(GameManagerScript.instance.interactedItem);

            GameManagerScript.instance.interactedItem.transform.parent = inventoryGO.transform;
            GameManagerScript.instance.interactedItem.transform.localPosition = GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingPos;
            GameManagerScript.instance.interactedItem.transform.localEulerAngles = GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingRot;

            //Disable picked up object's rigidbody
            if (GameManagerScript.instance.interactedItem.GetComponent<Rigidbody>() != null)
            {
                GameManagerScript.instance.interactedItem.GetComponent<Rigidbody>().isKinematic = true;
            }

            //Disable picked up object's outline
            if (GameManagerScript.instance.interactedItem.GetComponent<Outline>() != null && GameManagerScript.instance.interactedItem.GetComponent<Outline>().enabled)
            {
                GameManagerScript.instance.interactedItem.GetComponent<Outline>().enabled = false;
            }

            //If picking up object from container, update itemContained and isContainingItem variables of container
            if (GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingContainer != null)
            {
                GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingContainer.isContainingItem = false;
                GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingContainer.itemContained = null;
                GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingContainer = null;
            }

            GameManagerScript.instance.interactedItem.gameObject.SetActive(false);

            //Remove laden item from stove
            if (GameManagerScript.instance.interactedItem == stove.ladenItem)
            {
                stove.isLaden = false;
                stove.ladenItem = null;
                stove.stoveContainer.isContainingItem = false;
            }
        }

        else if (GameManagerScript.instance.interactedItem.GetComponent<Food>() != null && 
            GameManagerScript.instance.interactedItem.GetComponent<Food>().temperature >= 47)
        {
            Debug.Log("Food is too hot to hold with hands! Consider transferring them from their container instead.");
        }
    }

    //Placing an item into a container
    public void Place()
    {
        GameObject tempObj = GameManagerScript.instance.playerInventory.currentItemHeld;
        GameManagerScript.instance.container.Contain(GameManagerScript.instance.playerInventory.currentItemHeld);
        GameManagerScript.instance.playerInventory.RemoveItem(GameManagerScript.instance.playerInventory.currentItemHeld);
        tempObj.SetActive(true);

        tempObj.GetComponent<Interactable>().holdingContainer = GameManagerScript.instance.container;

        GameManagerScript.instance.isPlaceable = false;

        if (GameManagerScript.instance.interactedItem == stove.ladenItem)
        {
            stove.UpdateLadenFood(stove.ladenItem);
        }
    }

    //Transfering the laden item of the currently held container into another empty container
    public void Transfer(Container otherContainer)
    {
        Container heldContainer = GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Container>();

        heldContainer.itemContained.transform.parent = null;
        heldContainer.itemContained.transform.position = otherContainer.placePoint.position;
        heldContainer.itemContained.transform.rotation = otherContainer.placePoint.rotation;
        heldContainer.itemContained.transform.parent = otherContainer.transform;

        heldContainer.isContainingItem = false;
        otherContainer.itemContained = heldContainer.itemContained;
        otherContainer.isContainingItem = true;
        heldContainer.itemContained = null;
        GameManagerScript.instance.isTransferable = false;

        GameManagerScript.instance.playerInventory.SetLayerRecursively(otherContainer.itemContained, LayerMask.NameToLayer("Default"));
    }
}
