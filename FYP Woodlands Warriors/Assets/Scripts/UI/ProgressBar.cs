using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    private void Start()
    {
        slider.value = 0;
    }

    public void SetMaxProgress(int capacity)
    {
        slider.maxValue = capacity;
    }

    public void AddProgress(float progress)
    {
        if (slider.value < slider.maxValue)
        {
            slider.value += progress;

            if (slider.value == slider.maxValue)
            {
                Camera.main.GetComponent<CamTransition>().MoveCamera(GameManagerScript.instance.playerControl.raycastPointTransform);
            }
        }
    }

    public void ResetProgress()
    {
        slider.value = 0;
    }
}
