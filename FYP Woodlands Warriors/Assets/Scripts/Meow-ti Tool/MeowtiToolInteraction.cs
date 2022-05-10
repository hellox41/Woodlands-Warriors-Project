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

    // Start is called before the first frame update
    void Start()
    {
        
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

                    if (!desiredFood.isPrepared)
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

    void PrepareFood()
    {
        if (desiredFood.foodType == "Bread")
        {
            if (currentApparatusType == "KNIFE")
            {
                if (currentActionType == "CUT/SLICE" && !desiredFood.isBreadCut)  //Cut the bread
                {
                    desiredFood.gameObject.transform.localScale -= new Vector3(2f, 2f, 2f);
                    desiredFood.isBreadCut = true;
                }

                if (currentActionType == "SPREAD")  //Spreading method
                {
                    spread = desiredFood.gameObject.GetComponentInChildren<Spread>();
                    if (!desiredFood.isBreadSpreadKaya && !desiredFood.isBreadSpreadButter)  //If the bread is plain
                    {
                        spread.mr.enabled = true;

                        if (currentCondiment == "Butter")  //Spreading butter on plain bread
                        {
                            spread.gameObject.SetActive(true);
                            spread.UpdateSpread("Butter");
                            desiredFood.isBreadSpreadButter = true;
                        }

                        if (currentCondiment == "Kaya") //Spreading butter on plain bread
                        {
                            spread.gameObject.SetActive(true);
                            spread.UpdateSpread("Kaya");
                            desiredFood.isBreadSpreadKaya = true;
                        }
                    }

                    if ((desiredFood.isBreadSpreadButter && currentCondiment == "Kaya") || (desiredFood.isBreadSpreadKaya && currentCondiment == "Butter")) //Combining spreads
                    {
                        spread.UpdateSpread("Kaya and Butter");
                        desiredFood.isBreadSpreadButter = true;
                        desiredFood.isBreadSpreadKaya = true;
                    }
                }
            }

            desiredFood.CheckIfCooked();
        }
    }
}
