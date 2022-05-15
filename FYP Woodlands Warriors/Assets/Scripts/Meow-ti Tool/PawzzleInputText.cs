using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PawzzleInputText : MonoBehaviour
{
    public TMP_InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnValueChanged()
    {
        var upperText = inputField.text.ToUpper();
        if (upperText != inputField.text)
        {
            inputField.text = upperText;
        }
    }
}
