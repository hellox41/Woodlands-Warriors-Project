﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject meowtiToolGO;
    public GameObject radialMenuGO;
    public RadialMenu radialMenu;

    public MeowtiTool meowtiTool;

    public CanvasGroup toolCanvas;
    public CanvasGroup UICanvas;

    public Transform raycastPointTransform;

    public Camera playerView;

    public Vector3 toolZoomPos;
    public Vector3 toolZoomEuler;
    public Vector3 toolOriginalPos;
    public Vector3 toolOriginalEuler;

    public bool isMousingOverInteractible = false;
    public bool isMousingOverContainer = false;

    [SerializeField] float interactDistance = 50f;

    RaycastHit normalRaycastHit;
    RaycastHit cookingRaycastHit;

    public Outline outline = null;
    private Outline prevOutline = null;  //Outline for prep buttons
    private Outline previousOutline = null;  //Outline for interactable objects

    public Interactable interactable = null;

    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))   //Press R to toggle zoom on meow-ti tool
        {
            if (!GameManagerScript.instance.isZoomed)
            {
                //Zoom in on the meow-ti tool, enable cursor and tool canvas.

                meowtiToolGO.transform.localPosition = toolZoomPos;
                meowtiToolGO.transform.localRotation = Quaternion.Euler(toolZoomEuler);
                playerView.fieldOfView = 45;
                toolCanvas.interactable = true;
                UICanvas.gameObject.SetActive(false);
                meowtiTool.activeCanvas.interactable = true;
                GameManagerScript.instance.ChangeCursorLockedState(false);
                GameManagerScript.instance.isZoomed = true;
            }

            else if (GameManagerScript.instance.isZoomed)
            {
                //Zoom out of the meow-ti tool, disable cursor and tool canvas.

                if (!meowtiTool.isTyping)
                {
                    meowtiToolGO.transform.localPosition = toolOriginalPos;
                    meowtiToolGO.transform.localRotation = Quaternion.Euler(toolOriginalEuler);
                    playerView.fieldOfView = 70;
                    toolCanvas.interactable = false;
                    UICanvas.gameObject.SetActive(true);
                    meowtiTool.activeCanvas.interactable = false;
                    GameManagerScript.instance.ChangeCursorLockedState(true);
                    GameManagerScript.instance.isZoomed = false;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && GameManagerScript.instance.isPreparing)  //Get out of prep 
        {
            playerView.GetComponent<CamTransition>().MoveCamera(raycastPointTransform);
        }

        //Non-cooking raycasting
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out normalRaycastHit, interactDistance) && !GameManagerScript.instance.isPreparing)
        {
            if (normalRaycastHit.transform.GetComponent<Interactable>() != null)  //If mousing over interactable item,
            {
                isMousingOverInteractible = true;
                GameManagerScript.instance.interactedItem = normalRaycastHit.transform.gameObject;

                outline = normalRaycastHit.transform.GetComponent<Outline>();

                if (previousOutline == null)
                {
                    previousOutline = outline;
                }

                if (!outline.enabled)  //Enable outline
                {
                    outline.enabled = true;
                }
            }

            if (normalRaycastHit.transform.GetComponent<Interactable>() == null && outline != null)  //If looking at nothing, remove all variables
            {
                outline.enabled = false;
                outline = null;
                isMousingOverInteractible = false;

                if (radialMenuGO.activeInHierarchy)
                {
                    HideRadialMenu();
                }
            }

            if (normalRaycastHit.transform.GetComponent<Container>() != null)  //If looking at container, 
            {
                isMousingOverContainer = true;
                GameManagerScript.instance.container = normalRaycastHit.transform.GetComponent<Container>();  //Assign container variable in game manager script

                //Show placement preview if holding pickup/placeable item, and if container is not already containing another item
                if (inventory.currentItemHeld.GetComponent<Interactable>() != null && inventory.currentItemHeld.GetComponent<Interactable>().isPickup && !normalRaycastHit.transform.GetComponent<Container>().isContainingItem)
                {
                    if (inventory.currentItemHeld.name != "Meow-ti Tool")
                    {
                        GameManagerScript.instance.container.ShowPreview();
                        GameManagerScript.instance.isPlaceable = true;
                    }
                }
            }

            if (normalRaycastHit.transform.GetComponent<Container>() == null && isMousingOverContainer)  //Remove placement preview after looking away 
            {
                isMousingOverContainer = false;

                if (inventory.currentItemHeld.GetComponent<Interactable>() != null && inventory.currentItemHeld.GetComponent<Interactable>().isPickup && GameManagerScript.instance.container.isShowingPreview)
                {
                    GameManagerScript.instance.container.HidePreview();
                    GameManagerScript.instance.container.isShowingPreview = false;
                }
            }
        }

        if (normalRaycastHit.transform == null)  //Remove variables if not looking at anything
        {
            if (outline != null)             
            {
                outline.enabled = false;
            }

            if (GameManagerScript.instance.container != null && GameManagerScript.instance.container.isShowingPreview)  //Hide the preview
            {
                GameManagerScript.instance.container.HidePreview();
                GameManagerScript.instance.container.isShowingPreview = false;
            }

            isMousingOverInteractible = false;
            isMousingOverContainer = false;

            GameManagerScript.instance.isPlaceable = false;
        }

        if (previousOutline != null && outline != null && previousOutline != outline)  //If looking from container to container, disable previous container's outline and update variables
        {
            if (previousOutline.GetComponent<Container>() != null && previousOutline.GetComponent<Container>().isShowingPreview)
            {
                previousOutline.GetComponent<Container>().HidePreview();
            }

            //Show placement preview if holding pickup/placeable item, and if container is not already containing another item
            if (inventory.currentItemHeld.GetComponent<Interactable>() != null && inventory.currentItemHeld.GetComponent<Interactable>().isPickup)
            {
                if (!normalRaycastHit.transform.GetComponent<Container>().isContainingItem)
                {
                    GameManagerScript.instance.container.ShowPreview();
                    GameManagerScript.instance.isPlaceable = true;
                }

                else if (normalRaycastHit.transform.GetComponent<Container>().isContainingItem)
                {
                    GameManagerScript.instance.isPlaceable = false;
                }
            }
            previousOutline.enabled = false;
            GameManagerScript.instance.container = null;
            previousOutline = outline;
        }

        //Toggle outline on interactable objects in cursor raycast (in ingredient prep)
        if (GameManagerScript.instance.isPreparing && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out cookingRaycastHit, interactDistance))
        {

            if (cookingRaycastHit.transform.GetComponent<Interactable>() != null && cookingRaycastHit.transform.GetComponent<Interactable>().isRaycastButton)
            {
                prevOutline = cookingRaycastHit.transform.GetComponent<Outline>();
                prevOutline.enabled = true;
            }

            else if ((cookingRaycastHit.transform.GetComponent<Interactable>() == null || !cookingRaycastHit.transform.GetComponent<Interactable>().isRaycastButton) && prevOutline != null)
            {
                prevOutline.enabled = false;
                prevOutline = null;
            }
        }

        if (Input.GetMouseButtonDown(1) && isMousingOverInteractible && !GameManagerScript.instance.isInteracting && !GameManagerScript.instance.isPreparing)  //Bring up radial menu by right-clicking
        {
            interactable = outline.GetComponent<Interactable>();

            if (interactable.CheckCurrentlyInteractable())
            {
                GameManagerScript.instance.isInteracting = true;

                GameManagerScript.instance.ChangeCursorLockedState(false);
                GameManagerScript.instance.interactedFood = outline.GetComponent<Food>();

                radialMenu.CheckPrepareButton(interactable.isPreparable);
                radialMenu.CheckPickupButton(interactable.isPickup);
                radialMenu.CheckPlaceButton(GameManagerScript.instance.isPlaceable);

                radialMenuGO.SetActive(true);
            }
        }

        if (Input.GetMouseButtonUp(1) && GameManagerScript.instance.isInteracting)  //Hide radial menu when right click is released
        {
            HideRadialMenu();
            interactable = null;
        }

    }

    public void HideRadialMenu()
    {
        GameManagerScript.instance.isInteracting = false;
        GameManagerScript.instance.ChangeCursorLockedState(true);
        GameManagerScript.instance.interactedFood = null;

        radialMenuGO.SetActive(false);
    }
}
