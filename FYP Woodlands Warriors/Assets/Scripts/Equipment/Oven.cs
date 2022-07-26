using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Oven : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip clickSfx;
    bool isCoverClosed = false;
    bool hasLockedIn = false;

    public Light ovenLight;

    public Animator coverAnimator;

    public bool hasStarted = false;

    Container container;

    [Header("Display")]
    public ProgressBar ovenProgressBar;
    public TMP_Text ovenDisplayText;

    public int displayNo;
    public int digitSelection = 1;

    public int thousands = 0;
    public int hundreds = 0;
    public int tens = 0;
    public int ones = 0;

    private void Start()
    {
        container = GetComponent<Container>();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isCoverClosed && GameManagerScript.instance.orders.satayPrep.isRackInOven)
        {
            ovenProgressBar.AddProgress(Time.deltaTime);
        }

        if (ovenProgressBar.slider.value >= ovenProgressBar.slider.maxValue && !hasLockedIn)
        {
            LockIn();
        }
    }

    public void ToggleCover()
    {
        if (!hasStarted)
        {
            ovenDisplayText.text = string.Format($"<color=#A7DF40FF>{thousands}</color>{hundreds}{tens}{ones}");
            hasStarted = true;
        }

        isCoverClosed = true;

        if (isCoverClosed)
        {
            coverAnimator.SetTrigger("Close");
            ovenLight.enabled = true;
            audioSource.Play();
        }    
    }

    public void CheckForRack()
    {
        if (container.itemContained.GetComponent<Interactable>().objectName == "metalRack")
        {
            GameManagerScript.instance.orders.satayPrep.isRackInOven = true;
        }
    }

    public void ChangeDigitSelection()
    {
        GameManagerScript.instance.orders.sfxAudioSource.PlayOneShot(clickSfx);
        digitSelection++;

        if (digitSelection > 4)
        {
            digitSelection = 1;
        }

        //Format the string to display each parsed variable number in their respective digit position. 
        if (digitSelection == 1)
        {
            ovenDisplayText.text = string.Format($"<color=#A7DF40FF>{thousands}</color>{hundreds}{tens}{ones}");
        }

        else if (digitSelection == 2)
        {
            ovenDisplayText.text = string.Format($"{thousands}<color=#A7DF40FF>{hundreds}</color>{tens}{ones}");
        }

        else if (digitSelection == 3)
        {
            ovenDisplayText.text = string.Format($"{thousands}{hundreds}<color=#A7DF40FF>{tens}</color>{ones}");
        }

        else if (digitSelection == 4)
        {
            ovenDisplayText.text = string.Format($"{thousands}{hundreds}{tens}<color=#A7DF40FF>{ones}</color>");
        }
    }

    public void ChangeDigitValue()
    {
        GameManagerScript.instance.orders.sfxAudioSource.PlayOneShot(clickSfx);
        //Change thousands digit
        if (digitSelection == 1)
        {
            thousands++;

            if (thousands > 9)
            {
                thousands = 0;
            }
            ovenDisplayText.text = string.Format($"<color=#A7DF40FF>{thousands}</color>{hundreds}{tens}{ones}");
        }

        //Change hundreds digit
        if (digitSelection == 2)
        {
            hundreds++;

            if (hundreds > 9)
            {
                hundreds = 0;
            }
            ovenDisplayText.text = string.Format($"{thousands}<color=#A7DF40FF>{hundreds}</color>{tens}{ones}");
        }

        //Change tens digit
        if (digitSelection == 3)
        {
            tens++;

            if (tens > 9)
            {
                tens = 0;
            }
            ovenDisplayText.text = string.Format($"{thousands}{hundreds}<color=#A7DF40FF>{tens}</color>{ones}");
        }

        //Change ones digit
        if (digitSelection == 4)
        {
            ones++;

            if (ones > 9)
            {
                ones = 0;
            }
            ovenDisplayText.text = string.Format($"{thousands}{hundreds}{tens}<color=#A7DF40FF>{ones}</color>");
        }

        displayNo = thousands * 1000 + hundreds * 100 + tens * 10 + ones;
    }

    void LockIn()
    {
        hasLockedIn = true;
        if ((GameManagerScript.instance.orders.satayPrep.meatType == "Beef" && displayNo == 0956) ||
            (GameManagerScript.instance.orders.satayPrep.meatType == "Chicken" && displayNo == 1777) ||
            (GameManagerScript.instance.orders.satayPrep.meatType == "Mutton" && displayNo == 0155))
        {
            GameManagerScript.instance.orders.satayPrep.areSkewersGrilled = true;
            GameManagerScript.instance.orders.CheckIfCooked();
        }

        else
        {
            GameManagerScript.instance.orders.dishQualityBar.AddProgress(-25f);
            ovenProgressBar.SetProgress(0);
            ovenProgressBar.UpdateProgress();
            ovenProgressBar.slider.maxValue -= 15;
            hasLockedIn = false;
        }
    }
}
