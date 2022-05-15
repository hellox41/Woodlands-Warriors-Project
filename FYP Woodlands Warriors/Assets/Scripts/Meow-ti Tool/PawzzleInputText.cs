using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PawzzleInputText : MonoBehaviour
{
    public TMP_InputField inputField;

    public void OnValueChanged()
    {
        var upperText = inputField.text.ToUpper();
        if (upperText != inputField.text)
        {
            inputField.text = upperText;
        }
    }
}
