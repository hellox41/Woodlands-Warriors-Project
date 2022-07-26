using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//Don't touch if u dunno JSON
public class ComicHandler : MonoBehaviour
{
    public TMP_Text comicText;
    public TMP_Text speakerText;

    public List<Sprite> comicSprites = new List<Sprite>();
    public Image comicSprite;

    ScrollingText scrollingText;

    int currentJsonTextIndex = 0;

    public LevelLoader levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        scrollingText = comicText.GetComponent<ScrollingText>();
        comicSprite.sprite = comicSprites[0];
        UpdateTextJson(currentJsonTextIndex);  
    }

    // Update is called once per frame
    void Update()
    {
        //Left click to progress to the next comic/dialogue
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            currentJsonTextIndex++;

            scrollingText.Stop();
            UpdateTextJson(currentJsonTextIndex);
        }
    }

    [Serializable]
    public class TextClass
    {
        public string comicTextString;
        public string speakerTextString;
        public int panelIndex;

        public void ClearText()
        {
            comicTextString = null;
        }
    }

    public void UpdateTextJson(int jsonIndex)
    {
        string json = File.ReadAllText(Application.streamingAssetsPath + @"\comicJson.json");
        TextClass[] textClasses = JsonHelper.FromJson<TextClass>(json);

        //If reached the end of the comics (from JSON file)
        if (currentJsonTextIndex > textClasses.Length - 1)
        {
            //Go to level1
            levelLoader.LoadLevel(2);
            GameManagerScript.instance.ChangeCursorLockedState(false);
        }

        if (textClasses[jsonIndex].panelIndex == 12)
        {
            levelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (textClasses[jsonIndex].panelIndex < 12)
        {
            scrollingText.Show(textClasses[jsonIndex].comicTextString);
            speakerText.text = textClasses[jsonIndex].speakerTextString;
        }

        if (comicSprites.IndexOf(comicSprite.sprite) != textClasses[jsonIndex].panelIndex)
        {
            comicSprite.sprite = comicSprites[textClasses[jsonIndex].panelIndex];
        }
    }
}


//Copy-and-pasted from StackOverflow, basically allows the array in the JSON file to be read by Unity
[Serializable]
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
