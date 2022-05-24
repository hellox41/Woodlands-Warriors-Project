using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
    public int ordersLeft;

    public string currentOrder;

    [Header("Current Order Values")]
    public int stagesToPrepare;
    public int currentStage = 1;

    public bool isPrepared = false;

    [Header("Foods")]
    public KayaToastPrep kayaToastPrep;

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
            currentOrder = GameManagerScript.instance.orders[Random.Range(0, 1)];
            Debug.Log(currentOrder + " order recieved!");
        }

        GenerateRandomIngredients();
        currentStage = 1;
    }

    void GenerateRandomIngredients()
    {
        if (currentOrder == "KAYATOAST")  //Random ingredient generation for kaya toast
        {
            kayaToastPrep.breadType = kayaToastPrep.breadTypes[Random.Range(0, kayaToastPrep.breadTypes.Length)];  //Random bread type
        }
    }

    public void CheckIfCooked()
    {
        if (currentStage == stagesToPrepare)
        {
            isPrepared = true;
        }
    }
}
