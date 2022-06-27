using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bread : MonoBehaviour
{
    public string breadType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTooltipActionType()
    {
        GameManagerScript.instance.playerControl.raycastActionTooltipText.text = "Cut (Requires Knife)";
    }
}
