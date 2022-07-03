using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condiment : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyKaya()
    {
        GameManagerScript.instance.orders.kayaToastPrep.isKayaAppliedToKnife = true;
        GameManagerScript.instance.orders.kayaToastPrep.condimentText.text = "Knife Condiment: Kaya";
    }

    public void ApplyButter()
    {
        GameManagerScript.instance.orders.kayaToastPrep.isButterAppliedToKnife = true;
        GameManagerScript.instance.orders.kayaToastPrep.condimentText.text = "Knife Condiment: Butter";
    }
}
