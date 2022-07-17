using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalRack : MonoBehaviour
{
    Container container;

    public void CheckSkewers()
    {
        GameManagerScript.instance.orders.satayPrep.areSkewersOnRack = true;
    }
}
