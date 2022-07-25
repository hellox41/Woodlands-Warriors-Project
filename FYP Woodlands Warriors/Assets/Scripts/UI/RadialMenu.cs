using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//RadialMenu dictates prep sequences and general pick and place mechanic
public class RadialMenu : MonoBehaviour
{
    public string buttonType;

    public Food food;

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

        GameManagerScript.instance.playerControl.HideRadialMenu();
        GameManagerScript.instance.playerControl.interactable = null;
        GameManagerScript.instance.container = null;
        GameManagerScript.instance.playerControl.outline.enabled = false;
        GameManagerScript.instance.playerControl.outline = null;
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
            if (GameManagerScript.instance.accessedApparatus == "KNIFE")
            {
                //KAYATOAST
                //Spreading butter n kaya
                if (orders.kayaToastPrep.isBreadCut && orders.kayaToastPrep.isBreadToasted)
                {
                    prepFood = "Bread";
                    prepType = "Spreading Condiments";
                    GameManagerScript.instance.interactedFood.GetComponent<Interactable>().enabled = true;
                    orders.prepProgressBar.ResetValue();
                    orders.prepProgressBar.SetMaxProgress(2);
                    camTransition.MoveCamera(GameManagerScript.instance.interactedFood.GetComponent<Food>().camTransitionTransform2);
                    ShowPrepUI();
                    prepStatusGO.SetActive(true);
                    CheckItemsInteractibility();
                }   

                //KAYATOAST
                //Cutting bread
                else if (!orders.kayaToastPrep.isBreadCut)
                {
                    prepFood = "Bread";
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
            if (stove.isLaden && stove.ladenItem.GetComponent<Interactable>().objectName == "skillet" && stove.ladenItem.GetComponent<Container>().itemContained.GetComponent<Food>().foodType == "BREAD"
                && (orders.kayaToastPrep.breadType != "WHOLEWHEAT" || (orders.kayaToastPrep.breadType == "WHOLEWHEAT" && orders.kayaToastPrep.isBreadCut)))
            {
                prepFood = "Bread";
                prepType = "Toasting Bread";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponentInParent<Stove>().camTransitionTransform1);
                orders.kayaToastPrep.ToastBread();
                ShowPrepUI();
                CheckItemsInteractibility();
            }

            //HALF-BOILEDEGGS
            //Boiling eggs
            if (stove.isLaden && stove.ladenItem.GetComponent<Interactable>().objectName == "pot" && orders.halfBoiledEggsPrep.isPotFilledWithWater)
            {
                prepFood = "Pot";
                prepType = "Boiling Eggs";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
                orders.halfBoiledEggsPrep.StartBoilingEggs();
                float tempToDisplay = stove.ladenItem.GetComponent<LiquidHolder>().liquidGO.GetComponent<Food>().temperature;
                if (stove.ladenItem.GetComponent<LiquidHolder>().liquidGO.GetComponent<Food>().temperature > 100)
                {
                    tempToDisplay = 100;
                }
                GameManagerScript.instance.prepStatusText.text = "Boiling: " + Mathf.FloorToInt(tempToDisplay) + "%";
                ShowPrepUI();
                prepStatusGO.SetActive(true);
                CheckItemsInteractibility();
            }

            //NASILEMAK
            //Frying chicken
            if (stove.isLaden && stove.ladenItem.GetComponent<Interactable>().objectName == "skillet" && stove.ladenItem.GetComponent<Container>().itemContained.GetComponent<Food>().foodType == "CHICKEN")
            {
                prepFood = "Skillet";
                prepType = "Frying Chicken";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponentInParent<Stove>().camTransitionTransform1);
                orders.nasiLemakPrep.StartFryingChicken();
                ShowPrepUI();
                CheckItemsInteractibility();
            }

            //Frying sambal
            if (stove.isLaden && stove.ladenItem.GetComponent<Interactable>().objectName == "skillet" && stove.ladenItem.GetComponent<Container>().itemContained.GetComponent<Food>().foodType == "SAMBAL")
            {
                prepFood = "Skillet";
                prepType = "Cooking Sambal";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponentInParent<Stove>().camTransitionTransform1);
                orders.nasiLemakPrep.StartCookingSambal();
                ShowPrepUI();
                CheckItemsInteractibility();
            }
        }

        //Filling pot with water in the sink
        if (objectName == "sink")
        {
            Sink sink = GameManagerScript.instance.interactedItem.GetComponent<Sink>();
            Container sinkContainer = GameManagerScript.instance.interactedItem.GetComponent<Container>();

            //Fill with water (half-boiled eggs)
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

            //Washing rice
            if (sinkContainer.itemContained.GetComponent<Interactable>().objectName == "cookerPot")
            {
                sink.liquidHolder = sinkContainer.itemContained.GetComponent<LiquidHolder>();
                prepFood = "Cooker Pot";
                prepType = "Washing Rice";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
                orders.nasiLemakPrep.StartFillingPotWithWater();
                ShowPrepUI();
                CheckItemsInteractibility();
            }
        }

        //Mortar (satay)
        if (objectName == "mortar")
        {
            if (GameManagerScript.instance.orders.currentOrder == "SATAY")
            {
                //Adding ingredients
                if (!orders.satayPrep.areAllAdded)
                {
                    prepFood = "Marinate";
                    prepType = "Adding Ingredients";
                    camTransition.MoveCamera(GameManagerScript.instance.interactedItem.transform.GetComponent<Interactable>().camPoint1);
                    orders.satayPrep.StartAddingIngredients();
                    ShowPrepUI();
                    CheckItemsInteractibility();
                }

                //Grinding ingredients
                if (orders.satayPrep.areAllAdded && !orders.satayPrep.isMixGrinded && GameManagerScript.instance.accessedApparatus == "PESTLE")
                {
                    prepFood = "Marinate";
                    prepType = "Grinding Ingredients";
                    camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
                    orders.satayPrep.StartGrindingIngredients();
                    ShowPrepUI();
                    CheckItemsInteractibility();
                }

                //Adding and mixing meat cubes
                if (orders.satayPrep.isMixGrinded && GameManagerScript.instance.accessedApparatus == "SPATULA" && !orders.satayPrep.isMeatMixed)
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


            else if (GameManagerScript.instance.orders.currentOrder == "NASILEMAK")
            {
                if (!orders.nasiLemakPrep.areAllSambalAdded)
                {
                    prepFood = "Sambal";
                    prepType = "Adding Sambal Ingredients";
                    camTransition.MoveCamera(GameManagerScript.instance.interactedItem.transform.GetComponent<Interactable>().camPoint1);
                    orders.nasiLemakPrep.StartAddingSambalIngredients();
                    ShowPrepUI();
                    CheckItemsInteractibility();
                }

                else if (orders.nasiLemakPrep.areAllSambalAdded && !orders.nasiLemakPrep.isSambalGrinded && GameManagerScript.instance.accessedApparatus == "PESTLE")
                {
                    prepFood = "Sambal";
                    prepType = "Grinding Sambal Ingredients";
                    camTransition.MoveCamera(GameManagerScript.instance.interactedItem.transform.GetComponent<Interactable>().camPoint1);
                    orders.nasiLemakPrep.StartGrindingSambalIngredients();
                    ShowPrepUI();
                    CheckItemsInteractibility();
                }

                else if (orders.nasiLemakPrep.isSambalGrinded && GameManagerScript.instance.accessedApparatus == "SPATULA")
                {
                    prepFood = "Sambal Paste";
                    prepType = "Scooping Sambal";
                    camTransition.MoveCamera(GameManagerScript.instance.interactedItem.transform.GetComponent<Interactable>().camPoint1);
                    ShowPrepUI();
                    CheckItemsInteractibility();
                }
            }
        }

        //Threading Cubes
        if (objectName == "skewers" && orders.satayPrep.isMeatMixed)
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
            if (orders.satayPrep.isRackInOven)
            {
                prepFood = "Skewers";
                prepType = "Grilling";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
                orders.satayPrep.StartGrillingSkewers();
                ShowPrepUI();
                CheckItemsInteractibility();
            }
        }

        //Cooker pot (nasilemak)
        if (objectName == "cookerPot")
        {
            if (!orders.nasiLemakPrep.isRiceInPot)
            {
                prepFood = "Rice Pot";
                prepType = "Adding Rice";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
                orders.nasiLemakPrep.StartAddingRice();
                ShowPrepUI();
                CheckItemsInteractibility();
            }

            else if (orders.nasiLemakPrep.isRiceCooked && orders.nasiLemakPrep.isRiceInPot)
            {
                if (GameManagerScript.instance.accessedApparatus == "PADDLE")
                {
                    prepFood = "Rice Pot";
                    prepType = "Scooping Rice";
                    camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint2);
                    ShowPrepUI();
                    CheckItemsInteractibility();
                }
            }
        }

        //Rice cooker appliance
        if (objectName == "riceCooker")
        {
            if (orders.nasiLemakPrep.isPotInCooker && !orders.nasiLemakPrep.isRiceCooked 
                && orders.nasiLemakPrep.isPotFilledWithWater)
            {
                prepFood = "Rice";
                prepType = "Cooking Rice";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
                orders.nasiLemakPrep.StartCookingRice();
                ShowPrepUI();
                CheckItemsInteractibility();
            }
        }

        //Mixing bowl
        if (objectName == "mixingBowl")
        {
            if (!orders.nasiLemakPrep.areAllAdded)
            {
                prepFood = "Marinate";
                prepType = "Adding Ingredients";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint1);
                orders.nasiLemakPrep.StartAddingIngredients();
                ShowPrepUI();
                CheckItemsInteractibility();
            }

            else if (orders.nasiLemakPrep.areAllAdded && GameManagerScript.instance.accessedApparatus == "SPATULA" && !orders.nasiLemakPrep.isMarinateMixed)
            {
                prepFood = "Marinate";
                prepType = "Mixing Ingredients";
                camTransition.MoveCamera(GameManagerScript.instance.interactedItem.GetComponent<Interactable>().camPoint2);
                orders.nasiLemakPrep.StartMixingIngredients();
                ShowPrepUI();
                CheckItemsInteractibility();
            }
        }
        orders.prepProgressBar.UpdateProgress();
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
                        orders.halfBoiledEggsPrep.isHeatingWater = false;
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
            heldContainer.itemsContained[i].transform.rotation = otherContainer.placePoint.rotation * Quaternion.Euler(0f, 0f, Random.Range(-15f, 15f));
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
            else if (prepType == "Toasting Bread" && (interactable.objectName == "skillet" || interactable.objectName == "stovePower"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            else if (prepType == "Spreading Condiments" && (interactable.objectName == "bread" || interactable.objectName == "kaya" || interactable.objectName == "butter"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }


            //Checking for half-boiled eggs
            //Filling water
            else if (prepType == "Filling Water" && interactable.objectName == "tap")
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            else if (prepType == "Boiling Eggs" && (interactable.objectName == "pot" || interactable.objectName == "stovePower"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }


            //Checking for satay
            //Adding ingredients
            else if (prepType == "Adding Ingredients" && orders.currentOrder == "SATAY" && (interactable.objectName == "shallots" || 
                interactable.objectName == "tumericPowder" || interactable.objectName == "mincedGarlic" || interactable.objectName == "chilliPowder"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Grinding mix
            else if (prepType == "Grinding Ingredients" && (interactable.objectName == "mortar"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Mixing meat
            else if (prepType == "Mixing Meat" && (interactable.objectName == "meatCubes" || interactable.objectName == "mortar"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Threading meat
            else if (prepType == "Threading Cubes" && (interactable.objectName == "skewers" || interactable.objectName == "mortar"))
            {
                interactable.isCurrentlyRaycastInteractable = true;

                if (interactable.objectName == "mortar")
                {
                    interactable.raycastAction = "Pickup Cube";
                }
            }

            //Grilling skewers
            else if (prepType == "Grilling" && (interactable.objectName == "ovenDoor" || interactable.objectName == "posKnob" || interactable.objectName == "valueKnob"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }


            //NASILEMAK
            //Adding rice
            else if (prepType == "Adding Rice" && interactable.objectName == "ricePacket")
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Washing rice
            else if (prepType == "Washing Rice" && interactable.objectName == "tap")
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Cooking rice
            else if (prepType == "Cooking Rice" && (interactable.objectName == "coconutMilk" || interactable.objectName == "pandan" || interactable.objectName == "cookerButton"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Scooping rice
            else if (prepType == "Scooping Rice" && (interactable.objectName == "cookedRice"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Adding marinate ingredients (nasi lemak)
            else if (prepType == "Adding Ingredients" && orders.currentOrder == "NASILEMAK" && (interactable.objectName == "chicken" ||
    interactable.objectName == "tumericPowder" || interactable.objectName == "mincedGarlic" || interactable.objectName == "saltShaker"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Mixing chicken with marinate
            else if (prepType == "Mixing Ingredients" && interactable.objectName == "mixingBowl")
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Frying tumeric chicken
            else if (prepType == "Frying Chicken" && (interactable.objectName == "stovePower" || interactable.objectName == "skillet"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            //Adding sambal ingredients
            else if (prepType == "Adding Sambal Ingredients" && (interactable.objectName == "chilliPadi" || interactable.objectName == "shallots" || interactable.objectName == "mincedGarlic"
                || interactable.objectName == "shrimpPaste" || interactable.objectName == "waterCup"))
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            else if (prepType == "Grinding Sambal Ingredients" && interactable.objectName == "mortar")
            {
                interactable.isCurrentlyRaycastInteractable = true;
            }

            else if (prepType == "Scooping Sambal" && interactable.objectName == "sambalPaste")
            {
                interactable.isCurrentlyRaycastInteractable = true;
                interactable.GetComponent<Collider>().enabled = true;
            }

            else if (prepType == "Cooking Sambal" && (interactable.objectName == "skillet" || interactable.objectName == "stovePower" || interactable.objectName == "oilBottle"
                || interactable.objectName == "anchovyBowl"))
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
