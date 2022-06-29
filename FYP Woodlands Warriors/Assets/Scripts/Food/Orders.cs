using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
    public int ordersLeft;

    public string currentOrder;

    [Header("Current Order Values")]
    public int stagesToPrepare;

    public bool isPrepared = false;

    [Header("Foods")]
    public KayaToastPrep kayaToastPrep;

    [Header("Extra")]
    public ProgressBar progressBar;

    // Start is called before the first frame update
    void Start()
    {
        CreateOrder();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateOrder()
    {
        if (GameManagerScript.instance.levelNo == 1)
        {
            currentOrder = GameManagerScript.instance.orderTypes[Random.Range(0, 1)];
            Debug.Log(currentOrder + " order recieved!");
        }

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
        }
    }

    public void CheckIfCooked()
    {
        //Checking for kayatoast
        if (currentOrder == "KAYATOAST")
        {
            if (kayaToastPrep.isBreadCut && kayaToastPrep.isBreadToasted && kayaToastPrep.isBreadSpreadKaya && kayaToastPrep.isBreadSpreadButter)
            {
                isPrepared = true;
            }
        }

        if (isPrepared == true)
        {
            Debug.Log("You successfully cooked a dish!");
        }
    }
}
