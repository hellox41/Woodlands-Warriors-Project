using System.Collections;
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

    RaycastHit raycastHit;

    public Outline outline = null;
    private Outline previousOutline = null;

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

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, interactDistance) && !GameManagerScript.instance.isPreparing)
        {
            if (raycastHit.transform.GetComponent<Interactable>() != null)  //If mousing over interactable item,
            {
                isMousingOverInteractible = true;
                GameManagerScript.instance.interactedItem = raycastHit.transform.gameObject;

                outline = raycastHit.transform.GetComponent<Outline>();

                if (previousOutline == null)
                {
                    previousOutline = outline;
                }

                if (!outline.enabled)  //Enable outline
                {
                    outline.enabled = true;
                }
            }

            if (raycastHit.transform.GetComponent<Interactable>() == null && outline != null)  //If looking at nothing, remove all variables
            {
                outline.enabled = false;
                outline = null;
                isMousingOverInteractible = false;

                if (radialMenuGO.activeInHierarchy)
                {
                    HideRadialMenu();
                }
            }

            if (raycastHit.transform.GetComponent<Container>() != null && isMousingOverInteractible)  //If looking at container, 
            {
                isMousingOverContainer = true;
                GameManagerScript.instance.container = raycastHit.transform.GetComponent<Container>();

                if (inventory.currentItemHeld.GetComponent<Food>() != null)  //Show placement preview if holding food
                {
                    GameManagerScript.instance.container.ShowPreview();
                    GameManagerScript.instance.isPlaceable = true;
                }
            }

            if (raycastHit.transform.GetComponent<Container>() == null && isMousingOverContainer)  //Remove placement preview after looking away 
            {
                isMousingOverContainer = false;

                if (inventory.currentItemHeld.GetComponent<Food>() != null && GameManagerScript.instance.container.isShowingPreview)
                {
                    Destroy(GameManagerScript.instance.container.previewedFood);
                    GameManagerScript.instance.container.isShowingPreview = false;
                }
            }
        }

        if (raycastHit.transform == null)  //Remove variables if not looking at anything
        {
            if (outline != null)             
            {
                outline.enabled = false;
            }

            if (GameManagerScript.instance.container != null && GameManagerScript.instance.container.isShowingPreview)
            {
                Destroy(GameManagerScript.instance.container.previewedFood);
                GameManagerScript.instance.container.isShowingPreview = false;
            }

            isMousingOverInteractible = false;
            isMousingOverContainer = false;

            GameManagerScript.instance.isPlaceable = false;
        }

        if (previousOutline != null && outline != null && previousOutline != outline)
        {
            if (previousOutline.GetComponent<Container>() != null && previousOutline.GetComponent<Container>().isShowingPreview)
            {
                Destroy(previousOutline.GetComponent<Container>());
                previousOutline.GetComponent<Container>().isShowingPreview = false;
            }
            previousOutline.enabled = false;
            previousOutline = outline;
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
