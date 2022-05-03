﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeowtiTool : MonoBehaviour
{
    public string currentPawzzleType;

    public TMP_InputField primaryInput;

    public CanvasGroup huhCanvas;
    public CanvasGroup radioCommsCanvas;
    public CanvasGroup launchCodesCanvas;
    public CanvasGroup activeCanvas;

    public AudioSource toolAudioSource;

    [Header ("Debug")]
    public string pawzzleTypeOverride;

    [Header ("Apparatus Values")]
    public int knifeValue;

    [Header ("Huh? Configuration")]
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

    [Header ("Radio Comms")]
    string[] radioCommsWords = { "BEATS", "BELFRY", "BELLS", "BENIGN", "COMIC", "COMMIE", "COMMIT", "CULLS", "CURED", "CURRY", "RUINED", "RUSHES", "RUSSET", "RUSSIA", "RUSTIC", "RUSTY" };
    int[] radioCommsFrequencies = { 619, 261, 173, 506, 092, 452, 369, 992, 142, 748, 338, 410, 028, 645, 100 };
    public AudioClip[] radioAudioClips;

    public string radioWord;
    public int radioFrequency;
    public AudioClip radioAudioClipToPlay;
    public int audioPlayCount = 0;
    bool isClipPlaying = false;

    public TMP_Text radioCommText;

    [Header ("Launch Codes")]
                   //   0    1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16   17   18   19   20   21   22   23   24   25
    string[] chars = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
                       //  26  27   28   29   30   31   32   33   34   35   36   37   38   39   40   41   42   43   44   45   46   47   48   49   50   51
                          "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z"};
    string[] displayChars = { null, null };
    public TMP_Text launchCodesDisplay;
    int stageNo;
    bool isButtonPressed = false;
    public float heldDuration = 0f;
    public float heldTimer = 1f;
    public string heldOrTapped = null;
    int randomVal1;
    int randomVal2;
    bool launchedPreviousRockets = false;


    // Start is called before the first frame update
    void Start()
    {
        huhCanvas.gameObject.SetActive(false);
        radioCommsCanvas.gameObject.SetActive(false);
        launchCodesCanvas.gameObject.SetActive(false);

        currentPawzzleType = GameManagerScript.instance.PrimaryPawzzles[Random.Range(0, GameManagerScript.instance.PrimaryPawzzles.Length - 1)];  //Randomly generate the first pawzzle

        if (pawzzleTypeOverride != null)  //Control which pawzzle gets generated (debug)
        {
            currentPawzzleType = pawzzleTypeOverride;
        }

        randomVal1 = Random.Range(0, chars.Length - 1);
        randomVal2 = Random.Range(0, chars.Length - 1);

        knifeValue = 100;
        InitializePawzzle(currentPawzzleType);
    }

    private void Update()
    {
        if (isButtonPressed)
        {
            heldDuration += Time.deltaTime;
        }
    }

    void InitializePawzzle(string PawzzleType)  //Activate the pawzzle's respective canvas and pick a random answer for them
    {
        if (PawzzleType == "HUH?")
        {
            InitializeHuh();
            huhCanvas.gameObject.SetActive(true);
            huhCanvas.interactable = true;
            activeCanvas = huhCanvas;
        }

        if (PawzzleType == "RADIOCOMMS")
        {
            InitializeRadioComms();
            radioCommsCanvas.gameObject.SetActive(true);
            radioCommsCanvas.interactable = true;
            activeCanvas = radioCommsCanvas;
        }

        if (PawzzleType == "LAUNCHCODES")
        {
            InitializeLaunchCodes();
            launchCodesCanvas.gameObject.SetActive(true);
            launchCodesCanvas.interactable = true;
            activeCanvas = launchCodesCanvas;
        }
    }

    void InitializeHuh()
    {
        //Pick from left or right table, this information is used in KNIFE pawzzle
        leftOrRightTable = Random.Range(0, 2);
        int rng = Random.Range(0, huhLeftDisplays.Length - 1);

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
    
    void InitializeRadioComms()  //Pick random answer for radiocomms
    {
        int rng = Random.Range(0, radioCommsWords.Length - 1);

        radioWord = radioCommsWords[rng];
        radioFrequency = radioCommsFrequencies[rng];
        radioAudioClipToPlay = radioAudioClips[rng];
    }

    void InitializeLaunchCodes()
    {
        stageNo = 1;

        displayChars[0] = chars[randomVal1];
        displayChars[1] = chars[randomVal2];

        launchCodesDisplay.text = displayChars[0] + displayChars[1];
    }


    public void SubmitPrimaryInput()  //Player presses the submit button for primary pawzzle
    {
        int inputValue = int.Parse(primaryInput.text);

        if (currentPawzzleType == "HUH?")
        {
            if (inputValue + huhValue == knifeValue)
            {
                SolvePrimaryPawzzle("KNIFE");
            }
        }

        if (currentPawzzleType == "RADIOCOMMS")
        {
            if (radioFrequency > knifeValue)
            {
                if (radioFrequency - knifeValue == inputValue)
                {
                    SolvePrimaryPawzzle("KNIFE");
                }
            }

            if (radioFrequency < knifeValue)
            {
                if (knifeValue + radioFrequency == inputValue)
                {
                    SolvePrimaryPawzzle("KNIFE");
                }
            }

            if (radioFrequency == knifeValue)
            {
                if (inputValue == radioFrequency || inputValue == knifeValue)
                {
                    SolvePrimaryPawzzle("KNIFE");
                }
            }
        }
    }

    void SolvePrimaryPawzzle(string desiredApparatus)
    {
        Debug.Log("Pawzzle solved! You accessed the " + desiredApparatus + ".");
    }

    //Pawzzle-specific button methods below


    //RADIOCOMMS
    public void PlayRadioCommClip()
    {
        if (!isClipPlaying)
        {
            toolAudioSource.clip = radioAudioClipToPlay;
            toolAudioSource.Play();
            audioPlayCount++;

            StartCoroutine(delayClip());
        }
        else
        {
            Debug.Log("Wait for the clip to finish playback");
        }
    }

    IEnumerator delayClip()
    {
        isClipPlaying = true;
        radioCommText.text = "PLAYING AUDIO...";
        radioCommText.fontSize = 20;

        yield return new WaitForSeconds(radioAudioClipToPlay.length);
        isClipPlaying = false;
        radioCommText.text = "PLAY AUDIO";
        radioCommText.fontSize = 24;
    }


    //LAUNCHCODES
    public void LaunchCodesPointerDown()
    {
        heldDuration = 0;
        isButtonPressed = true;
    }

    public void LaunchCodesPointerUp()
    {
        isButtonPressed = false;

        if (heldDuration >= heldTimer)
        {
            heldOrTapped = "Held";
        }
        else if (heldDuration < heldTimer)
        {
            heldOrTapped = "Tapped";
        }

        if (stageNo < 4)
        {
            CheckLaunchCodesPart1();
        }
    }

    void CheckLaunchCodesPart1()
    {

        if ((displayChars[0] == displayChars[1]) || (randomVal1 - 26 == randomVal2) || (randomVal2 - 26 == randomVal1))  //Check if the letters are the same
        {
            CheckLaunch("Tapped");
        }

        else if ((randomVal1 - 1 == randomVal2) || (randomVal1 + 1 == randomVal2) || (randomVal2 + 1 == randomVal2) || (randomVal1 - 27 == randomVal2) || (randomVal1 - 25 == randomVal2)
            || (randomVal2 - 27 == randomVal1) || (randomVal2 - 25 == randomVal1)) //If letters in consequetive order in alphabet
        {
            CheckLaunch("Held");
        }

        else if ((randomVal1 == 0 || randomVal1 == 4 || randomVal1 == 8 || randomVal1 == 14 || randomVal1 == 20)
            || (randomVal2 == 0 || randomVal2 == 4 || randomVal2 == 8 || randomVal2 == 14 || randomVal2 == 20))  //If either letter is both a vowel and a capital letter
        {
            if (launchedPreviousRockets)
            {
                CheckLaunch("Held");
            }

            if (!launchedPreviousRockets)
            {
                CheckLaunch("Tapped");
            }
        }

        else if (randomVal1 > 26 && randomVal2 > 26)  //If both letters are capital
        {
            CheckLaunch("Tap");
        }

        else if ((randomVal1 > 20 && randomVal1 < 26) || (randomVal2 > 20 && randomVal2 < 26) || (randomVal1 > 46) || (randomVal2 > 46))  // If either letter comes after the 20th letter
        {
            CheckLaunch("Held");
        }

        else if (randomVal1 < 4 || randomVal2 < 4 || (randomVal1 > 25 && randomVal1 < 30) || (randomVal2 > 25 && randomVal2 < 30))  //If either letter comes before the 5th letter
        {
            CheckLaunch("Tapped");
        }

        else  //Otherwise
        {
            if (launchedPreviousRockets)
            {
                CheckLaunch("Held");
            }

            if (!launchedPreviousRockets)
            {
                CheckLaunch("Tapped");
            }
        }

        if (stageNo == 3)
        {
            Debug.Log("All stages complete!");
        }

        if (stageNo < 3)
        {
            Debug.Log("Proceeding to stage " + (stageNo + 1));
            stageNo++;
        }
    }

    void CheckLaunch(string success)
    {
        if (success == heldOrTapped)
        {
            if (heldOrTapped == "Tapped")
            {
                launchedPreviousRockets = true;
            }

            Debug.Log("Success!");
        }

        else if (success != heldOrTapped)
        {
            Debug.Log("You operated the rocket incorrectly!");
        }

        //Generate a different, random launch code
        randomVal1 = Random.Range(0, chars.Length - 1);
        randomVal2 = Random.Range(0, chars.Length - 1);
        displayChars[0] = chars[randomVal1];
        displayChars[1] = chars[randomVal2];

        launchCodesDisplay.text = displayChars[0] + displayChars[1];
    }
}
