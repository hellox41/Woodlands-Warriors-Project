using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RadialMenu : MonoBehaviour
{
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
    public GameObject prepStatusGO;

    Camera mainCamera;
    CamTransition camTransition;

    private string prepFood;
    public string prepType;

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
        if (prepStatusGO.activeInHierarchy)
        {
            prepStatusGO.SetActive(false);
        }

        if (objectName == "bread")
        {
            prepFood = GameManagerScript.instance.interactedFood.GetComponent<Bread>().breadType + " Bread";
           
            if (GameManagerScript.instance.accessedApparatus == "KNIFE")
            {
                //KAYATOAST
                //Spreading butter n kaya
                if (orders.kayaToastPrep.isBreadCut && orders.kayaToastPrep.isBreadToasted)
                {
                    prepType = "Spreading Condiments";
                    GameManagerScript.instance.interactedFood.GetComponent<Interactable>().enabled = true;
                    GameManagerScript.instance.orders.prepProgressBar.ResetValue();
                    GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(2);
                    camTransition.MoveCamera(GameManagerScript.instance.interactedFood.GetComponent<Food>().camTransitionTransform2);
                    ShowPrepUI();
                    prepStatusGO.SetActive(true);
                    CheckItemsInteractibility();
                }   

                //KAYATOAST
                //Cutting bread
                else if (!orders.kayaToastPrep.isBreadCut)
                {
                    prepType = "Slicing Edges";
                    GameManagerScript.instance.interactedFood.GetComponent<Interactable>().enabled = false;
                    orders.kayaToastPrep.cutCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
                    orders.kayaToastPrep.cutButtons[0].SetActive(true);

                    camTransition.MoveCamera(GameManagerScript.instance.interactedFood.GetComponent<Food>().camTransitionTransform1);
                    orders.kayaToastPrep.CutBread();
                    ShowPrepUI();
                    CheckItemsInteractibility();
                }
            }
        }

        if (objectName == "stove")  
        {
            Stove stove = GameManagerScript.instance.interactedItem.transform.parent.GetComponent<Stove>();

            //KAYATOAST
            //Toasting bread
            if (stove.isLaden && stove.ladenItem.GetComponent<Interactable>().objectName == "skillet" && stove.ladenItem.GetComponent<Container>().itemContained.GetComponent<Food>().foodType == "BREAD")
            {
                prepFood = stove.ladenItem.GetComponent<Container>().itemContained.GetComponent<Bread>().breadType + " Bread";
                prepType = "Toasting Bread";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponentInParent<Stove>().camTransitionTransform1);
                orders.kayaToastPrep.ToastBread();
                ShowPrepUI();
                CheckItemsInteractibility();
            }

            //HALF-BOILEDEGGS
            //Boiling eggs
            if (stove.isLaden && stove.ladenItem.GetComponent<Interactable>().objectName == "pot" && GameManagerScript.instance.orders.halfBoiledEggsPrep.isPotFilledWithWater)
            {
                prepFood = "Pot";
                prepType = "Boiling Eggs";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
                orders.halfBoiledEggsPrep.StartBoilingEggs();
                GameManagerScript.instance.prepStatusText.text = "Boiling: " + Mathf.FloorToInt(stove.ladenItem.GetComponent<LiquidHolder>().liquidGO.GetComponent<Food>().temperature) + "%";
                ShowPrepUI();
                prepStatusGO.SetActive(true);
                CheckItemsInteractibility();
            }
        }

        //Filling pot with water in the sink
        if (objectName == "sink")
        {
            Sink sink = GameManagerScript.instance.interactedItem.GetComponent<Sink>();
            Container sinkContainer = GameManagerScript.instance.interactedItem.GetComponent<Container>();
            if (sinkContainer.itemContained.GetComponent<Interactable>().objectName == "pot")
            {
                sink.liquidHolder = sinkContainer.itemContained.GetComponent<LiquidHolder>();
                prepFood = "Pot";
                prepType = "Filling Water";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
                orders.halfBoiledEggsPrep.StartFillingWater();
                ShowPrepUI();
                CheckItemsInteractibility();
            }
        }

        //Mortar (satay)
        if (objectName == "mortar")
        {
            //Adding ingredients
            if (!GameManagerScript.instance.orders.satayPrep.areAllAdded)
            {
                prepFood = "Marinate";
                prepType = "Adding Ingredients";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.transform.GetComponent<Interactable>().camPoint1);
                orders.satayPrep.StartAddingIngredients();
                ShowPrepUI();
                CheckItemsInteractibility();
            }

            //Grinding ingredients
            if (GameManagerScript.instance.orders.satayPrep.areAllAdded && !GameManagerScript.instance.orders.satayPrep.isMixGrinded && GameManagerScript.instance.accessedApparatus == "PESTLE")
            {
                prepFood = "Marinate";
                prepType = "Grinding Ingredients";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
                orders.satayPrep.StartGrindingIngredients();
                ShowPrepUI();
                CheckItemsInteractibility();
            }

            //Adding and mixing meat cubes
            if (GameManagerScript.instance.orders.satayPrep.isMixGrinded && GameManagerScript.instance.accessedApparatus == "SPATULA" && !GameManagerScript.instance.orders.satayPrep.isMeatMixed)
            {
                prepFood = "Meat Cubes";
                prepType = "Mixing Meat";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint2);
                GameManagerScript.instance.interactedItem.GetComponent<Interactable>().raycastAction = "Mix (Requires Spatula)";
                orders.satayPrep.StartMixingMeat();
                ShowPrepUI();
                CheckItemsInteractibility();
            }
        }

        //Threading Cubes
        if (objectName == "skewers" && GameManagerScript.instance.orders.satayPrep.isMeatMixed)
        {
            prepFood = "Meat Cubes";
            prepType = "Threading Cubes";
            camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
            orders.satayPrep.StartThreadingMeat();
            ShowPrepUI();
            CheckItemsInteractibility();
        }

        //Grilling Skewers
        if (objectName == "oven")
        {
            GameManagerScript.instance.interactedItem.GetComponent<Oven>().CheckForRack();
            if (GameManagerScript.instance.orders.satayPrep.isRackInOven)
            {
                prepFood = "Skewers";
                prepType = "Grilling";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
                orders.satayPrep.StartGrillingSkewers();
                ShowPrepUI();
                CheckItemsInteractibility();
            }
        }

        GameManagerScript.instance.orders.prepProgressBar.UpdateProgress();
    }

    public void ShowPrepUI()
    {
        prepTypeText.text = prepFood + ", " + prepType;
        prepUIGO.SetActive(true);
    }

    public void Pickup()  //Add an item to the player's inventory and set the item's transform to holding pos and rot
    {
        if ((GameManagerScript.instance.interactedItem.GetComponent<Food>() != null && GameManagerScript.instance.interactedItem.GetComponent<Food>().temperature < 47) ||
            GameManagerScript.instance.interactedItem.GetComponent<Food>() == null && GameManagerScript.instance.playerInventory.itemsHeld.Count < 3)
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
                GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingContainer.load -= GameManagerScript.instance.interactedItem.GetComponent<Interactable>().loadValue;
                GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingContainer.itemsContained.Remove(GameManagerScript.instance.interactedItem);
                GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingContainer.isContainingItem = false;
                GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingContainer.itemContained = null;
                GameManagerScript.instance.interactedItem.GetComponent<Interactable>().holdingContainer = null;          
            }

            GameManagerScript.instance.interactedItem.gameObject.SetActive(false);

            //Remove laden item from stove
            if (GameManagerScript.instance.interactedItem.GetComponentInParent<Transform>().GetComponentInParent<Stove>() != null)
            {
                if (GameManagerScript.instance.interactedItem == GameManagerScript.instance.playerControl.stove.ladenItem)
                {
                    GameManagerScript.instance.playerControl.stove.isLaden = false;
                    GameManagerScript.instance.playerControl.stove.ladenItem = null;
                    GameManagerScript.instance.playerControl.stove.stoveContainer.isContainingItem = false;

                    if (GameManagerScript.instance.interactedItem.GetComponent<Interactable>().objectName == "pot")
                    {
                        GameManagerScript.instance.orders.halfBoiledEggsPrep.isHeatingWater = false;
                    }
                }
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
        if (GameManagerScript.instance.container.AddLoad(tempObj.GetComponent<Interactable>().loadValue))
        {
            GameManagerScript.instance.container.Contain(GameManagerScript.instance.playerInventory.currentItemHeld, GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Interactable>().placeOffset);
            GameManagerScript.instance.playerInventory.RemoveItem(GameManagerScript.instance.playerInventory.currentItemHeld);
            tempObj.SetActive(true);

            tempObj.GetComponent<Interactable>().holdingContainer = GameManagerScript.instance.container;

            GameManagerScript.instance.isPlaceable = false;

            if (GameManagerScript.instance.playerControl.stove != null && GameManagerScript.instance.playerControl.stove.ladenItem == null)
            {
                GameManagerScript.instance.playerControl.stove.isLaden = true;
                GameManagerScript.instance.playerControl.stove.ladenItem = tempObj;

                if (tempObj.GetComponent<LiquidHolder>() != null)
                {
                    GameManagerScript.instance.playerControl.stove.ladenFood = tempObj.GetComponent<LiquidHolder>().liquidGO;
                }

                if (GameManagerScript.instance.interactedItem == GameManagerScript.instance.playerControl.stove.ladenItem)
                {
                    GameManagerScript.instance.playerControl.stove.UpdateLadenFood(GameManagerScript.instance.playerControl.stove.ladenItem);
                }
            }
        }
    }

    //Transfering the laden item of the currently held container into another empty container
    public void Transfer(Container otherContainer)
    {
        Container heldContainer = GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Container>();

        //heldContainer.load -= heldContainer.itemContained.GetComponent<Interactable>().loadValue;
        //otherContainer.load += heldContainer.itemContained.GetComponent<Interactable>().loadValue;

        for (int i = 0; i < heldContainer.itemsContained.Count; i++)
        {
            heldContainer.itemsContained[i].transform.parent = null;
            heldContainer.itemsContained[i].transform.position = otherContainer.placePoint.position + new Vector3(Random.Range(0f, 0.05f), Random.Range(0f, 0.005f), 0f);
            heldContainer.itemsContained[i].transform.rotation = otherContainer.placePoint.rotation * Quaternion.Euler(90f, 0f, Random.Range(-15f, 15f));
            heldContainer.itemsContained[i].transform.parent = otherContainer.transform;

            GameManagerScript.instance.playerInventory.SetLayerRecursively(heldContainer.itemsContained[i], LayerMask.NameToLayer("Default"));
            otherContainer.itemsContained.Add(heldContainer.itemsContained[i]);

            otherContainer.itemContained = heldContainer.itemContained;
            otherContainer.isContainingItem = true;
            heldContainer.itemContained = null;
        }
        heldContainer.itemsContained.Clear();
        GameManagerScript.instance.isTransferable = false;

        if (GameManagerScript.instance.interactedItem.GetComponent<Interactable>().objectName == "metalRack")
        {
            GameManagerScript.instance.interactedItem.GetComponent<MetalRack>().CheckSkewers();
        }
    }

    void CheckItemsInteractibility()
    {
        Interactable[] interactables = FindObjectsOfType<Interactable>();

        foreach (Interactable interactable in interactables)
        {
            //Checking for kaya toast
            //Slicing Edges
            if (prepType == "Slicing Edges" && interactable.objectName == "bread")
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Toasting bread
            if (prepType == "Toasting Bread" && (interactable.objectName == "skillet" || interactable.objectName == "stovePower"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            if (prepType == "Spreading Condiments" && (interactable.objectName == "bread" || interactable.objectName == "kaya" || interactable.objectName == "butter"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }


            //Checking for half-boiled eggs
            //Filling water
            if (prepType == "Filling Water" && interactable.objectName == "tap")
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            if (prepType == "Boiling Eggs" && (interactable.objectName == "pot" || interactable.objectName == "egg" || interactable.objectName == "stovePower"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }


            //Checking for satay
            //Adding ingredients
            if (prepType == "Adding Ingredients" && (interactable.objectName == "shallots" || interactable.objectName == "tumericPowder" || interactable.objectName == "mincedGarlic" ||
                interactable.objectName == "chilliPowder"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Grinding mix
            if (prepType == "Grinding Ingredients" && (interactable.objectName == "mortar"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Mixing meat
            if (prepType == "Mixing Meat" && (interactable.objectName == "meatCubes" || interactable.objectName == "mortar"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Threading meat
            if (prepType == "Threading Cubes" && (interactable.objectName == "skewers" || interactable.objectName == "mortar"))
            {
                interactable.isCurrentlyRaycastInteractable = true;

                if (interactable.objectName == "mortar")
                {
                    interactable.raycastAction = "Pickup Cube";
                }
            }

            //Grilling skewers
            if (prepType == "Grilling" && (interactable.objectName == "ovenDoor" || interactable.objectName == "posKnob" || interactable.objectName == "valueKnob"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }
        }
    }

    public void ResetAllInteractibility()
    {
        Interactable[] interactables = FindObjectsOfType<Interactable>();

        foreach (Interactable interactable in interactables)
        {
            interactable.isCurrentlyRaycastInteractable = false;
        }
    }    
}
