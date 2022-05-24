using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    public string[] PrimaryPawzzles = { "18CARROT", "RADIOCAFE", "LAUNCHCODES" };
    public string[] ApparatusPawzzles = { "KNIFE" };

    public string[] orders = { "KAYATOAST", "HALF-BOILEDEGGS" };

    public string accessedApparatus;

    public int levelNo;
    public int ordersLeft;
    public int pawzzleDifficulty;

    public bool isZoomed = false;
    public bool isInteracting = false;

    public Food interactedFood;

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

        if (levelNo == 1)
        {
            ordersLeft = 3;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
