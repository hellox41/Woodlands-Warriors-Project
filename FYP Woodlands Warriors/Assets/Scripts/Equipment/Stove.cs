using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    [Header("Stove Container")]
    public bool isLaden = false;

    public GameObject ladenItem;

    public Transform camTransitionTransform1;

    public Container stoveContainer;

    [Space]

    [Header("Cooking Functionality")]
    public bool isPoweredOn = false;

    [SerializeField] float stoveTimeElapsed;

    public Material stoveLightMat;

    public Light stoveIndicatorLight;

    [SerializeField] List<string> stoveLightColor = new List<string>();
    public string currentStoveLightColorName;
    [SerializeField] Color purpleColor;
    [SerializeField] Color pinkColor;
    [SerializeField] Color originalColor;

    [SerializeField] KayaToastPrep kayaToastPrep;

    // Start is called before the first frame update
    void Start()
    {
        stoveContainer = GetComponentInChildren<Container>();

        stoveIndicatorLight.enabled = false;
        stoveLightMat.DisableKeyword("_EMISSION");
        stoveLightMat.color = originalColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPoweredOn)
        {
            stoveTimeElapsed += Time.deltaTime;
            kayaToastPrep.toastedTime += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Container>() != null && !isLaden && other.gameObject.layer != LayerMask.NameToLayer("Holding"))
        {
            isLaden = true;
            ladenItem = other.gameObject;
            stoveContainer.isContainingItem = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == ladenItem && isLaden && other.gameObject.layer != LayerMask.NameToLayer("Holding"))
        {
            isLaden = false;
            ladenItem = null;
            stoveContainer.isContainingItem = false;
        }
    }

    public void ToggleStovePower()
    {
        isPoweredOn = !isPoweredOn;
        stoveLightMat.SetColor("_EmissionColor", Color.red);

        if (isPoweredOn)
        {
            kayaToastPrep.isBeingToasted = true;
            stoveIndicatorLight.enabled = true;
            stoveLightMat.EnableKeyword("_EMISSION");
            stoveIndicatorLight.color = purpleColor;
            stoveLightMat.color = purpleColor;
        }

        else if (!isPoweredOn)
        {
            kayaToastPrep.isBeingToasted = false;
            stoveIndicatorLight.enabled = false;
            stoveLightMat.DisableKeyword("_EMISSION");
            stoveLightMat.color = originalColor;
        }
    }

    void ChangeLightColor(string colorToChangeTo)
    {
        if (currentStoveLightColorName != colorToChangeTo)
        {
            currentStoveLightColorName = colorToChangeTo;

            if (currentStoveLightColorName == "Purple")
            {
                stoveIndicatorLight.color = purpleColor;
                stoveLightMat.color = purpleColor;
            }

        }
    }
}
