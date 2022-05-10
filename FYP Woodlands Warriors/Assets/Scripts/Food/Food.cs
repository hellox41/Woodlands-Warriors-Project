using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    //Possible food types: Bread, Butter, Kaya
    public string foodType;

    public bool isPrepared = false;

    [Header("Bread Values")]
    public bool isBreadCut = false;
    public bool isBreadToasted = false;
    public bool isBreadSpreadButter = false;
    public bool isBreadSpreadKaya = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckIfCooked()
    {
        if (foodType == "Bread")
        {
            if (isBreadCut && isBreadToasted && isBreadSpreadButter && isBreadSpreadKaya)
            {
                isPrepared = true;
            }
        }
    }
}
