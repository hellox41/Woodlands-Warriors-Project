using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatayPrep : MonoBehaviour
{
    public int preparedCount = 0;

    public int[] dishTimes;
    [Header("Meat Type")]
    public string meatType;
    public string[] meatTypes;

    [Header("Adding Ingredients")]
    public float savedAddingProgress;
    public bool areShallotsAdded = false;
    public bool isTumericPowderAdded = false;
    public bool isMincedGarlicAdded = false;
    public bool isChilliPowderAdded = false;

    public bool areAllAdded = false;

    [Header("Grinding Ingredients")]
    public GameObject mixingBar;
    public float savedGrindingProgress;

    public bool isMixGrinded = false;

    [Header("Mixing Meat")]
    public float savedMixingProgress;
    public bool isMeatInMortar = false;
    public bool isMeatMixed = false;

    [Header("Threading Cubes")]
    public float savedThreadingProgress;
    public GameObject threadingUI;
    public bool isHoldingSkewer = false;
    public int cubesOnSkewer = 0;
    public int skewersPrepared = 0;
    public bool areSkewersPrepared = false;

    [Header("Grilling Skewers")]
    public bool areSkewersOnRack = false;
    public bool isRackInOven = false;
    public bool areSkewersGrilled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAddingIngredients()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetValue();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(4);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedAddingProgress);
    }

    public void StartGrindingIngredients()
    {
        mixingBar.SetActive(true);
        GameManagerScript.instance.orders.prepProgressBar.ResetValue();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(1);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedGrindingProgress);
    }

    public void StartMixingMeat()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetValue();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(10);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedMixingProgress);
    }

    public void StartThreadingMeat()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetValue();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(3);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(savedThreadingProgress);
    }

    public void ConfirmSkewer()
    {
        GameObject skewers = GameObject.Find("Skewers");
        Skewers skewersScript = skewers.GetComponent<Skewers>();

        if (!skewersScript.isSkewerMoving)
        {
            //Checking for beef cubes
            if (meatType == "Beef")
            {
                //If the 1st or 2nd skewer does not have 3 cubes
                if ((skewersPrepared == 0 || skewersPrepared == 1) && cubesOnSkewer != 3)
                {
                    GameManagerScript.instance.orders.dishQualityBar.AddProgress(-20f);
                }

                if (skewersPrepared == 2 && cubesOnSkewer != 4)
                {
                    GameManagerScript.instance.orders.dishQualityBar.AddProgress(-20f);
                }
            }

            //Checking for chicken cubes
            else if (meatType == "Chicken")
            {
                if (skewersPrepared == 0 && cubesOnSkewer != 5)
                {
                    GameManagerScript.instance.orders.dishQualityBar.AddProgress(-20f);
                }

                if ((skewersPrepared == 1 || skewersPrepared == 2) && cubesOnSkewer != 2)
                {
                    GameManagerScript.instance.orders.dishQualityBar.AddProgress(-20f);
                }
            }

            //Checking for mutton cubes
            else if (meatType == "Mutton")
            {
                if (skewersPrepared == 0 && cubesOnSkewer != 2)
                {
                    GameManagerScript.instance.orders.dishQualityBar.AddProgress(-20f);
                }

                if (skewersPrepared == 1 && cubesOnSkewer != 4)
                {
                    GameManagerScript.instance.orders.dishQualityBar.AddProgress(-20f);
                }

                if (skewersPrepared == 2 && cubesOnSkewer != 5)
                {
                    GameManagerScript.instance.orders.dishQualityBar.AddProgress(-20f);
                }
            }
            /*skewersScript.spawnedSkewer.transform.rotation = Quaternion.Euler(90f, 0f, Random.Range(-15f, 15f));
            skewersScript.spawnedSkewer.transform.position = GameObject.Find("skewerPlacePoint").transform.position + new Vector3(Random.Range(0f, 0.05f), Random.Range(0f, 0.005f), 0f);
            skewersScript.spawnedSkewer.transform.parent = GameObject.Find("meatPlate").transform;*/

            skewersScript.GetComponentInParent<Container>().Contain(skewersScript.spawnedSkewer, new Vector3(Random.Range(0f, 0.05f), Random.Range(0f, 0.005f), 0f));
            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
            threadingUI.SetActive(false);
            skewersPrepared++;
            isHoldingSkewer = false;
            cubesOnSkewer = 0;

            if (skewersPrepared == 3)
            {
                areSkewersPrepared = true;

                GameObject meatPlate = GameObject.Find("meatPlate");
                meatPlate.GetComponent<Interactable>().isPickup = true;
                Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
                Destroy(skewers);
            }
        }
    }

    public void DiscardSkewer()
    {
        Skewers skewersScript = GameObject.Find("Skewers").GetComponent<Skewers>();

        threadingUI.SetActive(false);
        skewersPrepared++;
        isHoldingSkewer = false;
        cubesOnSkewer = 0;

        Destroy(skewersScript.spawnedSkewer);
    }

    public void StartGrillingSkewers()
    {
        GameManagerScript.instance.orders.prepProgressBar.ResetValue();
        GameManagerScript.instance.orders.prepProgressBar.SetMaxProgress(1);
        GameManagerScript.instance.orders.prepProgressBar.SetProgress(0);
    }

    public void ResetVariables()
    {
        savedGrindingProgress = 0f;
        isMixGrinded = false;

        savedMixingProgress = 0f;
        isMeatInMortar = false;
        isMeatMixed = false;

        savedThreadingProgress = 0f;
        isHoldingSkewer = false;
        cubesOnSkewer = 0;
        skewersPrepared = 0;
        areSkewersPrepared = false;

        areSkewersOnRack = false;
        isRackInOven = false;
        areSkewersGrilled = false;
    }    
}
