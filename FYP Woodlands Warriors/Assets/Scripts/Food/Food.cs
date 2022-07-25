using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    //Possible food types: Bread, Butter, Kaya
    public string foodType;

    public bool isSolid;
    public bool isBeingHeated = false;
    public bool isBoiling = false;

    public float temperature = 28f;
    public int boilingPoint;

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

            if (GameManagerScript.instance.orders.halfBoiledEggsPrep != null && GameManagerScript.instance.orders.halfBoiledEggsPrep.isHeatingWater
                && !GameManagerScript.instance.orders.halfBoiledEggsPrep.isWaterBoiled)
            {
                GameManagerScript.instance.prepStatusText.text = "Boiling: " + 
                    Mathf.FloorToInt(GameManagerScript.instance.playerControl.stove.ladenItem.GetComponent<LiquidHolder>().liquidGO.GetComponent<Food>().temperature) + "%";
            }
        }

        if (!isBoiling && !isSolid && temperature >= boilingPoint)
        {
            Debug.Log("Boiled");
            isBoiling = true;

            if (GameManagerScript.instance.orders.halfBoiledEggsPrep != null && !GameManagerScript.instance.orders.halfBoiledEggsPrep.isWaterBoiled)
            {
                GameManagerScript.instance.orders.halfBoiledEggsPrep.isWaterBoiled = true;
                GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
                GameManagerScript.instance.orders.halfBoiledEggsPrep.savedBoilingEggsProgress++;

                GameObject[] eggs = GameObject.FindGameObjectsWithTag("Egg");

                for (int i = 0; i < eggs.Length; i++)
                {
                    eggs[i].GetComponent<Interactable>().isCurrentlyRaycastInteractable = true;
                }
            }
        }

        if (isBoiling && !isSolid && temperature < boilingPoint)
        {
            isBoiling = false;
        }
    }
}
