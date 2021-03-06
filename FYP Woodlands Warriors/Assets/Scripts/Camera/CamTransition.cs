using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTransition : MonoBehaviour
{
    public AudioClip camTransSfx;
    [Range(1, 10)]
    [SerializeField] float transitionSpeed = 6f;

    public MeshRenderer playerMr;
    public GameObject heldItemObj;

    public Transform pointToMoveTo;
    public Transform defaultCamTransform;

    [SerializeField] GameObject crosshairGO;
    [SerializeField] GameObject playerGO;
    [SerializeField] GameObject prepUIGO;

    [SerializeField] List<GameObject> children = new List<GameObject>();

    private void Start()
    {
        defaultCamTransform = GameObject.Find("Raycast").GetComponent<Transform>();

        foreach (Transform child in transform)
        {
            children.Add(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManagerScript.instance.isCamTransitioning)
        {
            transform.position = Vector3.Lerp(transform.position, pointToMoveTo.position, transitionSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, pointToMoveTo.localRotation.eulerAngles, transitionSpeed * Time.deltaTime));

            if (Vector3.Distance(transform.position, pointToMoveTo.position) < 0.005f)
            {
                GameManagerScript.instance.isCamTransitioning = false;

                GameManagerScript.instance.ChangeCursorLockedState(false);

                if (pointToMoveTo == defaultCamTransform)  //If resetting camera to first person view
                {
                    transform.position = defaultCamTransform.position;
                    transform.rotation = Quaternion.Euler(defaultCamTransform.rotation.eulerAngles);
                    transform.parent = playerGO.transform;

                    if (!GameManagerScript.instance.isShowcasing)
                    {
                        foreach (GameObject obj in children)
                        {
                            obj.transform.parent = transform;
                            obj.transform.localPosition = new Vector3(0, 0, 0);
                            obj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        }
                        GameManagerScript.instance.ChangeCursorLockedState(true);
                    }

                    crosshairGO.SetActive(true);
                    prepUIGO.SetActive(false);
                    if (GameManagerScript.instance.isPreparing)
                    {
                        GameManagerScript.instance.isPreparing = false;

                        heldItemObj.SetActive(true);

                        GameManagerScript.instance.radialMenu.ResetAllInteractibility();
                    }

                    GameManagerScript.instance.playerControl.HideActionTooltip();

                    if (GameManagerScript.instance.playerControl.prevOutline != null)
                    {
                        GameManagerScript.instance.playerControl.prevOutline.enabled = false;
                        GameManagerScript.instance.playerControl.prevOutline = null;
                    }
                }

                if (GameManagerScript.instance.isShowcasing)
                {
                    Camera.main.transform.position = GameManagerScript.instance.orders.foodShowcaseTrans.position + GameManagerScript.instance.orders.camOffset;
                }
            }
        }
    }

    public void MoveCamera(Transform transitionPoint)  //Detach children and parents, start the camera transition, and enable cursor
    {
        GameManagerScript.instance.orders.sfxAudioSource.PlayOneShot(camTransSfx, 0.3f);
        playerMr.enabled = false;
        heldItemObj = GameManagerScript.instance.playerInventory.currentItemHeld;
        heldItemObj.SetActive(false);

        GameManagerScript.instance.isCamTransitioning = true;
        if (GameManagerScript.instance.interactedFood != null)
        {
            GameManagerScript.instance.interactedFood.GetComponent<Outline>().enabled = false;
        }

        crosshairGO.SetActive(false);
        pointToMoveTo = transitionPoint;
        GameManagerScript.instance.isPreparing = true;

        transform.DetachChildren();
        transform.parent = null;
    }

    public void ResetCameraTransform()
    {
        GameManagerScript.instance.orders.sfxAudioSource.PlayOneShot(camTransSfx, 0.3f);
        pointToMoveTo = defaultCamTransform;
        GameManagerScript.instance.isCamTransitioning = true;
    }
}
