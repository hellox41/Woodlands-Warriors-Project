﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerControl : MonoBehaviour
{
    public GameObject meowtiToolGO;
    public GameObject radialMenuGO;
    public GameObject raycastActionTooltip;
    public RadialMenu radialMenu;

    public TMP_Text raycastActionTooltipText;

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
    public Outline prevOutline = null;  //Outline for prep buttons
    private Outline previousOutline = null;  //Outline for interactable objects

    public Interactable interactable = null;

    // Start is called before the first frame update
    void Start()
    {
        raycastActionTooltip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !GameManagerScript.instance.isPreparing)   //Press R to toggle zoom on meow-ti tool
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

        //Non-cooking raycasting
        if (!GameManagerScript.instance.isPreparing && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out normalRaycastHit, interactDistance))
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

                if (GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Interactable>() != null && !normalRaycastHit.transform.GetComponent<Container>().isContainingItem)
                {
                    //Show placement preview if holding pickup/placeable item, and if container is not already containing another item
                    if ((GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Container>() == null &&
                        GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Interactable>().isPickup) ||
                        (GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Container>() != null && 
                        normalRaycastHit.transform.GetComponent<Container>().canContainContainers))
                    {
                        GameManagerScript.instance.container.ShowPreview();
                        GameManagerScript.instance.isPlaceable = true;
                    }

                    //Enable isTransferable if holding a container which is containing food, and looking at another empty container 
                    if (GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Container>() != null &&
                        GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Container>().isContainingItem)
                    {
                        GameManagerScript.instance.isTransferable = true;
                    }
                }
            }

            if (normalRaycastHit.transform.GetComponent<Container>() == null && isMousingOverContainer)  //Remove placement preview after looking away 
            {
                isMousingOverContainer = false;

                if (GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Interactable>() != null &&
                    GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Interactable>().isPickup &&
                    GameManagerScript.instance.container.isShowingPreview)
                {
                    GameManagerScript.instance.container.HidePreview();
                    GameManagerScript.instance.container.isShowingPreview = false;
                    GameManagerScript.instance.isPlaceable = false;
                    GameManagerScript.instance.isTransferable = false;
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
            GameManagerScript.instance.isTransferable = false;
        }

        if (previousOutline != null && outline != null && previousOutline != outline)  //If looking from container to container, disable previous container's outline and update variables
        {
            if (previousOutline.GetComponent<Container>() != null && previousOutline.GetComponent<Container>().isShowingPreview)
            {
                previousOutline.GetComponent<Container>().HidePreview();
            }

            //Show placement preview if holding pickup/placeable item, and if container is not already containing another item
            if (GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Interactable>() != null && 
                GameManagerScript.instance.playerInventory.currentItemHeld.GetComponent<Interactable>().isPickup && 
                normalRaycastHit.transform.GetComponent<Container>() != null)
            {
                if (!normalRaycastHit.transform.GetComponent<Container>().isContainingItem && normalRaycastHit.transform.GetComponent<Container>().canContainContainers)
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

                raycastActionTooltipText.text = cookingRaycastHit.transform.GetComponent<Interactable>().raycastAction;
                ShowActionTooltip();
            }

            else if ((cookingRaycastHit.transform.GetComponent<Interactable>() == null || !cookingRaycastHit.transform.GetComponent<Interactable>().isRaycastButton) && prevOutline != null)
            {
                prevOutline.enabled = false;
                prevOutline = null;

                HideActionTooltip();
            }
        }

        if (Input.GetMouseButtonDown(1) && isMousingOverInteractible && !GameManagerScript.instance.isInteracting && !GameManagerScript.instance.isPreparing)  //Bring up radial menu by right-clicking
        {
            interactable = outline.GetComponent<Interactable>();

            if (interactable.CheckCurrentlyInteractable() && !radialMenuGO.activeInHierarchy)
            {            
                GameManagerScript.instance.isInteracting = true;

                GameManagerScript.instance.ChangeCursorLockedState(false);
                GameManagerScript.instance.interactedFood = outline.GetComponent<Food>();

                radialMenu.CheckPrepareButton(interactable.isPreparable);
                radialMenu.CheckPickupButton(interactable.isPickup);
                radialMenu.CheckPlaceButton(GameManagerScript.instance.isPlaceable);
                radialMenu.CheckTransferButton(GameManagerScript.instance.isTransferable);

                radialMenuGO.SetActive(true);
            }
        }

        if (Input.GetMouseButtonUp(1) && GameManagerScript.instance.isInteracting)  //Hide radial menu when right click is released
        {
            HideRadialMenu();
            interactable = null;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && GameManagerScript.instance.isPreparing)  //Get out of prep 
        {
            playerView.GetComponent<CamTransition>().MoveCamera(raycastPointTransform);
        }

        if (Input.GetKeyDown(KeyCode.Tab))  //Toggle shrink or expand order on the left side of the screen
        {
            GameManagerScript.instance.orders.ToggleOrderUI(GameManagerScript.instance.isOrderUIShrunk);
        }

        if (raycastActionTooltip.activeInHierarchy)
        {
            TooltipFollowCursor();
        }
    }

    public void HideRadialMenu()
    {
        GameManagerScript.instance.isInteracting = false;
        GameManagerScript.instance.ChangeCursorLockedState(true);
        GameManagerScript.instance.interactedFood = null;

        radialMenuGO.SetActive(false);
    }

    public void ShowActionTooltip()
    {
        if (!raycastActionTooltip.activeInHierarchy)
        {
            raycastActionTooltip.SetActive(true);
        }
    }

    public void HideActionTooltip()
    {
        if (raycastActionTooltip.activeInHierarchy)
        {
            raycastActionTooltip.SetActive(false);
        }
    }

    public void TooltipFollowCursor()
    {
        raycastActionTooltip.transform.position = Input.mousePosition + new Vector3(80f, -30f, 0);
    }
}
