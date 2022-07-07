using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidHolder : MonoBehaviour
{
    public float capacity;
    public float currentLevel;
    public float fullLevelY;

    public bool isContainingLiquid = false;
    public bool isBoilable = false;
    public bool isBoiling = false;

    public string liquidType;

    public GameObject liquidGO;

    public Transform dropPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        liquidGO.transform.localPosition = new Vector3(0f, currentLevel / capacity * fullLevelY, 0f);
    }

    public void DropItem(GameObject objToDrop)
    {
        objToDrop.transform.position = dropPoint.position;
        objToDrop.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void StrainEggs()
    {
        if (GameManagerScript.instance.accessedApparatus == "STRAINER" && GameManagerScript.instance.orders.halfBoiledEggsPrep.eggsInsidePot > 0)
        {
            //use Regex.Replace() to get only the digits in the string of text for the timer
            string timeDigits = Regex.Replace(GameManagerScript.instance.orders.timer.timerText.text, "[^0-9]", "");

            if ((GameManagerScript.instance.orders.halfBoiledEggsPrep.eggsType == "BROWN" && CheckStrainingDigit(timeDigits, 5)) ||
                (GameManagerScript.instance.orders.halfBoiledEggsPrep.eggsType == "WHITE" && CheckStrainingDigit(timeDigits, 8)))
            {

            }

            else
            {
                Debug.Log("You removed the eggs at the incorrect time!");
            }

            GameManagerScript.instance.orders.halfBoiledEggsPrep.eggsInsidePot--;
            GameManagerScript.instance.orders.halfBoiledEggsPrep.eggsStrained++;
            GameManagerScript.instance.orders.progressBar.AddProgress(1);

            GameManagerScript.instance.orders.CheckIfCooked();
        }
    }

    bool CheckStrainingDigit(string timeDigits, int digitToCheck)
    {
        return timeDigits.Contains(digitToCheck.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Egg")
        {
            GameManagerScript.instance.orders.halfBoiledEggsPrep.eggsInsidePot++;
            if (GameManagerScript.instance.orders.halfBoiledEggsPrep.eggsInsidePot == 2)
            {
                GameManagerScript.instance.orders.halfBoiledEggsPrep.areEggsBoiling = true;
                GameManagerScript.instance.orders.progressBar.AddProgress(1);
            }
        }
    }
}
