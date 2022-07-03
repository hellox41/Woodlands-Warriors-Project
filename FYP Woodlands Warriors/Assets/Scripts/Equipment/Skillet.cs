using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skillet : MonoBehaviour
{
    public void FlipBread()
    {
        if (GameManagerScript.instance.accessedApparatus != "SPATULA")
        {
            Debug.Log("You need the Spatula to flip the bread!");
        }

        else if (GameManagerScript.instance.accessedApparatus == "SPATULA" && GameManagerScript.instance.orders.kayaToastPrep.isFlippedOnThisColor)
        {
            Debug.Log("You've already flipped the bread on this colour!");
        }

        else if (GameManagerScript.instance.accessedApparatus == "SPATULA" && !GameManagerScript.instance.orders.kayaToastPrep.isFlippedOnThisColor)
        {
            if (GameManagerScript.instance.orders.kayaToastPrep.breadType == "MULTIGRAIN" && 
                GameManagerScript.instance.playerControl.stove.stoveLightMat.color == GameManagerScript.instance.playerControl.stove.purpleColor)
            {
                GameManagerScript.instance.orders.kayaToastPrep.timesFlippedCorrectly++;
                GameManagerScript.instance.orders.progressBar.AddProgress(1);
            }

            if (GameManagerScript.instance.orders.kayaToastPrep.breadType == "WHOLEWHEAT" && 
                GameManagerScript.instance.playerControl.stove.stoveLightMat.color == GameManagerScript.instance.playerControl.stove.pinkColor)
            {
                GameManagerScript.instance.orders.kayaToastPrep.timesFlippedCorrectly++;
                GameManagerScript.instance.orders.progressBar.AddProgress(1);
            }

            if (GameManagerScript.instance.orders.kayaToastPrep.breadType == "HONEYOAT" &&
                GameManagerScript.instance.playerControl.stove.stoveLightMat.color == Color.red)
            {
                GameManagerScript.instance.orders.kayaToastPrep.timesFlippedCorrectly++;
                GameManagerScript.instance.orders.progressBar.AddProgress(1);
            }

            if ((GameManagerScript.instance.orders.kayaToastPrep.breadType == "MULTIGRAIN" || GameManagerScript.instance.orders.kayaToastPrep.breadType == "HONEYOAT") &&
                GameManagerScript.instance.orders.kayaToastPrep.timesFlippedCorrectly == 2)
            {
                GameManagerScript.instance.orders.kayaToastPrep.isBreadToasted = true;
            }

            if (GameManagerScript.instance.orders.kayaToastPrep.breadType == "WHOLEWHEAT" && GameManagerScript.instance.orders.kayaToastPrep.timesFlippedCorrectly == 3)
            {
                GameManagerScript.instance.orders.kayaToastPrep.isBreadToasted = true;
            }

            GameManagerScript.instance.orders.kayaToastPrep.isFlippedOnThisColor = true;
        }
    }
}
