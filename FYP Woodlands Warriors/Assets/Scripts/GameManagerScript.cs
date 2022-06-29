using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    public string[] PrimaryPawzzles = { "18CARROT", "RADIOCAFE", "VITAMINS" };

    public string[] orderTypes = { "KAYATOAST", "HALF-BOILEDEGGS" };

    public string accessedApparatus = null;

    public int levelNo;
    public int ordersLeft;
    public int pawzzleDifficulty;
    public int strikes = 0;

    public float roomTemperature = 28f;

    public bool isZoomed = false;
    public bool isInteracting = false;
    public bool isPreparing = false;
    public bool isPlaceable = false;
    public bool isTransferable = false;

    public Food interactedFood;

    public PlayerControl playerControl;

    public Orders orders;

    public GameObject interactedItem;

    public Container container;

    public Inventory playerInventory;
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
        accessedApparatus = null;

        if (levelNo == 1)
        {
            ordersLeft = 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ErrorMade()
    {
        strikes++;

        Debug.Log("A mistake was made!");
        if (strikes == 3)
        {
            Debug.Log("You failed the level!");
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
}
