using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject meowtiToolGO;
    public GameObject radialMenu;

    public MeowtiTool meowtiTool;

    public CanvasGroup toolCanvas;
    public CanvasGroup UICanvas;

    public Camera playerView;

    public Vector3 toolZoomPos;
    public Vector3 toolZoomEuler;
    public Vector3 toolOriginalPos;
    public Vector3 toolOriginalEuler;

    public bool isMousingOverInteractible = false;
    [SerializeField] float interactDistance = 50f;

    RaycastHit raycastHit;
    Outline outline = null;

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
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    GameManagerScript.instance.isZoomed = false;
                }
            }
        }

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out raycastHit, interactDistance))
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

            else if (raycastHit.transform.GetComponent<Interactable>() == null)
            {
                outline.enabled = false;
                isMousingOverInteractible = false;
            }
        }

        if (Vector3.Distance(transform.position, outline.transform.position) > 1.75)
        {
            outline.enabled = false;
            isMousingOverInteractible = false;
        }

        if (Input.GetMouseButtonDown(1) && isMousingOverInteractible && !GameManagerScript.instance.isInteracting)
        {
            GameManagerScript.instance.isInteracting = true;
            radialMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonUp(1) && GameManagerScript.instance.isInteracting)
        {
            GameManagerScript.instance.isInteracting = false;
            radialMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void Prepare(string prepType)
    {
        prepType = "CUT/SLICE";
        Debug.Log("Preparing the " + outline.GetComponent<Food>().foodType + " via " + prepType);
    }
}
