using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ending : MonoBehaviour
{
    public LevelLoader levelLoader;
    public int sceneIndex = 0;
    public List<Sprite> comicSprites = new List<Sprite>();
    public List<string> speakerNames = new List<string>();
    public List<string> comicTexts = new List<string>();
    public Image comicImage;

    public TMP_Text comicText;
    public TMP_Text speakerText;
    ScrollingText scrollingText;

    public GameObject finishPanel;
    // Start is called before the first frame update
    void Start()
    {
        scrollingText = comicText.GetComponent<ScrollingText>();
        comicImage.sprite = comicSprites[0];
        scrollingText.Show(comicTexts[0]);
        speakerText.text = speakerNames[0];

        GameManagerScript.instance.ChangeCursorLockedState(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Left click to progress to the next comic/dialogue
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            sceneIndex++;

            if (sceneIndex > comicTexts.Count - 1)
            {
                finishPanel.SetActive(true);
                return;
            }

            if (comicImage.sprite != comicSprites[sceneIndex])
            {
                comicImage.sprite = comicSprites[sceneIndex];
            }

            if (speakerText.text != speakerNames[sceneIndex])
            {
                speakerText.text = speakerNames[sceneIndex];
            }

            scrollingText.Stop();
            scrollingText.Show(comicTexts[sceneIndex]);
        }
    }

    public void ReturnToMenu()
    {
        levelLoader.LoadLevel(0);
    }
}
