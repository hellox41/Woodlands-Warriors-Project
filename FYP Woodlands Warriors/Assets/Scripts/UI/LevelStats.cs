using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelStats : MonoBehaviour
{
    public TMP_Text levelLabel;
    public TMP_Text dishesPrepared;

    public TMP_Text timeAndStars;

    public Image gradeImage;

    [SerializeField] Sprite[] gradeSprites; // 0 = S, 1 = A, 2 = B, 3 = C, 4 = D, 5 = F

    public void UpdateLevelStats()
    {
        levelLabel.text = "Day " + GameManagerScript.instance.levelNo + " Complete!";
        dishesPrepared.text = "Kaya Toast: " + GameManagerScript.instance.orders.kayaToastPrep.preparedCount +
            "\nHalf-boiled Eggs: " + GameManagerScript.instance.orders.halfBoiledEggsPrep.preparedCount;

        float minutes = Mathf.FloorToInt(GameManagerScript.instance.orders.stageTime / 60);
        float seconds = Mathf.FloorToInt(GameManagerScript.instance.orders.stageTime % 60);

        timeAndStars.text = "Total Time: " + string.Format("{00}:{1:00}", minutes, seconds) +
            "\nTotal Stars: " + GameManagerScript.instance.orders.totalStageStars;

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
    }

    public void NextLevel()
    {
        Debug.Log("Changing to next scene");
    }
}
