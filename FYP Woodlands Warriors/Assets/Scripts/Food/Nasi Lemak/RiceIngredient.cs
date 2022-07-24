using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceIngredient : MonoBehaviour
{
    public GameObject potPandan;
    public Material waterMat;

    public void AddIngredient(string ingredientType)
    {
        if (ingredientType == "pandan")
        {
            potPandan.SetActive(true);
        }

        else if (ingredientType == "coconutMilk")
        {
            waterMat.color = new Color32(230, 243, 250, 127);
        }

        gameObject.SetActive(false);
    }
}
