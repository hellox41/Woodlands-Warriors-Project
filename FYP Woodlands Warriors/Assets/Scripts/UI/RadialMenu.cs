using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RadialMenu : MonoBehaviour
{
    public string buttonType;

    public Food food;

    public PlayerControl playerControl;
    public MeowtiToolInteraction meowtiToolInteraction;

    Orders orders;
    public GameObject pickupButton;
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

        playerControl.HideRadialMenu();
    }

    void PrepSetup(string apparatusType, string foodType)
    {

        camTransition = mainCamera.GetComponent<CamTransition>();

        if (foodType == "BREAD")
        {
            prepFood = GameManagerScript.instance.interactedFood.GetComponent<Bread>().breadType + " Bread";
            if (apparatusType == "KNIFE")
            {
                if (orders.currentStage == 1)
                {
                    Debug.Log("Initiate cutting bread cooking event");
                    prepType = "Slicing Edges";
                    camTransition.MoveCamera(GameManagerScript.instance.interactedFood.GetComponent<Food>().camTransitionTransform1);
                    orders.kayaToastPrep.CutBread();                   
                }
            }
        }

        prepTypeText.text = prepFood + ", " + prepType;
        prepUIGO.SetActive(true);
    }



    public void Pickup()  //Add an item to the player's inventory
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
