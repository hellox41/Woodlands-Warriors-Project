using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
   public bool isPouringWater = false;

    public float waterOutputModifier = 7f;

    Container sinkContainer;

    public LiquidHolder liquidHolder;

    public GameObject waterPour;

    // Start is called before the first frame update
    void Start()
    {
        sinkContainer = GetComponent<Container>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //Pouring water into pot
        if (isPouringWater && sinkContainer.itemContained.GetComponent<Interactable>().objectName == "pot")
        {
            liquidHolder = sinkContainer.itemContained.GetComponent<LiquidHolder>();

            //Change liquid type to water
            if (liquidHolder.liquidType != "Water")
            {
                liquidHolder.liquidType = "Water";
            }

            //Enable the water level visuals
            if (!liquidHolder.liquidGO.activeInHierarchy)
            {
                liquidHolder.liquidGO.SetActive(true);
            }

            //Increase the water level of the liquid holder (pot)
            if (liquidHolder.currentLevel < liquidHolder.capacity)
            {
                GameManagerScript.instance.orders.progressBar.AddProgress(Mathf.FloorToInt(Time.fixedDeltaTime * waterOutputModifier));
                liquidHolder.currentLevel += Time.fixedDeltaTime * waterOutputModifier;
            }

            //Enable isBoilable when there is 1 litre of water in the pot
            if (!liquidHolder.isBoilable && liquidHolder.currentLevel > 1000f)
            {
                liquidHolder.isBoilable = true;
            }
        }
    }

    public void TurnTap()
    {
        isPouringWater = !isPouringWater;

        if (isPouringWater)
        {
            waterPour.SetActive(true);
        }

        else if (!isPouringWater)
        {
            waterPour.SetActive(false);
        }
    }
}
