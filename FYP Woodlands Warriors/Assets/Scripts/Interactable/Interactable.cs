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

    public Container holdingContainer;

    Outline outline;

    public Vector3 holdingPos;
    public Vector3 holdingRot;

    public Transform camPoint1;
    public Transform camPoint2;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        CheckCurrentlyInteractable();
    }

    public bool CheckCurrentlyInteractable()
    {
        if (!isPreparable && !isPickup && !GameManagerScript.instance.isPlaceable && isRaycastButton && GetComponent<Container>() == null)
        {
            return false;
        }

        else return true;
    }
}
