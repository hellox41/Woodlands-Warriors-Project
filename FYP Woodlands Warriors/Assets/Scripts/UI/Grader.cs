using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Grader : MonoBehaviour
{
    public TMP_Text showcaseNameText;
    public TMP_Text gradingText;

    [SerializeField] int timeStars;
    [SerializeField] int qualityStars;

    [SerializeField] int quality3StarThreshold;
    [SerializeField] int quality2StarThreshold;
    [SerializeField] int quality1StarThreshold;

    string finalDishRating;

    public void UpdateText()
    {
        GradeQuality();
        GradeTime();

        GradeRating();

        if (GameManagerScript.instance.orders.currentOrder == "KAYATOAST")
        {
            showcaseNameText.text = "Kaya Toast";
        }

        if (GameManagerScript.instance.orders.currentOrder == "HALF-BOILEDEGGS")
        {
            showcaseNameText.text = "Half-boiled Eggs";
        }

        if (GameManagerScript.instance.orders.currentOrder == "SATAY")
        {
            showcaseNameText.text = "Satay";
        }

        if (GameManagerScript.instance.orders.currentOrder == "NASILEMAK")
        {
            showcaseNameText.text = "Nasi Lemak";
        }

        gradingText.text = "Time: " + timeStars + " Stars" + "\n" + 
            "Quality: " + qualityStars + " Stars" + "\n \n" +
            "Rating: " + finalDishRating;

        GameManagerScript.instance.orders.totalStageStars += qualityStars;
        GameManagerScript.instance.orders.totalStageStars += timeStars;
    }

    void GradeQuality()
    {
        //Quality >= 90
        if (GameManagerScript.instance.orders.dishQualityBar.slider.value >= quality3StarThreshold)
        {
            qualityStars = 3;
        }

        //Quality 50 > x > 90
        else if (GameManagerScript.instance.orders.dishQualityBar.slider.value >= quality2StarThreshold && GameManagerScript.instance.orders.dishQualityBar.slider.value < quality3StarThreshold)
        {
            qualityStars = 2;
        }

        //Quality 30 > x > 50
        else if (GameManagerScript.instance.orders.dishQualityBar.slider.value < quality2StarThreshold && GameManagerScript.instance.orders.dishQualityBar.slider.value > quality1StarThreshold)
        {
            qualityStars = 1;
        }

        //Quality < 30
        else if (GameManagerScript.instance.orders.dishQualityBar.slider.value < quality1StarThreshold)
        {
            qualityStars = 0;
        }
    }

    void GradeTime()
    {
        if (GameManagerScript.instance.orders.currentOrder == "KAYATOAST")
        {
            if (GameManagerScript.instance.orders.dishTime <= GameManagerScript.instance.orders.kayaToastPrep.dishTimes[0])
            {
                timeStars = 3;
            }

            if (GameManagerScript.instance.orders.dishTime <= GameManagerScript.instance.orders.kayaToastPrep.dishTimes[1] &&
                GameManagerScript.instance.orders.dishTime > GameManagerScript.instance.orders.kayaToastPrep.dishTimes[0])
            {
                timeStars = 2;
            }

            if (GameManagerScript.instance.orders.dishTime <= GameManagerScript.instance.orders.kayaToastPrep.dishTimes[2] &&
                GameManagerScript.instance.orders.dishTime > GameManagerScript.instance.orders.kayaToastPrep.dishTimes[1])
            {
                timeStars = 1;
            }
        }

        if (GameManagerScript.instance.orders.currentOrder == "HALF-BOILEDEGGS")
        {
            if (GameManagerScript.instance.orders.dishTime <= GameManagerScript.instance.orders.halfBoiledEggsPrep.dishTimes[0])
            {
                timeStars = 3;
            }

            if (GameManagerScript.instance.orders.dishTime <= GameManagerScript.instance.orders.halfBoiledEggsPrep.dishTimes[1] &&
                GameManagerScript.instance.orders.dishTime > GameManagerScript.instance.orders.halfBoiledEggsPrep.dishTimes[0])
            {
                timeStars = 2;
            }

            if (GameManagerScript.instance.orders.dishTime <= GameManagerScript.instance.orders.halfBoiledEggsPrep.dishTimes[2] &&
                GameManagerScript.instance.orders.dishTime > GameManagerScript.instance.orders.halfBoiledEggsPrep.dishTimes[1])
            {
                timeStars = 1;
            }
        }

        if (GameManagerScript.instance.orders.currentOrder == "SATAY")
        {
            if (GameManagerScript.instance.orders.dishTime <= GameManagerScript.instance.orders.satayPrep.dishTimes[0])
            {
                timeStars = 3;
            }

            if (GameManagerScript.instance.orders.dishTime <= GameManagerScript.instance.orders.satayPrep.dishTimes[1] &&
                GameManagerScript.instance.orders.dishTime > GameManagerScript.instance.orders.satayPrep.dishTimes[0])
            {
                timeStars = 2;
            }

            if (GameManagerScript.instance.orders.dishTime <= GameManagerScript.instance.orders.satayPrep.dishTimes[2] &&
                GameManagerScript.instance.orders.dishTime > GameManagerScript.instance.orders.satayPrep.dishTimes[1])
            {
                timeStars = 1;
            }
        }
    }

    void GradeRating()
    {
        int totalStars = timeStars + qualityStars;

        //Purr-fect! (6 stars)
        if (totalStars == 6)
        {
            finalDishRating = "Purrfect!";
        }

        //Paw-some! (4 or 5 stars)
        if (totalStars < 6 && totalStars > 3)
        {
            finalDishRating = "Paw-some!";
        }

        //OK... (2 or 3 stars)
        if (totalStars > 1 && totalStars <= 3)
        {
            finalDishRating = "OK...";
        }

        //Cat-astrophic! (1 or 0 stars)
        if (totalStars <= 1)
        {
            finalDishRating = "Cat-astrophic!";
        }
    }
}
