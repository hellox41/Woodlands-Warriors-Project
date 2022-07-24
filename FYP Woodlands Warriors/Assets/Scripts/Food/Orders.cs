using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Orders handles order generation, grading, prep object instantiation, and cooking.
public class Orders : MonoBehaviour
{
    public int ordersDone;

    public string currentOrder;

    public ReceiptNouns receiptNouns;

    [Header("Current Order Values")]
    public int stagesToPrepare;

    [Header("Food Scripts")]
    public KayaToastPrep kayaToastPrep;
    public HalfBoiledEggsPrep halfBoiledEggsPrep;
    public SatayPrep satayPrep;
    public NasiLemakPrep nasiLemakPrep;

    [Header("Final Food Gameobjects")]
    public GameObject multigrainKayaToast;
    public GameObject honeyOatKayaToast;
    public GameObject wholeWheatKayaToast;

    public GameObject halfBoiledEggs;

    public GameObject beefSatay;
    public GameObject chickenSatay;
    public GameObject muttonSatay;

    public GameObject nasiLemak;

    GameObject finalFoodShowcased;

    [Header("Food Instancing")]
    public Transform instancingSpawnPoint;
    public GameObject kayaToastObjects;
    public GameObject halfBoiledEggsObjects;
    public GameObject satayObjects;
    public GameObject nasiLemakObjects;

    public GameObject eggs;
    public Material whiteEggMat;
    public Material brownEggMat;

    public GameObject beefCubes;
    public GameObject chickenCubes;
    public GameObject muttonCubes;

    [Header("Timer Values")]
    public float stageTime;
    public float dishTime;
    public TMP_Text stageTimeText;
    public TMP_Text timeGradingText;
    public bool isShowing3Star = true;
    public bool isShowing2Star = false;
    public bool isShowing1Star = false;
    public bool isShowingLatest = false;

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

    [Header("Audio")]
    public AudioSource sfxAudioSource;
    public AudioSource musicAudioSource;

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
    public GameOver gameOver;


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

        else if (currentOrder == "SATAY")
        {
            timeProgressBar.SetMaxProgress(satayPrep.dishTimes[3]);
        }

        else if (currentOrder == "NASILEMAK")
        {
            timeProgressBar.SetMaxProgress(nasiLemakPrep.dishTimes[3]);
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

        //Checking for failure
        if (!GameManagerScript.instance.isShowingGameOver)
        {
            if (currentOrder == "KAYATOAST" && dishTime > kayaToastPrep.dishTimes[3])
            {
                StartCoroutine(gameOver.DisplayGameOver("dishTime")); ;
            }

            else if (currentOrder == "HALF-BOILEDEGGS" && dishTime > halfBoiledEggsPrep.dishTimes[3])
            {
                StartCoroutine(gameOver.DisplayGameOver("dishTime"));
            }

            else if (currentOrder == "SATAY" && dishTime > satayPrep.dishTimes[3])
            {
                StartCoroutine(gameOver.DisplayGameOver("dishTime"));
            }

            else if (currentOrder == "NASILEMAK" && dishTime > nasiLemakPrep.dishTimes[3])
            {
                StartCoroutine(gameOver.DisplayGameOver("dishTime"));
            }

            if (dishQualityBar.slider.value <= 0)
            {
                StartCoroutine(gameOver.DisplayGameOver("dishQuality"));
            }
        }
    }

    //If parameters are left blank, will generate a random order based on the level
    public void CreateOrder()
    {
        //If first stage, restrict possible dishes to first 2 dishes (kaya toast and eggs)
        if (GameManagerScript.instance.levelNo == 1)
        {
            currentOrder = GameManagerScript.instance.orderTypes[Random.Range(0, 2)];
        }

        //If second stage, restrict possible dishes to eggs and satay
        if (GameManagerScript.instance.levelNo == 2)
        {
            currentOrder = GameManagerScript.instance.orderTypes[Random.Range(1, 3)];

        }

        //If third stage, restrict possible dishes to satay and nasilemak
        if (GameManagerScript.instance.levelNo == 3)
        {
            currentOrder = GameManagerScript.instance.orderTypes[Random.Range(2, 4)];

        }

        Debug.Log(currentOrder + " order recieved!");
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

        if (currentOrder == "SATAY")
        {
            satayPrep.meatType = satayPrep.meatTypes[Random.Range(0, satayPrep.meatTypes.Length)];

            if (satayPrep.meatType == "Beef")
            {
                orderInfoText.text = "Beef Cubes";
            }

            if (satayPrep.meatType == "Chicken")
            {
                orderInfoText.text = "Chicken Cubes";
            }

            if (satayPrep.meatType == "Mutton")
            {
                orderInfoText.text = "Mutton Cubes";
            }

            spawnedPrep = Instantiate(satayObjects, instancingSpawnPoint.position, instancingSpawnPoint.rotation);          
        }

        if (currentOrder == "NASILEMAK")
        {
            nasiLemakPrep.riceType = nasiLemakPrep.riceTypes[Random.Range(0, nasiLemakPrep.riceTypes.Length)];

            orderInfoText.text = nasiLemakPrep.riceType + " Rice";

            spawnedPrep = Instantiate(nasiLemakObjects, instancingSpawnPoint.position, Quaternion.identity);
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
                kayaToastPrep.preparedCount++;
                radialMenu.prepStatusGO.SetActive(false);
                ShowcaseFinishedDish();

                if (!GameManagerScript.instance.dishesPrepared.Contains("Kaya Toast: "))
                {
                    GameManagerScript.instance.dishesPrepared.Add("Kaya Toast: ");
                    GameManagerScript.instance.dishCount.Add(1);
                }

                else if (GameManagerScript.instance.dishesPrepared.Contains("Kaya Toast: "))
                {
                    GameManagerScript.instance.dishCount[GameManagerScript.instance.dishesPrepared.IndexOf("Kaya Toast: ")]++;
                }
            }
        }

        //Checking for half-boiled eggs
        if (currentOrder == "HALF-BOILEDEGGS")
        {
            if (halfBoiledEggsPrep.areEggsBoiled && halfBoiledEggsPrep.eggsStrained == 2)
            {
                halfBoiledEggsPrep.preparedCount++;
                radialMenu.prepStatusGO.SetActive(false);
                ShowcaseFinishedDish();

                if (!GameManagerScript.instance.dishesPrepared.Contains("Half-boiled Eggs: "))
                {
                    GameManagerScript.instance.dishesPrepared.Add("Half-boiled Eggs: ");
                    GameManagerScript.instance.dishCount.Add(1);
                }

                else if (GameManagerScript.instance.dishesPrepared.Contains("Half-boiled Eggs: "))
                {
                    GameManagerScript.instance.dishCount[GameManagerScript.instance.dishesPrepared.IndexOf("Half-boiled Eggs: ")]++;
                }
            }
        }

        //Checking for satay
        if (currentOrder == "SATAY")
        {
            if (satayPrep.areSkewersGrilled)
            {
                satayPrep.preparedCount++;
                radialMenu.prepStatusGO.SetActive(false);
                ShowcaseFinishedDish();

                if (!GameManagerScript.instance.dishesPrepared.Contains("Satay: "))
                {
                    GameManagerScript.instance.dishesPrepared.Add("Satay: ");
                    GameManagerScript.instance.dishCount.Add(1);
                }

                else if (GameManagerScript.instance.dishesPrepared.Contains("Satay: "))
                {
                    GameManagerScript.instance.dishCount[GameManagerScript.instance.dishesPrepared.IndexOf("Satay: ")]++;
                }
            }
        }

        //Checking for nasilemak
        if (currentOrder == "NASILEMAK")
        {
            if (nasiLemakPrep.isRiceScooped && nasiLemakPrep.isChickenFried && nasiLemakPrep.isSambalFried)
            {
                nasiLemakPrep.preparedCount++;
                radialMenu.prepStatusGO.SetActive(false);
                ShowcaseFinishedDish();

                if (!GameManagerScript.instance.dishesPrepared.Contains("Nasi Lemak: "))
                {
                    GameManagerScript.instance.dishesPrepared.Add("Nasi Lemak: ");
                    GameManagerScript.instance.dishCount.Add(1);
                }

                else if (GameManagerScript.instance.dishesPrepared.Contains("Nasi Lemak: "))
                {
                    GameManagerScript.instance.dishCount[GameManagerScript.instance.dishesPrepared.IndexOf("Nasi Lemak: ")]++;
                }
            }
        }
    }

    //Instantiate the showcase and start the turnaround
    void ShowcaseFinishedDish()
    {
        Debug.Log("You completed a dish!");
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

        if (currentOrder == "SATAY")
        {
            if (satayPrep.meatType == "Beef")
            {
                finalFoodShowcased = Instantiate(beefSatay, foodShowcaseTrans.position, foodShowcaseTrans.rotation);
            }

            if (satayPrep.meatType == "Chicken")
            {
                finalFoodShowcased = Instantiate(chickenSatay, foodShowcaseTrans.position, foodShowcaseTrans.rotation);
            }

            if (satayPrep.meatType == "Mutton")
            {
                finalFoodShowcased = Instantiate(muttonSatay, foodShowcaseTrans.position, foodShowcaseTrans.rotation);
            }
            satayPrep.ResetVariables();
        }

        if (currentOrder == "NASILEMAK")
        {
            finalFoodShowcased = Instantiate(nasiLemak, foodShowcaseTrans.position, Quaternion.identity);
            nasiLemakPrep.ResetVariables();
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
            orderUI.posToTransitionTo = new Vector2(180, 540);
        }

        else if (!shrunk)
        {
            orderUI.sizeToTransitionTo = new Vector3(0.4f, 0.4f, 0.4f);
            orderUI.posToTransitionTo = new Vector2(0, 0);
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
                    timeGradingText.color = twoStarColor - new Color32(100, 100, 100, 0);
                    timeGradingFill.color = twoStarColor;
                }
            }

            else if (currentOrder == "HALF-BOILEDEGGS")
            {
                timeGradingText.text = (halfBoiledEggsPrep.dishTimes[0] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > halfBoiledEggsPrep.dishTimes[0])
                {
                    isShowing3Star = false;
                    isShowing2Star = true;
                    timeGradingText.color = twoStarColor - new Color32(100, 100, 100, 0);
                    timeGradingFill.color = twoStarColor;
                }
            }

            else if (currentOrder == "SATAY")
            {
                timeGradingText.text = (satayPrep.dishTimes[0] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > satayPrep.dishTimes[0])
                {
                    isShowing3Star = false;
                    isShowing2Star = true;
                    timeGradingText.color = twoStarColor - new Color32(100, 100, 100, 0);
                    timeGradingFill.color = twoStarColor;
                }
            }

            else if (currentOrder == "NASILEMAK")
            {
                timeGradingText.text = (nasiLemakPrep.dishTimes[0] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > nasiLemakPrep.dishTimes[0])
                {
                    isShowing3Star = false;
                    isShowing2Star = true;
                    timeGradingText.color = twoStarColor - new Color32(100, 100, 100, 0);
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
                    timeGradingText.color = oneStarColor - new Color32(100, 100, 100, 0);
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
                    timeGradingText.color = oneStarColor - new Color32(100, 100, 100, 0);
                    timeGradingFill.color = oneStarColor;
                }
            }

            else if (currentOrder == "SATAY")
            {
                timeGradingText.text = (satayPrep.dishTimes[1] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > satayPrep.dishTimes[1])
                {
                    isShowing2Star = false;
                    isShowing1Star = true;
                    timeGradingText.color = oneStarColor - new Color32(100, 100, 100, 0);
                    timeGradingFill.color = oneStarColor;
                }
            }

            else if (currentOrder == "NASILEMAK")
            {
                timeGradingText.text = (nasiLemakPrep.dishTimes[1] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > nasiLemakPrep.dishTimes[1])
                {
                    isShowing2Star = false;
                    isShowing1Star = true;
                    timeGradingText.color = oneStarColor - new Color32(100, 100, 100, 0);
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
                    timeGradingText.color = latestColor - new Color32(100, 100, 100, 0);
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
                    timeGradingText.color = latestColor - new Color32(100, 100, 100, 0);
                    timeGradingFill.color = latestColor;
                }
            }

            else if (currentOrder == "SATAY")
            {
                timeGradingText.text = (satayPrep.dishTimes[2] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > satayPrep.dishTimes[2])
                {
                    isShowing1Star = false;
                    isShowingLatest = true;
                    timeGradingText.color = latestColor - new Color32(100, 100, 100, 0);
                    timeGradingFill.color = latestColor;
                }
            }


            else if (currentOrder == "NASILEMAK")
            {
                timeGradingText.text = (nasiLemakPrep.dishTimes[2] - Mathf.FloorToInt(dishTime)).ToString();

                if (dishTime > nasiLemakPrep.dishTimes[2])
                {
                    isShowing1Star = false;
                    isShowingLatest = true;
                    timeGradingText.color = latestColor - new Color32(100, 100, 100, 0);
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

            if (currentOrder == "SATAY")
            {
                timeGradingText.text = (satayPrep.dishTimes[3] - Mathf.FloorToInt(dishTime)).ToString();
            }

            if (currentOrder == "NASILEMAK")
            {
                timeGradingText.text = (nasiLemakPrep.dishTimes[3] - Mathf.FloorToInt(dishTime)).ToString();
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
            levelStats.gameObject.SetActive(true);
            StartCoroutine(levelStats.UpdateLevelStats());
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            StartCoroutine(levelStats.UpdateLevelStats());
        }
    }
}
