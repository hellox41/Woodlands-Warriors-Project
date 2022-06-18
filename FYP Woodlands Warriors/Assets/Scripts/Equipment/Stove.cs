using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    public bool isLaden = false;

    public GameObject ladenItem;

    public Transform camTransitionTransform1;

    Container stoveContainer;
    // Start is called before the first frame update
    void Start()
    {
        stoveContainer = GetComponentInChildren<Container>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Container>() != null && !isLaden)
        {
            isLaden = true;
            ladenItem = other.gameObject;
            stoveContainer.isContainingItem = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == ladenItem && isLaden)
        {
            isLaden = false;
            ladenItem = null;
            stoveContainer.isContainingItem = false;
        }
    }
}
