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
    public HalfBoiledEggsPrep halfBoiledEggsPrep;

    [Header("Final Food Gameobjects")]
    public GameObject multigrainKayaToast;
    public GameObject honeyOatKayaToast;
    public GameObject wholeWheatKayaToast;
    public GameObject halfBoiledEggs;

    GameObject finalFoodShowcased;

    [Header("Food Instancing")]
    public Transform instancingSpawnPoint;
    public GameObject kayaToastObjects;

    public GameObject brownEggs;
    public GameObject whiteEggs;

    [Header("Timer Values")]
    public Timer timer;
    public float stageTime;

    [Header("Extra")]
    public ProgressBar progressBar;
    public Transform foodShowcaseTrans;
    [Range(0f, 1f)]
    [SerializeField] float camRotationSpeed;
    public Vector3 camOffset;
    [SerializeField] TMP_Text orderNameText;
    [SerializeField] TMP_Text orderInfoText;
    [SerializeField] GameObject orderUIGO;
    [SerializeField] GameObject continueButton;
    GameObject spawnedPrep;
    public RadialMenu radialMenu;

    [Header("Debug")]
    public string orderTypeOverride;

    // Start is called before the first frame update
    void Start()
    {
        orderUIGO.SetActive(false);
        if (orderTypeOverride == null || orderTypeOverride == "")
        {
            CreateOrder();
        }

        else
        {
            CreateOrder(orderTypeOverride);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManagerScript.instance.isShowcasing && !GameManagerScript.instance.isCamTransitioning)
        {
            Camera.main.transform.LookAt(finalFoodShowcased.transform);
            Camera.main.transform.Translate(Vector3.right * Time.fixedDeltaTime * camRotationSpeed);
        }

        if (!GameManagerScript.instance.isShowcasing)
        {
            stageTime += Time.deltaTime;
            UpdateTimer();
        }
    }

    //If parameters are left blank, will generate a random order based on the level
    public void CreateOrder()
    {
        //If first stage, restrict possible dishes to first 2 dishes (kaya toast and eggs)
        if (GameManagerScript.instance.levelNo == 1)
        {
            currentOrder = GameManagerScript.instance.orderTypes[Random.Range(0, 2)];
            Debug.Log(currentOrder + " order recieved!");
        }

        orderNameText.text = currentOrder;
        GenerateRandomIngredients();
    }

    //If parameter is filled in, will generate that specific order
    void CreateOrder(string orderType)
    {
        currentOrder = orderType;
        Debug.Log(orderType + " order recieved!");

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

            spawnedPrep = Instantiate(kayaToastObjects, instancingSpawnPoint.position, instancingSpawnPoint.rotation);
            GameManagerScript.instance.playerControl.stove = GameObject.Find("Stove").GetComponent<Stove>();
        }

        if (currentOrder == "HALF-BOILEDEGGS")
        {
            halfBoiledEggsPrep.eggsType = halfBoiledEggsPrep.eggTypes[Random.Range(0, halfBoiledEggsPrep.eggTypes.Length)];

            if (halfBoiledEggsPrep.eggsType == "BROWN")
            {
                orderInfoText.text = "Brown Eggs";

            }

            if (halfBoiledEggsPrep.eggsType == "WHITE")
            {
                orderInfoText.text = "White Eggs";
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
                radialMenu.prepStatusGO.SetActive(false);
            }
        }

        if (currentOrder == "HALF-BOILEDEGGS")
        {
            if (halfBoiledEggsPrep.areEggsBoiled && halfBoiledEggsPrep.eggsStrained == 2)
            {
                isPrepared = true;
                radialMenu.prepStatusGO.SetActive(false);
            }
        }

        if (isPrepared == true)
        {
            Debug.Log("You successfully cooked a dish!");
            ShowcaseFinishedDish();
        }
    }

    //Instantiate the showcase and start the turnaround
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

            kayaToastPrep.ResetVariables();
        }

        if (currentOrder == "HALF-BOILEDEGGS")
        {
            finalFoodShowcased = Instantiate(halfBoiledEggs, foodShowcaseTrans.position, Quaternion.identity);
            halfBoiledEggsPrep.ResetVariables();
        }
        
        GameManagerScript.instance.isShowcasing = true;
        GameManagerScript.instance.ChangeCursorLockedState(false);
        continueButton.SetActive(true);
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

    void UpdateTimer()
    {
        float minutes = Mathf.FloorToInt(stageTime / 60);
        float seconds = Mathf.FloorToInt(stageTime % 60);
        timer.timerText.text = string.Format("{00}:{1:00}", minutes, seconds);
    }

    public void Continue()
    {
        GameManagerScript.instance.isShowcasing = false;
        Destroy(spawnedPrep);
        CreateOrder();
        continueButton.SetActive(false);
        Camera.main.GetComponent<CamTransition>().MoveCamera(Camera.main.GetComponent<CamTransition>().defaultCamTransform);
        GameManagerScript.instance.ChangeCursorLockedState(true);
    }
}
