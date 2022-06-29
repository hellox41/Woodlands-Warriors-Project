using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    //Possible food types: Bread, Butter, Kaya
    public string foodType;

    public bool isSolid;
    public bool isBeingHeated = false;

    public float temperature = 28f;

    //Transform points for the camera to transition to when preparing food
    public Transform camTransitionTransform1;
    public Transform camTransitionTransform2;
    public Transform camTransitionTransform3;
    public Transform camTransitionTransform4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Reduce temperature of food to room temp if not being heated
        if (!isBeingHeated && temperature > GameManagerScript.instance.roomTemperature)
        {
            temperature -= Time.deltaTime * 0.5f;
        }
    }
}
