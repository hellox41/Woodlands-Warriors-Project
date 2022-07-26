using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    [Header("SFX")]
    public AudioClip mttExplosion;
    private AudioSource[] allAudioSources;

    [Header("Game Over")]
    public CanvasGroup persistentCanvas;
    public CanvasGroup gameOverCanvasGroup;
    public CanvasGroup panelCanvasGroup;
    public GameObject blackOverlay;

    public GameObject qualityBar;
    public GameObject timeBar;
    public GameObject timerPanel;

    public GameObject gameOverPanel;
    public TMP_Text gameOverReasonText;

    public GameObject quitInfoPanel;

    public ScrollingText scrollingText;

    private void Start()
    {
        scrollingText = gameOverReasonText.GetComponent<ScrollingText>();
    }

    public IEnumerator DisplayGameOver(string reasonForFail)
    {
        allAudioSources = FindObjectsOfType(typeof (AudioSource)) as AudioSource[];

        foreach (AudioSource audioSource in allAudioSources)
        {
            audioSource.Stop();
        }
        GameManagerScript.instance.isShowingGameOver = true;
        Time.timeScale = 0;
        blackOverlay.SetActive(true);
        gameOverCanvasGroup.alpha = 1;
        gameOverCanvasGroup.interactable = true;
        panelCanvasGroup.blocksRaycasts = true;
        foreach (Transform uiObj in persistentCanvas.transform)
        {
            uiObj.gameObject.SetActive(false);

            if (uiObj.name == "GameOverPanel")
            {
                uiObj.gameObject.SetActive(true);
                uiObj.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }    
        }

        panelCanvasGroup = GetComponent<CanvasGroup>();

        if (reasonForFail == "dishQuality")
        {
            StartCoroutine(BlinkObj(qualityBar));
            yield return new WaitForSecondsRealtime(1.5f);
            panelCanvasGroup.alpha = 1;
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.ignoreParentGroups = true;
            scrollingText.Show("The dish was unsalvageable.");
        }

        if (reasonForFail == "dishTime")
        {
            StartCoroutine(BlinkObj(timeBar));
            yield return new WaitForSecondsRealtime(1.5f);
            panelCanvasGroup.alpha = 1;
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.ignoreParentGroups = true;
            scrollingText.Show("The order was not completed on time.");
        }

        if (reasonForFail == "strikes")
        {
            GameManagerScript.instance.orders.sfxAudioSource.PlayOneShot(mttExplosion);
            StartCoroutine(BlinkObj(timerPanel));
            yield return new WaitForSecondsRealtime(1.5f);
            panelCanvasGroup.alpha = 1;
            panelCanvasGroup.interactable = true;
            panelCanvasGroup.ignoreParentGroups = true;
            scrollingText.Show("The Meow-ti Tool self-destructed.");
        }
    }

    IEnumerator BlinkObj(GameObject blinkingObj)
    {
        blinkingObj.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        blinkingObj.SetActive(false);
        yield return new WaitForSecondsRealtime(0.5f);
        blinkingObj.SetActive(true);
        yield return new WaitForSecondsRealtime(0.5f);
        blinkingObj.SetActive(false);
    }

    public void Retry()
    {
        GameManagerScript.instance.StopAllCoroutines();
        GameManagerScript.instance.levelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Quit()
    {
        quitInfoPanel.SetActive(true);
        Time.timeScale = 1;
    }

    public void CancelQuit()
    {
        quitInfoPanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void QuitToMenu()
    {
        GameManagerScript.instance.levelLoader.LoadLevel(0);
        Time.timeScale = 1;
    }

    public void QuitToDesktop()
    {
        Application.Quit();
        Time.timeScale = 1;
    }
}
