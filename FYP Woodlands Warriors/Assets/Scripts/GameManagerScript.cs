using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript instance;

    public string[] PrimaryPawzzles = { "HUH?", "RADIOCOMMS", "LAUNCHCODES" };
    public string[] ApparatusPawzzles = { "KNIFE" };

    public bool isZoomed = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
