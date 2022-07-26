using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingBar : MonoBehaviour
{
    public ProgressBar progressBar;
    string prepType;

    // Start is called before the first frame update
    void Start()
    {
        progressBar = GetComponent<ProgressBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar.progress > 0 && !GameManagerScript.instance.orders.satayPrep.isMixGrinded && GameManagerScript.instance.isPreparing)
        {
            progressBar.AddProgress(-Time.deltaTime);
        }

        if (progressBar.progress >= progressBar.slider.maxValue)
        {
            if (prepType != GameManagerScript.instance.radialMenu.prepType)
            {
                prepType = GameManagerScript.instance.radialMenu.prepType;
            }

            //Grinding in mortar for Satay
            if (prepType == "Grinding Ingredients")
            {
                GameManagerScript.instance.orders.satayPrep.isMixGrinded = true;
                GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);

                GameManagerScript.instance.orders.satayPrep.mixingBar.SetActive(false);
            }

            //Mixing in mixing bowl for nasilemak
            else if (prepType == "Mixing Ingredients")
            {
                if (!GameManagerScript.instance.orders.nasiLemakPrep.isMarinateMixed)
                {
                    GameManagerScript.instance.orders.nasiLemakPrep.isMarinateMixed = true;
                    GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
                }

                GameManagerScript.instance.orders.nasiLemakPrep.mixingBar.SetActive(false);
            }

            //Grinding sambal mix in mortar for nasilemak
            else if (prepType == "Grinding Sambal Ingredients")
            {
                if (!GameManagerScript.instance.orders.nasiLemakPrep.isSambalGrinded)
                {
                    GameManagerScript.instance.orders.nasiLemakPrep.isSambalGrinded = true;
                    GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
                }

                GameManagerScript.instance.orders.nasiLemakPrep.mixingBar.SetActive(false);
            }
            Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
        }
    }
}
