using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeowtiTool : MonoBehaviour
{
    public string currentPawzzleType;

    public TMP_InputField primaryInput;

    public CanvasGroup huhCanvas;
    public CanvasGroup studioMixCanvas;
    public CanvasGroup launchCodesCanvas;
    public CanvasGroup activeCanvas;

    [Header("Huh? Configuration")]
    public TMP_Text huhText;
    string[] huhLeftDisplays = { "HUH", "AYCH YUU AYCH", "THE LETTERS HUH", "THE LETTERS AYCH YUU AYCH", "JUST THE LETTERS H U H NO SPACES WORD", 
        "THE LETTERS A Y C H SPACE Y U U SPACE A Y C H THREE WORDS TWO SPACES", "THE LETTERS A Y C H SPACE Y U U SPACE A Y C H WORD WORD WORD SPACE SPACE", 
        "THE WORD HUH 3 LETTERS", "THE WORD HUH LETTER LETTER LETTER", "I CAN'T SPELL IT OUT NO FUNNY STUFF", "A Y C H SPACE Y U U SPACE A Y C H", "THE WORDS AYCH YUU AYCH", 
        "THE LETTERS H U H FROM THE ALPHABET" };

    string[] huhRightDisplays = { "AGE YOU AGE", "THE LETTERS A G E SPACE Y O U SPACE A G E", "THE LETTERS H U H NO SPACES ONE WORD", "THE WORDS I CAN'T SPELL IT OUT ELEVEN WORDS IN TOTAL",
        "I CAN'T SPELL IT OUT", "NO IT LITERALLY SAYS I CAN'T SPELL IT OUT", "I CAN'T SPELL IT OUT WORDS ONLY", "I CAN'T SPELL OUT THE WORDS BUT THE DISPLAY IS HUH", "THE WORD HUH",
        "THE WORD HUH TREE LETTERS LIKE THE PLANT WORDS ONLY", "A G E SPACE Y O U SPACE A G E", "THE WORDS AGE YOU AGE", "AGE YOU AGE AS IN HOW OLD ARE YOU THAT AGE" };

    int[] huhLeftValues = { 62, 10, 43, 71, 100, 6, 21, 37, 50, 4, 14, 9, 92 };
    int[] huhRightValues = { 18, 0, 69, 85, 79, 47, 81, 64, 19, 46, 3, 5, 90 };

    public string textToDisplay;
    public int leftOrRightTable;
    public int huhValue;

    [Header("Apparatus Values")]
    public int knifeValue;

    // Start is called before the first frame update
    void Start()
    {
        huhCanvas.gameObject.SetActive(false);
        //studioMixCanvas.gameObject.SetActive(false);
        //launchCodesCanvas.gameObject.SetActive(false);

        //currentPawzzleType = GameManagerScript.instance.PrimaryPawzzles[Random.Range(0, 3)];

        knifeValue = 100;

        InitializePawzzle(currentPawzzleType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitializePawzzle(string PawzzleType)
    {
        if (PawzzleType == "HUH?")
        {
            InitializeHuh();
            huhCanvas.gameObject.SetActive(true);
            huhCanvas.interactable = true;
            activeCanvas = huhCanvas;
        }
    }

    void InitializeHuh()
    {
        //Pick from left or right table, this information is used in KNIFE pawzzle
        leftOrRightTable = Random.Range(0, 2);
        int rng = Random.Range(0, huhLeftDisplays.Length + 1);

        if (leftOrRightTable == 0)
        {
            textToDisplay = huhLeftDisplays[rng]; //Set the desired text to a random text on the left
            huhValue = huhLeftValues[rng];
        }

        else if(leftOrRightTable == 1)
        {
            textToDisplay = huhRightDisplays[rng]; //Set the desired text to a random text on the right
            huhValue = huhRightValues[rng];
        }

        huhText.text = textToDisplay;  //Set the desired text to the display text
    }

    public void SubmitPrimaryInput()
    {
        if (currentPawzzleType == "HUH?")
        {
            if (int.Parse(primaryInput.text) + huhValue == knifeValue)
            {
                Debug.Log("You got knife!");
            }
        }
    }
}
