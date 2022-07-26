using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condiment : MonoBehaviour
{
    public void ApplyKaya()
    {
        GameManagerScript.instance.orders.kayaToastPrep.isKayaAppliedToKnife = true;
        GameManagerScript.instance.prepStatusText.text = "Knife Condiment: Kaya";
        GameManagerScript.instance.orders.sfxAudioSource.PlayOneShot(GetComponent<AudioClip>());
    }

    public void ApplyButter()
    {
        GameManagerScript.instance.orders.kayaToastPrep.isButterAppliedToKnife = true;
        GameManagerScript.instance.prepStatusText.text = "Knife Condiment: Butter";
        GameManagerScript.instance.orders.sfxAudioSource.PlayOneShot(GetComponent<AudioClip>());
    }
}
