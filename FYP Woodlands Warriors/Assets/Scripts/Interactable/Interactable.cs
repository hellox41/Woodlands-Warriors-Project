using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string objectName;
    public bool isPreparable;
    public bool isPickup;

    Outline outline;

    public Vector3 holdingPos;
    public Vector3 holdingRot;

    // Start is called before the first frame update
    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        CheckCurrentlyInteractable();
    }

    public bool CheckCurrentlyInteractable()
    {
        if (!isPreparable && !isPickup && !GameManagerScript.instance.isPlaceable)
        {
            return false;
        }

        return true;
    }
}
