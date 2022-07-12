using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Orders : MonoBehaviour
{
    public int ordersDone;

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
    public GameObject halfBoiledEggsObjects;

    public GameObject eggs;
    public Material whiteEggMat;
    public Material brownEggMat;

    [Header("Timer Values")]
    public float stageTime;
    public float dishTime;
    public TMP_Text stageTimeText;
    public TMP_Text timeGradingText;
    bool isShowing3Star = true;
    bool isShowing2Star = false;
    bool isShowing1Star = false;
    bool isShowingLatest = false;

    public Image timeGradingFill;

    [SerializeField] Color threeStarColor;
    [SerializeField] Color twoStarColor;
    [SerializeField] Color oneStarColor;
    [SerializeField] Color latestColor;

    [Header("Grading")]
    public ProgressBar timeProgressBar;
    public GameObject showcaseUI;
    public Grader grader;
    public GameObject uiCanvas;
    public int totalStageStars = 0;
    public LevelStats levelStats;

    [Header("Extra")]
    public ProgressBar prepProgressBar;
    public ProgressBar dishQualityBar;
    public Transform foodShowcaseTrans;
    [Range(0f, 1f)]
    [SerializeField] float camRotationSpeed;
    public Vector3 camOffset;
    [SerializeField] TMP_Text orderNameText;
    [SerializeField] TMP_Text orderInfoText;
    [SerializeField] TMP_Text showcaseNameText;
    [SerializeField] TMP_Text showcaseGradingText;
    [SerializeField] GameObject orderUIGO;
    [SerializeField] GameObject continueButton;
    GameObject spawnedPrep;
    public RadialMenu radialMenu;
    [SerializeField] bool isAtShowcaseTrans = false;
    public GameObject stageRatingGO;

    [Header("Debug")]
    public string orderTypeOverride;

    // Start is called before the first frame update
    void Start()
    {
        orderUIGO.SetActive(false);
        ordersDone = 0;
        if (orderTypeOverride == null || orderTypeOverride == "")
        {
            CreateOrder();
        }

        else
        {
            CreateOrder(orderTypeOverride);
            Debug.Log(currentOrder + " order created through debug settings");
        }

        if (currentOrder == "KAYATOAST")
        {
            timeProgressBar.SetMaxProgress(kayaToastPrep.dishTimes[3]);
        }

        else if (currentOrder == "HALF-BOILEDEGGS")
        {
            timeProgressBar.SetMaxProgress(halfBoiledEggsPrep.dishTimes[3]);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManagerScript.instance.isShowcasing && !GameManagerScript.instance.isCamTransitioning)
        {
            if (!isAtShowcaseTrans)
            {
                Camera.main.transform.position = foodShowcaseTrans.position + camOffset;
                isAtShowcaseTrans = true;
            }
            Camera.main.transform.LookAt(finalFoodShowcased.transform);
            Camera.main.transform.Translate(Vector3.right * Time.fixedDeltaTime * camRotationSpeed);
        }

        if (!GameManagerScript.instance.isShowcasing)
        {
            stageTime += Time.deltaTime;
            UpdateTimer();
        }

        timeProgressBar.SetProgress(timeProgressBar.slider.maxValue - dishTime);
        timeProgressBar.UpdateProgress();
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
            spawnedPrep = Instantiate(halfBoiledEggsObjects, instancingSpawnPoint.position, instancingSpawnPoint.rotation);
            eggs = GameObject.Find("Eggs");
            halfBoiledEggsPrep.eggsType = halfBoiledEggsPrep.eggTypes[Random.Range(0, halfBoiledEggsPrep.eggTypes.Length)];
            List<MeshRenderer> mrs = new List<MeshRenderer>();
            foreach (Transform child in eggs.transform)
            {
                mrs.Add(child.GetComponent<MeshRenderer>());
            }

            if (halfBoiledEggsPrep.eggsType == "BROWN")
            {
                orderInfoText.text = "Brown Eggs";

                foreach(MeshRenderer mr in mrs)
                {
                    mr.material = brownEggMat;
                }
            }

            if (halfBoiledEggsPrep.eggsType == "WHITE")
            {
                orderInfoText.text = "White Eggs";

                foreach (MeshRenderer mr in mrs)
                {
                    mr.material = whiteEggMat;
                }
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
                kayaToastPrep.preparedCount++;
                radialMenu.prepStatusGO.SetActive(false);
            }
        }

        if (currentOrder == "HALF-BOILEDEGGS")
        {
            if (halfBoiledEggsPrep.areEggsBoiled && halfBoiledEggsPrep.eggsStrained == 2)
            {
                isPrepared = true;
                halfBoiledEggsPrep.preparedCount++;
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

        Camera.main.transform.DetachChildren();
        GameManagerScript.instance.isShowcasing = true;
        GameManagerScript.instance.ChangeCursorLockedState(false);
        uiCanvas.SetActive(false);
        showcaseUI.SetActive(true);
        grader.UpdateText();
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

    //Updates both stage timer and the grading timer's value and color
    void UpdateTimer()
    {
        //For main timer
        float minutes = Mathf.FloorToInt(stageTime / 60);
        float seconds = Mathf.FloorToInt(stageTime % 60);
        stageTimeText.text = string.Format("{00}:{1:00}", minutes, seconds);
        dishTime += Time.fixedDeltaTime;

        //For grading timer
        //Show time for 3 star
        if (isShowing3Star)
        {
            if (currentOrder == "KAYATOAST")
            {
                timeGradingText.text = (kayaToastPrep.dishTimes[0] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > kayaToastPrep.dishTimes[0])
                {
                    isShowing3Star = false;
                    isShowing2Star = true;
                    timeGradingText.color = twoStarColor;
                    timeGradingFill.color = twoStarColor;
                }
            }

            if (currentOrder == "HALF-BOILEDEGGS")
            {
                timeGradingText.text = (halfBoiledEggsPrep.dishTimes[0] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > halfBoiledEggsPrep.dishTimes[0])
                {
                    isShowing3Star = false;
                    isShowing2Star = true;
                    timeGradingText.color = twoStarColor;
                    timeGradingFill.color = twoStarColor;
                }
            }
        }

        //Show time for 2 star
        if (isShowing2Star)
        {
            if (currentOrder == "KAYATOAST")
            {
                timeGradingText.text = (kayaToastPrep.dishTimes[1] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > kayaToastPrep.dishTimes[1])
                {
                    isShowing2Star = false;
                    isShowing1Star = true;
                    timeGradingText.color = oneStarColor;
                    timeGradingFill.color = oneStarColor;
                }
            }

            if (currentOrder == "HALF-BOILEDEGGS")
            {
                timeGradingText.text = (halfBoiledEggsPrep.dishTimes[1] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > halfBoiledEggsPrep.dishTimes[1])
                {
                    isShowing2Star = false;
                    isShowing1Star = true;
                    timeGradingText.color = oneStarColor;
                    timeGradingFill.color = oneStarColor;
                }
            }
        }

        //Show time for 1 star
        if (isShowing1Star)
        {
            if (currentOrder == "KAYATOAST")
            {
                timeGradingText.text = (kayaToastPrep.dishTimes[2] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > kayaToastPrep.dishTimes[2])
                {
                    isShowing1Star = false;
                    isShowingLatest = true;
                    timeGradingText.color = latestColor;
                    timeGradingFill.color = latestColor;
                }
            }

            if (currentOrder == "HALF-BOILEDEGGS")
            {
                timeGradingText.text = (halfBoiledEggsPrep.dishTimes[2] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > halfBoiledEggsPrep.dishTimes[2])
                {
                    isShowing1Star = false;
                    isShowingLatest = true;
                    timeGradingText.color = latestColor;
                    timeGradingFill.color = latestColor;
                }
            }
        }

        //Show time for latest time possible
        if (isShowingLatest)
        {
            if (currentOrder == "KAYATOAST")
            {
                timeGradingText.text = (kayaToastPrep.dishTimes[3] - Mathf.FloorToInt(dishTime)).ToString();
            }

            if (currentOrder == "HALF-BOILEDEGGS")
            {
                timeGradingText.text = (halfBoiledEggsPrep.dishTimes[3] - Mathf.FloorToInt(dishTime)).ToString();
            }
        }
    }

    /*Create new order, along with the order's prep objects, destroy showcased food, hide the showcase UI, show the normalUI, move camera back to 1st person view, lock the cursor,
    and reset the dish time*/
    public void Continue()
    {
        ordersDone++;

        if (ordersDone == 3)
        {
            Time.timeScale = 0f;
            levelStats.UpdateLevelStats();
            levelStats.gameObject.SetActive(true);
            return;
        }

        GameManagerScript.instance.isShowcasing = false;
        Destroy(spawnedPrep);
        Destroy(finalFoodShowcased);
        CreateOrder();
        showcaseUI.SetActive(false);
        uiCanvas.SetActive(true);
        Camera.main.GetComponent<CamTransition>().MoveCamera(Camera.main.GetComponent<CamTransition>().defaultCamTransform);
        GameManagerScript.instance.ChangeCursorLockedState(true);
        isAtShowcaseTrans = false;

        dishTime = 0;
        GameManagerScript.instance.orders.dishQualityBar.SetProgress(100);
        GameManagerScript.instance.orders.dishQualityBar.UpdateProgress();
        timeGradingText.color = threeStarColor;
        timeGradingFill.color = threeStarColor;
    }
}
