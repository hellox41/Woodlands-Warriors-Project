using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string objectName;
    public string raycastAction;

    public bool isPreparable;
    public bool isPickup;
    public bool isRaycastButton;
    public bool isInInventory = false;

    public bool isCurrentlyRaycastInteractable = false;

    public int loadValue;

    public Container holdingContainer;

    Outline outline;

    public Vector3 holdingPos;
    public Vector3 holdingRot;
    public Vector3 placeOffset;

    public Transform camPoint1;
    public Transform camPoint2;

    public AudioClip interactAudio;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        CheckCurrentlyInteractable();
    }

    public bool CheckCurrentlyInteractable()
    {
        if (isRaycastButton && GameManagerScript.instance.isPreparing)
        {
            return true;
        }

        if (isPreparable || isPickup)
        {
            return true;
        }

        if (!isPreparable && !isPickup && !GameManagerScript.instance.isPlaceable && GetComponent<Container>() == null)
        {
            return false;
        }

        else if (GetComponent<Container>() != null)
        {
            return true;
        }

        else return false;
    }
}

