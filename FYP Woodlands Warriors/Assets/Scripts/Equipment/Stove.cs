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

    public Material stoveLightMat;

    public Light stoveIndicatorLight;

    [SerializeField] List<string> stoveLightColor = new List<string>();
    public string currentStoveLightColorName;

    //light color will rotate randomly between purple, red, and pink every 6 seconds
    public Color purpleColor; 
    public Color pinkColor; 
    public Color originalColor;

    Color switchedOnColor;

    [SerializeField] KayaToastPrep kayaToastPrep;

    // Start is called before the first frame update
    void Start()
    {
        stoveContainer = GetComponentInChildren<Container>();

        stoveIndicatorLight.enabled = false;
        stoveLightMat.DisableKeyword("_EMISSION");
        stoveLightMat.color = originalColor;

        switchedOnColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPoweredOn)
        {
            timeTillNextColorChange -= Time.deltaTime;
            kayaToastPrep.toastedTime += Time.deltaTime;

            if (ladenFood != null)
            {
                ladenFood.GetComponent<Food>().temperature += Time.deltaTime * heatingMultiplier;
            }

            if (timeTillNextColorChange <= 0)
            {
                timeTillNextColorChange = 6;
                ChangeLightColor();
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
        }
    }

    public void ToggleStovePower()
    {
        isPoweredOn = !isPoweredOn;

        if (isPoweredOn)
        {
            kayaToastPrep.isBeingToasted = true;
            if (ladenFood != null)
            {
                ladenFood.GetComponent<Food>().isBeingHeated = true;
            }

            stoveIndicatorLight.enabled = true;
            stoveLightMat.EnableKeyword("_EMISSION");
            UpdateColors();
        }

        else if (!isPoweredOn)
        {
            kayaToastPrep.isBeingToasted = false;
            if (ladenFood != null)
            {
                ladenFood.GetComponent<Food>().isBeingHeated = false;
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

        kayaToastPrep.isFlippedOnThisColor = false;
    }

    void UpdateColors()
    {
        stoveLightMat.color = switchedOnColor;
        stoveIndicatorLight.color = switchedOnColor;
        stoveLightMat.SetColor("_EmissionColor", switchedOnColor);
    }

    public void UpdateLadenFood(GameObject container)
    {
        if (container.GetComponent<Container>().itemContained != null && container.GetComponent<Container>().itemContained.GetComponent<Food>() != null)
        {
            ladenFood = container.GetComponent<Container>().itemContained;
        }
    }
}
