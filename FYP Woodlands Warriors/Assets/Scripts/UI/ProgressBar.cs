﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    public float progress;

    public void SetMaxProgress(int capacity)
    {
        slider.maxValue = capacity;
    }

    public void AddProgress(float progressToAdd)
    {
        /*if (slider.value < slider.maxValue)
        {
            slider.value += progress;

            if (slider.value == slider.maxValue)
            {
                Camera.main.GetComponent<CamTransition>().MoveCamera(GameManagerScript.instance.playerControl.raycastPointTransform);
            }
        }*/

        progress += progressToAdd;
        UpdateProgress();
    }

    public void UpdateProgress()
    {
        slider.value = progress;
    }

    public void SetProgress(float progressToSet)
    {
        progress = progressToSet;
    }

    public void ResetProgress()
    {
        progress = 0;
    }

    public void ResetValue()
    {
        slider.value = 0;
    }
}
