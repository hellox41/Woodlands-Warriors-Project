using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    [Header("Stove Container")]
    public bool isLaden = false;

    public GameObject ladenItem;
    public GameObject ladenFood;

    public Transform camTransitionTransform1;

    public Container stoveContainer;

    [Space]

    [Header("Cooking Functionality")]
    public bool isPoweredOn = false;

    [SerializeField] float timeTillNextColorChange = 6f;
    [SerializeField] float heatingMultiplier = 2.5f;
    int rng;

    public Material stoveLightMat;

    public Light stoveIndicatorLight;

    [SerializeField] List<string> stoveLightColor = new List<string>();
    public string currentStoveLightColorName;

    //light color will rotate randomly between purple, red, and pink every 6 seconds
    public Color purpleColor; 
    public Color pinkColor; 
    public Color originalColor;

    Color switchedOnColor;

    // Start is called before the first frame update
    void Start()
    {
        stoveContainer = GetComponentInChildren<Container>();

        stoveIndicatorLight.enabled = false;
        stoveLightMat.DisableKeyword("_EMISSION");
        stoveLightMat.color = originalColor;

        switchedOnColor = Color.white;
        rng = Random.Range(3, 7);
        timeTillNextColorChange = rng;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPoweredOn)
        {
            timeTillNextColorChange -= Time.deltaTime;

            if (ladenFood != null)
            {
                ladenFood.GetComponent<Food>().temperature += Time.deltaTime * heatingMultiplier;
                
                if (GameManagerScript.instance.orders.halfBoiledEggsPrep != null && ladenFood.GetComponent<Food>().temperature <= 101)
                {
                    GameManagerScript.instance.prepStatusText.text = "Boiling: " +
                    Mathf.FloorToInt(GameManagerScript.instance.playerControl.stove.ladenItem.GetComponent<LiquidHolder>().liquidGO.GetComponent<Food>().temperature) + "%";
                }
            }

            if (timeTillNextColorChange <= 0)
            {
                ChangeLightColor();
                timeTillNextColorChange = rng;
            }       
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Container>() != null && !isLaden && other.gameObject.layer != LayerMask.NameToLayer("Holding"))
        {
            isLaden = true;
            ladenItem = other.gameObject;
            stoveContainer.isContainingItem = true;

            UpdateLadenFood(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == ladenItem && isLaden && other.gameObject.layer != LayerMask.NameToLayer("Holding"))
        {
            isLaden = false;
            ladenItem = null;
            stoveContainer.isContainingItem = false;
            ladenFood = null;

            if (other.gameObject.GetComponent<Interactable>().objectName == "pot")
            {
                GameManagerScript.instance.orders.halfBoiledEggsPrep.isHeatingWater = false;
            }
        }
    }

    public void ToggleStovePower()
    {
        isPoweredOn = !isPoweredOn;

        if (isPoweredOn)
        {
            if (ladenItem.GetComponent<Container>().itemContained != null)
            {
                ladenItem.GetComponent<Container>().itemContained.GetComponent<Food>().isBeingHeated = true;

                if (GameManagerScript.instance.orders.currentOrder == "HALF-BOILEDEGGS")
                {
                    GameManagerScript.instance.orders.halfBoiledEggsPrep.isHeatingWater = true;
                }
            }

            stoveIndicatorLight.enabled = true;
            stoveLightMat.EnableKeyword("_EMISSION");
            UpdateColors();
        }

        else if (!isPoweredOn)
        {
            if (ladenItem.GetComponent<Container>().itemContained != null)
            {
                ladenItem.GetComponent<Container>().itemContained.GetComponent<Food>().isBeingHeated = false;

                if (GameManagerScript.instance.orders.currentOrder == "HALF-BOILEDEGGS")
                {
                    GameManagerScript.instance.orders.halfBoiledEggsPrep.isHeatingWater = false;
                }
            }

            stoveIndicatorLight.enabled = false;
            stoveLightMat.DisableKeyword("_EMISSION");
            stoveLightMat.color = originalColor;
        }
    }

    //Switches stove light color to a random color (pink, red, or purple) that is not already being shown
    void ChangeLightColor()
    {
        string colorToChangeTo;

        do
        {
            colorToChangeTo = stoveLightColor[Random.Range(0, stoveLightColor.Count)];
        }
        while (colorToChangeTo == currentStoveLightColorName);

        if (currentStoveLightColorName != colorToChangeTo)
        {
            currentStoveLightColorName = colorToChangeTo;

            if (currentStoveLightColorName == "Purple")
            {
                switchedOnColor = purpleColor;
            }

            if (currentStoveLightColorName == "Pink")
            {
                switchedOnColor = pinkColor;
            }

            if (currentStoveLightColorName == "Red")
            {
                switchedOnColor = Color.red;
            }
        }

        if (isPoweredOn)
        {
            UpdateColors();
        }

        GameManagerScript.instance.orders.kayaToastPrep.isFlippedOnThisColor = false;
        rng = Random.Range(3, 7);
    }

    void UpdateColors()
    {
        stoveLightMat.color = switchedOnColor;
        stoveIndicatorLight.color = switchedOnColor;
        stoveLightMat.SetColor("_EmissionColor", switchedOnColor);
    }

    public void UpdateLadenFood(GameObject container)
    {
        ladenItem = container;

        if (container.GetComponent<Container>().itemContained != null && container.GetComponent<Container>().itemContained.GetComponent<Food>() != null)
        {
            ladenFood = container.GetComponent<Container>().itemContained;
        }   
    }
}
