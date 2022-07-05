using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfBoiledEggsPrep : MonoBehaviour
{
    public bool isPotFilledWithWater = false;
    public bool isWaterBoiling = false;
    public bool areEggsBoiled = false;
    public bool areEggsCracked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFillingWater()
    {
        GameManagerScript.instance.orders.progressBar.ResetProgress();
        GameManagerScript.instance.orders.progressBar.SetMaxProgress(1000);
    }
}
