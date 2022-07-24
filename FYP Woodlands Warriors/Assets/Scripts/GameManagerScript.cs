using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//Singleton script that handles level data, time, pawzzle types, order types, and strikes. Also stores public instance variables for scripts to communicate with (e.g. interactedItem from orders)
public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    public LevelLoader levelLoader;

    public List<string> dishesPrepared = new List<string>();
    public List<int> dishCount = new List<int>();

    public string[] PrimaryPawzzles = { "F5", "18CARROT", "RADIOCAFE", "VITAMINS", "HEALTHYPLATE" };

    public string[] orderTypes = { "KAYATOAST", "HALF-BOILEDEGGS" };

    public string[] possibleApparatusNouns = { "MONKEY", "CARROT", "GRANDMA", "CHICKEN", "BANANA", "POTATO", "SOTONG" };
    public List<string> apparatusNouns = new List<string>();
    public string knifeNoun;   //index 0
    public string spatulaNoun; //index 1
    public string strainerNoun; //index 2
    public string pestleNoun; //index 3
    public string paddleNoun; //index 4

    public Image[] strikeImages;

    public string accessedApparatus = null;

    public int levelNo;
    public int pawzzleDifficulty;
    public int strikes = 0;
    public int tapTurnedCount = 0;

    public float roomTemperature = 28f;

    public bool inLevel = false;
    public bool isZoomed = false;
    public bool isInteracting = false;
    public bool isPreparing = false;
    public bool isPlaceable = false;
    public bool isTransferable = false;
    public bool isShowcasing = false;
    public bool isCamTransitioning = false;
    public bool isOrderUIShrunk = false;
    bool isFlashing = false;
    public bool isShowingGameOver = false;

    public Food interactedFood;

    public PlayerControl playerControl;

    public Orders orders;

    public GameObject interactedItem;

    public Container container;

    public Inventory playerInventory;

    public TMP_Text prepStatusText;

    public RadialMenu radialMenu;

    public Material waterMat;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        pawzzleDifficulty = levelNo;
        waterMat.color = new Color32(170, 243, 250, 127);
    }

    //Assign variables on level load
    private void OnLevelWasLoaded(int level)
    {
        pawzzleDifficulty = levelNo;
        orders = GameObject.Find("StageHandler").GetComponent<Orders>();
        GameObject player = GameObject.Find("Player");
        playerControl = player.GetComponent<PlayerControl>();
        playerInventory = player.GetComponent<Inventory>();
        radialMenu = playerControl.radialMenu;
        prepStatusText = radialMenu.prepTypeText;
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

        strikeImages[0] = GameObject.Find("strikeImage1").GetComponent<Image>();
        strikeImages[1] = GameObject.Find("strikeImage2").GetComponent<Image>();

        ResetVariablesOnLoad();

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            levelNo = 1;
            pawzzleDifficulty = 1;
        }

        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            levelNo = 2;
            pawzzleDifficulty = 2;
        }

        else if (SceneManager.GetActiveScene().name == "Level3")
        {
            levelNo = 3;
            pawzzleDifficulty = 3;
        }
    }   

    private void Update()
    {
        if (inLevel)
        {
            if (orders.currentOrder == "KAYATOAST" && orders.dishTime > orders.kayaToastPrep.dishTimes[3])
            {
                StartCoroutine(orders.gameOver.DisplayGameOver("dishTime"));
            }

            else if (orders.currentOrder == "HALF-BOILEDEGGS" && orders.dishTime > orders.halfBoiledEggsPrep.dishTimes[3])
            {
                StartCoroutine(orders.gameOver.DisplayGameOver("dishTime"));
            }

            else if (orders.currentOrder == "SATAY" && orders.dishTime > orders.satayPrep.dishTimes[3])
            {
                StartCoroutine(orders.gameOver.DisplayGameOver("dishTime"));
            }

            else if (orders.currentOrder == "NASILEMAK" && orders.dishTime > orders.nasiLemakPrep.dishTimes[3])
            {
                StartCoroutine(orders.gameOver.DisplayGameOver("dishTime"));
            }
        }

        /*if (Input.GetKeyDown(KeyCode.F12))
        {
            FailLevel();
        }*/
    }

    public void ErrorMade()
    {
        strikes++;

        if (strikes == 3)
        {
            StartCoroutine(orders.gameOver.DisplayGameOver("strikes"));
        }

        if (isFlashing)
        {
            StopCoroutine(FlashStrike());
            strikeImages[strikes - 2].color = Color.red;
            isFlashing = false;
        }

        if (!isShowingGameOver)
        {
            StartCoroutine(FlashStrike());
        }


        Debug.Log("A mistake was made!");
    }

    public void ChangeCursorLockedState(bool lockedState)  //if lockedState = true, lock cursor, otherwise unlock cursor
    {
        if (lockedState == true)  //lock cursor
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        else if (lockedState == false)  //unlock cursor
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    IEnumerator FlashStrike()
    {
        isFlashing = true;
        Color tmpRed = strikeImages[strikes - 1].color;
        tmpRed = Color.red;
        strikeImages[strikes - 1].color = tmpRed;

        yield return new WaitForSeconds(0.5f);

        tmpRed.a = 0.3f;
        strikeImages[strikes - 1].color = tmpRed;

        yield return new WaitForSeconds(0.5f);

        tmpRed.a = 1f;
        strikeImages[strikes - 1].color = tmpRed;

        yield return new WaitForSeconds(0.5f);

        tmpRed.a = 0.3f;
        strikeImages[strikes - 1].color = tmpRed;

        yield return new WaitForSeconds(0.5f);

        tmpRed.a = 1f;
        strikeImages[strikes - 1].color = tmpRed;
        isFlashing = false;
    }

    void ResetVariablesOnLoad()
    {
        strikes = 0;
        isZoomed = false;
        isInteracting = false;
        isPreparing = false;
        isCamTransitioning = false;
        isOrderUIShrunk = false;
        waterMat.color = new Color32(170, 243, 250, 127);
        dishesPrepared.Clear();
        dishCount.Clear();
    }
}
