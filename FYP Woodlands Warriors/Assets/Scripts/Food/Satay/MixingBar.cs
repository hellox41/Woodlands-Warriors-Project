using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingBar : MonoBehaviour
{
    public ProgressBar progressBar;
    // Start is called before the first frame update
    void Start()
    {
        progressBar = GetComponent<ProgressBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (progressBar.progress > 0 && !GameManagerScript.instance.orders.satayPrep.isMixGrinded)
        {
            progressBar.AddProgress(-Time.deltaTime);
        }

        if (progressBar.progress >= progressBar.slider.maxValue && !GameManagerScript.instance.orders.satayPrep.isMixGrinded)
        {
            GameManagerScript.instance.orders.satayPrep.isMixGrinded = true;
            GameManagerScript.instance.orders.satayPrep.mixingBar.SetActive(false);
            Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
            GameManagerScript.instance.orders.prepProgressBar.AddProgress(1);
        }
    }
}
