using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public Transform placePoint;
    // Start is called before the first frame update
    void Start()
    {
        placePoint = GetComponentInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
