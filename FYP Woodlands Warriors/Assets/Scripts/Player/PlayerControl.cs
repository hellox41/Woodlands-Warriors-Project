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
    bool isMousingOverContainer = false;
    [SerializeField] float interactDistance = 50f;

    RaycastHit raycastHit;
    public Outline outline = null;
    public Interactable interactable = null;
    public Inventory inventory;
    private Container container;

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
            if (raycastHit.transform.GetComponent<Interactable>() != null)
            {
                isMousingOverInteractible = true;

                outline = raycastHit.transform.GetComponent<Outline>();
                if (!outline.enabled)
                {
                    outline.enabled = true;
                }
            }

            else if (raycastHit.transform.GetComponent<Interactable>() == null && outline != null)
            {
                outline.enabled = false;
                isMousingOverInteractible = false;
            }

            if (raycastHit.transform.GetComponent<Container>() != null && isMousingOverInteractible)
            {
                isMousingOverContainer = true;
                container = raycastHit.transform.GetComponent<Container>();
            }
        }

        if (outline != null && Vector3.Distance(transform.position, outline.transform.position) > 1.75) 
        {
            outline.enabled = false;
            isMousingOverInteractible = false;
            isMousingOverContainer = false;
        }

        if (Input.GetMouseButtonDown(1) && isMousingOverInteractible && !GameManagerScript.instance.isInteracting && !GameManagerScript.instance.isPreparing)  //Bring up radial menu by right-clicking
        {
            interactable = outline.GetComponent<Interactable>();
            GameManagerScript.instance.isInteracting = true;

            GameManagerScript.instance.ChangeCursorLockedState(false);
            GameManagerScript.instance.interactedFood = outline.GetComponent<Food>();

            radialMenu.CheckPickupButton(interactable.isPickup);
            radialMenuGO.SetActive(true);
        }

        if (Input.GetMouseButtonUp(1) && GameManagerScript.instance.isInteracting)
        {
            HideRadialMenu();
        }

        if (Input.GetKeyDown(KeyCode.F) && isMousingOverContainer && !GameManagerScript.instance.isInteracting && inventory.currentItemHeld.GetComponent<Food>() != null)
        {
            PlaceFood();
        }
    }

    public void HideRadialMenu()
    {
        GameManagerScript.instance.isInteracting = false;
        GameManagerScript.instance.ChangeCursorLockedState(true);
        GameManagerScript.instance.interactedFood = null;

        radialMenuGO.SetActive(false);
    }

    void PlaceFood()
    {
        
    }
}
