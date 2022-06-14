using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    public bool isLaden = false;

    public GameObject ladenItem;

    public Transform camTransitionTransform1;
    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == ladenItem && isLaden)
        {
            isLaden = false;
            ladenItem = null;
        }
    }
}
