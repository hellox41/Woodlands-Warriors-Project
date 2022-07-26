using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PanelScript : MonoBehaviour
{
    public GameObject controlPanel;
    public GameObject urlButton;
    public LevelLoader levelLoader;
    public TMP_Text tutText;
    public Image panelImage;
    public List<Sprite> tutPanels = new List<Sprite>();
    public List<string> tutTexts = new List<string>();

    bool atLastPanel = false;

    public TMP_Text nextButtonText;

    Color32 originalTextCol;
    // Start is called before the first frame update
    void Start()
    {
        originalTextCol = nextButtonText.color;
        panelImage.sprite = tutPanels[0];
        panelImage.SetNativeSize();
        tutText.text = tutTexts[0];
    }

    public void NextButton()
    {
        if (tutPanels.IndexOf(panelImage.sprite) == tutPanels.Count - 3)
        {
            urlButton.SetActive(true);
        }

        else
        {
            urlButton.SetActive(false);
        }

        if (tutPanels.IndexOf(panelImage.sprite) == tutPanels.Count - 2)
        {
            panelImage.enabled = false;
            controlPanel.SetActive(true);
        }

        if (atLastPanel)
        {
            levelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }

        if (tutPanels.IndexOf(panelImage.sprite) < tutPanels.Count - 1)
        {
            panelImage.sprite = tutPanels[tutPanels.IndexOf(panelImage.sprite) + 1];
            panelImage.SetNativeSize();
            tutText.text = tutTexts[tutTexts.IndexOf(tutText.text) + 1];

            if (tutPanels.IndexOf(panelImage.sprite) == tutPanels.Count - 1)
            {
                atLastPanel = true;
                nextButtonText.text = "FINISH";
                nextButtonText.color = new Color32(255, 255, 255, 255);
            }
        }
    }

    public void PreviousButton()
    {
        if (atLastPanel)
        {
            panelImage.enabled = true;
            controlPanel.SetActive(false);
        }

        if (tutPanels.IndexOf(panelImage.sprite) > 0)
        {
            panelImage.sprite = tutPanels[tutPanels.IndexOf(panelImage.sprite) - 1];
            panelImage.SetNativeSize();
            tutText.text = tutTexts[tutTexts.IndexOf(tutText.text) - 1];

            if (tutPanels.IndexOf(panelImage.sprite) == tutPanels.Count - 2)
            {
                atLastPanel = false;
                nextButtonText.text = "NEXT";
                nextButtonText.color = originalTextCol;
            }

            if (tutPanels.IndexOf(panelImage.sprite) == tutPanels.Count - 2)
            {
                urlButton.SetActive(true);
            }

            else
            {
                urlButton.SetActive(false);
            }
        }
    }

    public void ButtonOpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
