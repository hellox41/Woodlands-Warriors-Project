using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    public float scrollSpeed;

    public string currentText;

    public TMP_InputField text;

    public IEnumerator ShowText()  //wipes text and displays text at a rate of 10 words per second
    {
        text.text = "";
        foreach (char c in currentText.ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(scrollSpeed);
        }
    }
}
