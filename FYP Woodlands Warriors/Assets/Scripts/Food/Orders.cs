using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Orders : MonoBehaviour
{
    public int ordersLeft;

    public string currentOrder;

    [Header("Current Order Values")]
    public int stagesToPrepare;

    public bool isPrepared = false;

    [Header("Food Scripts")]
    public KayaToastPrep kayaToastPrep;

    [Header("Final Food Gameobjects")]
    public GameObject multigrainKayaToast;
    public GameObject honeyOatKayaToast;
    public GameObject wholeWheatKayaToast;

    GameObject finalFoodShowcased;

    [Header("Food Instancing")]
    public Transform instancingSpawnPoint;
    public GameObject kayaToastObjects;

    [Header("Extra")]
    public ProgressBar progressBar;
    public Transform foodShowcaseTrans;
    [Range(0f, 1f)]
    [SerializeField] float camRotationSpeed;
    [SerializeField] Vector3 camOffset;
    [SerializeField] TMP_Text orderNameText;
    [SerializeField] TMP_Text orderInfoText;
    [SerializeField] GameObject orderUIGO;

    public RadialMenu radialMenu;

    // Start is called before the first frame update
    void Start()
    {
        orderUIGO.SetActive(false);
        CreateOrder();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManagerScript.instance.isShowcasing && !GameManagerScript.instance.isCamTransitioning)
        {
            Camera.main.transform.LookAt(finalFoodShowcased.transform);
            Camera.main.transform.Translate(Vector3.right * Time.fixedDeltaTime * camRotationSpeed);
        }
    }

    public void CreateOrder()
    {
        if (GameManagerScript.instance.levelNo == 1)
        {
            currentOrder = GameManagerScript.instance.orderTypes[Random.Range(0, 1)];
            Debug.Log(currentOrder + " order recieved!");
        }

        orderNameText.text = currentOrder;
        GenerateRandomIngredients();
    }

    void GenerateRandomIngredients()
    {
        if (currentOrder == "KAYATOAST")  //Random ingredient generation for kaya toast
        {
            kayaToastPrep.breadType = kayaToastPrep.breadTypes[Random.Range(0, kayaToastPrep.breadTypes.Length)];  //Random bread type

            if (kayaToastPrep.breadType == "MULTIGRAIN" || kayaToastPrep.breadType == "HONEYOAT")
            {
                kayaToastPrep.isBreadCut = true;
            }

            if (kayaToastPrep.breadType == "MULTIGRAIN")
            {
                orderInfoText.text = "Multigrain Bread";
            }

            if (kayaToastPrep.breadType == "WHOLEWHEAT")
            {
                orderInfoText.text = "Whole Wheat Bread";
            }

            if (kayaToastPrep.breadType == "HONEYOAT")
            {
                orderInfoText.text = "Honey Oat Bread";
            }
        }

        orderUIGO.SetActive(true);
    }

    public void CheckIfCooked()
    {
        //Checking for kayatoast
        if (currentOrder == "KAYATOAST")
        {
            if (kayaToastPrep.isBreadCut && kayaToastPrep.isBreadToasted && kayaToastPrep.isBreadSpreadKaya && kayaToastPrep.isBreadSpreadButter)
            {
                isPrepared = true;
                radialMenu.knifeStatusGO.SetActive(false);
            }
        }

        if (isPrepared == true)
        {
            Debug.Log("You successfully cooked a dish!");
            ShowcaseFinishedDish();
        }
    }

    void ShowcaseFinishedDish()
    {
        if (currentOrder == "KAYATOAST")
        {
            if (kayaToastPrep.breadType == "MULTIGRAIN")
            {
                finalFoodShowcased = Instantiate(multigrainKayaToast, foodShowcaseTrans.position, foodShowcaseTrans.rotation);
            }

            if (kayaToastPrep.breadType == "HONEYOAT")
            {
                finalFoodShowcased = Instantiate(honeyOatKayaToast, foodShowcaseTrans.position, foodShowcaseTrans.rotation);
            }

            if (kayaToastPrep.breadType == "WHOLEWHEAT")
            {
                finalFoodShowcased = Instantiate(wholeWheatKayaToast, foodShowcaseTrans.position, foodShowcaseTrans.rotation);
            }
        }

        Camera.main.transform.position = foodShowcaseTrans.position + camOffset;
        GameManagerScript.instance.isShowcasing = true;
    }

    public void ToggleOrderUI(bool shrunk)  //if param (bool shrunk) is true, expand the ui, else shrink the ui
    {
        OrderUI orderUI = orderUIGO.GetComponent<OrderUI>();
        if (shrunk)
        {
            orderUI.sizeToTransitionTo = new Vector3(1f, 1f, 1f);
            orderUI.posToTransitionTo = new Vector2(-335, 0);
        }

        else if (!shrunk)
        {
            orderUI.sizeToTransitionTo = new Vector3(0.4f, 0.4f, 0.4f);
            orderUI.posToTransitionTo = new Vector2(-400, 0);
        }

        orderUIGO.GetComponent<OrderUI>().isChangingSize = true;
    }
}
