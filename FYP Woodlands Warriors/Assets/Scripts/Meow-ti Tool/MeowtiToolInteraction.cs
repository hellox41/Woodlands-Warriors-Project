using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeowtiToolInteraction : MonoBehaviour
{
    public MeowtiTool meowtiTool;

    public Transform raycastPos;

    public string currentApparatusType;
    public string currentActionType;

    public string currentCondiment;

    public Food desiredFood;
    public Condiment condiment;
    public Spread spread;
    public Orders orders;

    // Start is called before the first frame update
    void Start()
    {
        orders = GameObject.Find("StageHandler").GetComponent<Orders>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (currentActionType != null || currentActionType != "") && !meowtiTool.isTyping)  //Press E to use the meow-ti tool to prepare food
        {
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100f))
            {
                if (hit.transform.tag == "Food")
                {
                    desiredFood = hit.transform.GetComponent<Food>();

                    if (!orders.isPrepared)
                    {
                        PrepareFood();
                    }
                }

                if (hit.transform.tag == "Condiment")
                {
                    condiment = hit.transform.GetComponent<Condiment>();
                    currentCondiment = condiment.condimentType;
                }
            }
        }
    }

    public void PrepareFood()
    {
        if (desiredFood.foodType == "Bread")
        {
            KayaToastPrep kayaToastPrep = GameObject.Find("StageHandler").GetComponent<KayaToastPrep>();
            if (currentApparatusType == "KNIFE")
            {
                if (currentActionType == "CUT/SLICE" && !kayaToastPrep.isBreadCut)  //Cut the bread
                {
                    desiredFood.gameObject.transform.localScale -= new Vector3(2f, 2f, 2f);
                    kayaToastPrep.isBreadCut = true;
                }

                if (currentActionType == "SPREAD")  //Spreading method
                {
                    spread = kayaToastPrep.gameObject.GetComponentInChildren<Spread>();
                    if (!kayaToastPrep.isBreadSpreadKaya && !kayaToastPrep.isBreadSpreadButter)  //If the bread is plain
                    {
                        spread.mr.enabled = true;

                        if (currentCondiment == "Butter")  //Spreading butter on plain bread
                        {
                            spread.gameObject.SetActive(true);
                            spread.UpdateSpread("Butter");
                            kayaToastPrep.isBreadSpreadButter = true;
                        }

                        if (currentCondiment == "Kaya") //Spreading butter on plain bread
                        {
                            spread.gameObject.SetActive(true);
                            spread.UpdateSpread("Kaya");
                            kayaToastPrep.isBreadSpreadKaya = true;
                        }
                    }

                    if ((kayaToastPrep.isBreadSpreadButter && currentCondiment == "Kaya") || (kayaToastPrep.isBreadSpreadKaya && currentCondiment == "Butter")) //Combining spreads
                    {
                        spread.UpdateSpread("Kaya and Butter");
                        kayaToastPrep.isBreadSpreadButter = true;
                        kayaToastPrep.isBreadSpreadKaya = true;
                    }
                }
            }

            orders.CheckIfCooked();
        }
    }
}
