using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelStats : MonoBehaviour
{
    public LevelLoader levelLoader;
    public TMP_Text levelLabel;
    public TMP_Text dishesLabelText;
    public TMP_Text dishesPreparedText;

    public TMP_Text time;
    public TMP_Text stars;

    public Image gradeImage;

    public GameObject nextLevelButton;

    [SerializeField] Sprite[] gradeSprites; // 0 = S, 1 = A, 2 = B, 3 = C, 4 = D, 5 = F

    public IEnumerator UpdateLevelStats()
    {
        yield return new WaitForSecondsRealtime(0.75f);
        levelLabel.text = GameManagerScript.instance.levelNo.ToString();
        levelLabel.gameObject.SetActive(true);
        
        for (int i = 0; i < GameManagerScript.instance.dishesPrepared.Count; i++)
        {
            if (i > 0)
            {
                dishesLabelText.text += "\n";
                dishesPreparedText.text += "\n";
            }

            yield return new WaitForSecondsRealtime(0.5f);
            dishesLabelText.text += GameManagerScript.instance.dishesPrepared[i];
            yield return new WaitForSecondsRealtime(0.5f);
            dishesPreparedText.text += GameManagerScript.instance.dishCount[i];
        }

        float minutes = Mathf.FloorToInt(GameManagerScript.instance.orders.stageTime / 60);
        float seconds = Mathf.FloorToInt(GameManagerScript.instance.orders.stageTime % 60);

        yield return new WaitForSecondsRealtime(1f);
        time.text = string.Format("{00}:{1:00}", minutes, seconds);
        time.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(1f);
        stars.text = GameManagerScript.instance.orders.totalStageStars.ToString();
        stars.gameObject.SetActive(true);

        //S grade (perfect)
        if (GameManagerScript.instance.orders.totalStageStars == GameManagerScript.instance.orders.ordersDone * 6)
        {
            gradeImage.sprite = gradeSprites[0];
        }

        //A grade
        else if (GameManagerScript.instance.orders.totalStageStars < GameManagerScript.instance.orders.ordersDone * 6 &&
            GameManagerScript.instance.orders.totalStageStars > GameManagerScript.instance.orders.ordersDone * 5)
        {
            gradeImage.sprite = gradeSprites[1];
        }

        //B grade
        else if (GameManagerScript.instance.orders.totalStageStars < GameManagerScript.instance.orders.ordersDone * 5 &&
    GameManagerScript.instance.orders.totalStageStars > GameManagerScript.instance.orders.ordersDone * 4)
        {
            gradeImage.sprite = gradeSprites[2];
        }

        //C grade
        else if (GameManagerScript.instance.orders.totalStageStars < GameManagerScript.instance.orders.ordersDone * 3 &&
    GameManagerScript.instance.orders.totalStageStars > GameManagerScript.instance.orders.ordersDone * 2)
        {
            gradeImage.sprite = gradeSprites[3];
        }

        //D grade
        else if (GameManagerScript.instance.orders.totalStageStars < GameManagerScript.instance.orders.ordersDone * 2 &&
    GameManagerScript.instance.orders.totalStageStars > GameManagerScript.instance.orders.ordersDone * 1.5f)
        {
            gradeImage.sprite = gradeSprites[4];
        }

        //F grade
        else if (GameManagerScript.instance.orders.totalStageStars < GameManagerScript.instance.orders.ordersDone * 1.5f)
        {
            gradeImage.sprite = gradeSprites[5];
        }

        yield return new WaitForSecondsRealtime(2f);
        gradeImage.enabled = true;

        yield return new WaitForSecondsRealtime(1f);
        nextLevelButton.SetActive(true);
    }

    public void NextLevel()
    {
        GameManagerScript.instance.levelNo++;
        if (GameManagerScript.instance.levelNo < 5)
        {
            levelLoader.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
            Time.timeScale = 1f;
        }
    }
}
