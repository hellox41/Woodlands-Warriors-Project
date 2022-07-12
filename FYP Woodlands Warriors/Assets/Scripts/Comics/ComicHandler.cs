using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComicHandler : MonoBehaviour
{
    public TMP_Text comicText;
    public TMP_Text speakerText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTextJson(1);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Serializable]
    public class TextClass
    {
        public string comicTextString;
        public string speakerTextString;
    }

    public void UpdateTextJson(int jsonIndex)
    {
        string json = File.ReadAllText(Application.dataPath + "/ComicText/comicJson.json");
        TextClass[] textClasses = JsonHelper.FromJson<TextClass>(json);

        comicText.text = textClasses[jsonIndex].comicTextString;
        speakerText.text = textClasses[jsonIndex].speakerTextString;
    }
}


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
