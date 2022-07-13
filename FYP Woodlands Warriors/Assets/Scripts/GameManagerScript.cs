using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    public string[] PrimaryPawzzles = { "F5", "18CARROT", "RADIOCAFE", "VITAMINS" };

    public string[] orderTypes = { "KAYATOAST", "HALF-BOILEDEGGS" };

    public Image[] strikeImages;

    public string accessedApparatus = null;

    public int levelNo;
    public int pawzzleDifficulty;
    public int strikes = 0;

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

    public Food interactedFood;

    public PlayerControl playerControl;

    public Orders orders;

    public GameObject interactedItem;

    public Container container;

    public Inventory playerInventory;

    public TMP_Text prepStatusText;

    public RadialMenu radialMenu;
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
    }

    //Assign variables on level load
    private void OnLevelWasLoaded(int level)
    {
        orders = GameObject.Find("StageHandler").GetComponent<Orders>();
        GameObject player = GameObject.Find("Player");
        playerControl = player.GetComponent<PlayerControl>();
        playerInventory = player.GetComponent<Inventory>();
        radialMenu = playerControl.radialMenu;
        prepStatusText = radialMenu.prepTypeText;

        strikeImages[0] = GameObject.Find("strikeImage1").GetComponent<Image>();
        strikeImages[1] = GameObject.Find("strikeImage2").GetComponent<Image>();

        if (level == 2)
        {
            levelNo = 1;
            pawzzleDifficulty = 1;
        }
    }   

    private void Update()
    {
        if (inLevel)
        {
            if (orders.currentOrder == "KAYATOAST" && orders.dishTime > orders.kayaToastPrep.dishTimes[3])
            {
                FailLevel();
            }

            else if (orders.currentOrder == "HALF-BOILEDEGGS" && orders.dishTime > orders.halfBoiledEggsPrep.dishTimes[3])
            {
                FailLevel();
            }
        }

        if (Input.GetKeyDown(KeyCode.F12))
        {
            FailLevel();
        }
    }

    public void ErrorMade()
    {
        strikes++;

        if (isFlashing)
        {
            StopCoroutine(FlashStrike());
            strikeImages[strikes - 2].color = Color.red;
            isFlashing = false;
        }

        StartCoroutine(FlashStrike());

        Debug.Log("A mistake was made!");

        if (strikes == 3)
        {
            FailLevel();
        }
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

    public void FailLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
