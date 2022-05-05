using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject meowtiToolGO;

    public MeowtiTool meowtiTool;

    public CanvasGroup toolCanvas;

    public Camera playerView;

    public Vector3 toolZoomPos;
    public Vector3 toolZoomEuler;
    public Vector3 toolOriginalPos;
    public Vector3 toolOriginalEuler;

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
                    meowtiTool.activeCanvas.interactable = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    GameManagerScript.instance.isZoomed = false;
                }
            }
        }
    }
}
