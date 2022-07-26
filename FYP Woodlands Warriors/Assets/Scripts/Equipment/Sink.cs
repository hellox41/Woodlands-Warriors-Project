using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : MonoBehaviour
{
    public AudioSource sinkAudio;
   public bool isPouringWater = false;

    public float waterOutputModifier;

    Container sinkContainer;

    public LiquidHolder liquidHolder;

    public GameObject waterPour;

    // Start is called before the first frame update
    void Start()
    {
        sinkContainer = GetComponent<Container>();
        sinkAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //Pouring water into pot
        if (isPouringWater && (sinkContainer.itemContained.GetComponent<Interactable>().objectName == "pot" ||
            sinkContainer.itemContained.GetComponent<Interactable>().objectName == "cookerPot"))
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
                liquidHolder.currentLevel += Time.fixedDeltaTime * waterOutputModifier;
                GameManagerScript.instance.orders.prepProgressBar.AddProgress(Time.fixedDeltaTime * waterOutputModifier);
                if (GameManagerScript.instance.orders.currentOrder == "HALF-BOILEDEGGS")
                {
                    GameManagerScript.instance.orders.halfBoiledEggsPrep.savedFillingWaterProgress += Time.fixedDeltaTime * waterOutputModifier;
                }

                if (GameManagerScript.instance.orders.prepProgressBar.slider.value >= 1000)
                {
                    isPouringWater = false;
                    waterPour.SetActive(false);
                    Camera.main.transform.GetComponent<CamTransition>().MoveCamera(GameManagerScript.instance.playerControl.raycastPointTransform);
                    sinkAudio.Stop();
                }
            }

            //Enable isBoilable when there is 1 litre of water in the pot
            if (!liquidHolder.isBoilable && liquidHolder.currentLevel > 1000f)
            {
                liquidHolder.isBoilable = true;
                if (GameManagerScript.instance.orders.currentOrder == "HALF-BOILEDEGGS")
                {
                    GameManagerScript.instance.orders.halfBoiledEggsPrep.isPotFilledWithWater = true;
                }
                
                else if (GameManagerScript.instance.orders.currentOrder == "NASILEMAK")
                {
                    GameManagerScript.instance.orders.nasiLemakPrep.isPotFilledWithWater = true;
                }
            }
        }
    }
}
