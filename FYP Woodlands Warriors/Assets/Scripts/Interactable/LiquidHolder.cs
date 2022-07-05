using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidHolder : MonoBehaviour
{
    public float capacity;
    public float currentLevel;
    public float fullLevelY;

    public bool isContainingLiquid = false;
    public bool isBoilable = false;
    public bool isBoiling = false;

    public string liquidType;

    public GameObject liquidGO;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        liquidGO.transform.localPosition = new Vector3(0f, currentLevel / capacity * fullLevelY, 0f);
    }
}
