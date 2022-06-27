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

    public void AddProgress(int progress)
    {
        slider.value += progress;
    }

    public void ResetProgress()
    {
        slider.value = 0;
    }
}
