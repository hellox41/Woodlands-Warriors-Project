using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScrollingText : MonoBehaviour
{
    public float scrollSpeed;  //Smaller the scroll speed, the faster.

    private string currentText;
    public string textToShow;

    public TMP_Text text;

    public void Show(string text)
    {
        currentText = text;
        StartCoroutine(ShowText());
    }

    public void Stop()
    {
        StopAllCoroutines();
    }

    private IEnumerator ShowText()  //wipes text and displays text at a rate of 10 words per second
    {
        text.text = "";
        foreach (char c in currentText.ToCharArray())
        {
            text.text += c;
            yield return new WaitForSeconds(scrollSpeed);
        }
    }
}
