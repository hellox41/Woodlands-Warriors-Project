using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class MeowtiTool : MonoBehaviour
{
    public string currentPawzzleType;

    public TMP_InputField primaryInput;

    public CanvasGroup primaryToolCanvas;
    public CanvasGroup eighteenCarrotCanvas;
    public CanvasGroup radioCafeCanvas;
    public CanvasGroup launchCodesCanvas;

    public CanvasGroup knifeCanvas;
    public CanvasGroup stoveCanvas;
    public CanvasGroup activeCanvas;

    public MeowtiToolInteraction meowtiToolInteraction;

    public AudioSource toolAudioSource;

    public bool isTyping = false;

    [Header ("Debug")]
    public string pawzzleTypeOverride;

    [Header("Apparatus Nouns")]
    public string[] possibleApparatusNouns = { "MONKEY", "CARROT", "GRANDMA", "CHICKEN", "BANANA", "POTATO", "SOTONG" };
    string knifeNoun;   //index 0
    string skilletNoun; //index 1
    public TMP_Text apparatusNounsText;

    [Header ("18 Carrot Configuration")]
    public TMP_Text eighteenCarrotText;
    string[] eighteenCarrotLeftDisplays = { "CARROT", "CARET", "THE WORD CARROT", "SEE EH AR AR OH TEE WORDS ONLY", "KAY AR AR EE TEE", "CARROT SYMBOL", 
        "JUST THE WORD CARROT", "CARET VEGETABLE BUT THE WORD ^ IS GOLD THEN THE SECOND CARAT IS THE SYMBOL", 
        "CARET WITH AN A", "KARAT", "THE WORD LIKE THE GOLD CARROT BUT THE CARET IS THE VEGETABLE", "K A Y SPACE A Y SPACE A R SPACE E E SPACE T E E", "K A Y  A Y  A R  E E  T E E", };

    string[] eighteenCarrotRightDisplays = { "CARAT", "^", "THE LETTERS C A R R O T ONE WORD", "SEE EH AR AR OH TEE",
        "KAY AY AR EE TEE LETTERS ONLY", "CARROT SYMBOL TWO WORDS", "SEE EH AR AY OH TEE", "CARROT VEGETABLE", "LITERALLY JUST THE WORD CARROT LIKE THE VEGETABLE",
        "CARROT WITH AN E", "S E E SPACE E H SPACE A R SPACE A R SPACE O H SPACE T E E ALL WORDS THIRTEEN LETTERS IN TOTAL", "KAY AY AR EE TEE NINE WORDS IN TOTAL", 
        "^ TWO WORDS" };

    string[] eighteenCarrotLeftAdjectives = { "COLD", "HOT", "STINKY", "COMPLEX", "RAW", "FLYING", "THICK", "BURNT", "ARTIFICIAL", "FRIED", "BOILED", "BLACK", "WHOLE" };
    string[] eighteenCarrotRightAdjectives= { "SMELLY", "SPICY", "SMALL", "SCARY", "FROZEN", "BROWN", "ROTTEN", "STRANGE", "JUICY", "FIERY", "HOLY", "HEAVY", "SEXY" };

    public string textToDisplay;
    public int leftOrRightTable;
    public string eighteenCarrotAdjective;

    [Header ("Radio Cafe")]
    string[] radioCafeWords = { "SMALL", "SMART", "COATED", "BENDY", "BEEFY", "TASTY", "CORNED", "SPICY", "TACKY", "SPIKY", "SMOKY", "RUSTY", "CURED", "CORKED", "ROAST", "CURRY"};
    public AudioClip[] radioAudioClips;

    public string radioWord;

    public AudioClip radioAudioClipToPlay;
    public int audioPlayCount = 0;
    bool isClipPlaying = false;

    public TMP_Text radioCafeText;

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
    public int rocketsLaunched = 0;
    public Button redButton;
    public TMP_Text launchCount;
    int casualties;
    string correctButtonType;


    [Header("Apparatus Pawzzles")]
    public string primaryPawzzleType;

    [Header("Knife")]
    public TMP_InputField cmd;
    public ScrollingText cmdScroll;
    public string knifeActionType;


    // Start is called before the first frame update
    void Start()
    {
        eighteenCarrotCanvas.gameObject.SetActive(false);
        radioCafeCanvas.gameObject.SetActive(false);
        launchCodesCanvas.gameObject.SetActive(false);

        currentPawzzleType = GameManagerScript.instance.PrimaryPawzzles[Random.Range(0, GameManagerScript.instance.PrimaryPawzzles.Length)];  //Randomly generate the first pawzzle

        if (pawzzleTypeOverride != "")  //Control which pawzzle gets generated (debug)
        {
            currentPawzzleType = pawzzleTypeOverride;
        }

        randomVal1 = Random.Range(0, chars.Length - 1);
        randomVal2 = Random.Range(0, chars.Length - 1);

        for (int i = 0; i < 2; i++)  //Initialize apparatus nouns
        {
            InitializeNoun(i);
        }
        apparatusNounsText.text = "APPARATUS NOUNS \n \n KNIFE - " + knifeNoun + "\n SKILLET - " + skilletNoun; 

        InitializePawzzle(currentPawzzleType);
        primaryPawzzleType = currentPawzzleType;
    }

    private void Update()
    {
        if (isButtonPressed)
        {
            heldDuration += Time.deltaTime;
        }

        if (isTyping && cmd.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            if (SolveKnife() != null)
            {
                cmdScroll.Show(SolveKnife() + " METHOD SUCCESSFULLY ACCESSED.");
                meowtiToolInteraction.currentApparatusType = "KNIFE";
                meowtiToolInteraction.currentActionType = SolveKnife();
            }
        }
    }

    void RemoveElement<T>(ref T[] arr, int index)
    {
        for (int i = index; i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }

        Array.Resize(ref arr, arr.Length - 1);
    }

    void InitializeNoun(int apparatusIndex)
    {
        int rng = Random.Range(0, possibleApparatusNouns.Length - 1);

        if (apparatusIndex == 0)
        {
            knifeNoun = possibleApparatusNouns[rng];
        }

        else if(apparatusIndex == 1)
        {
            skilletNoun = possibleApparatusNouns[rng];
        }

        RemoveElement(ref possibleApparatusNouns, rng);
    }

    void InitializePawzzle(string PawzzleType)  //Activate the pawzzle's respective canvas and pick a random answer for them
    {
        if (PawzzleType == "18CARROT")
        {
            Initialize18Carrot();
            eighteenCarrotCanvas.gameObject.SetActive(true);
            eighteenCarrotCanvas.interactable = true;
            activeCanvas = eighteenCarrotCanvas;
        }

        if (PawzzleType == "RADIOCAFE")
        {
            InitializeRadioCafe();
            radioCafeCanvas.gameObject.SetActive(true);
            radioCafeCanvas.interactable = true;
            activeCanvas = radioCafeCanvas;
        }

        if (PawzzleType == "LAUNCHCODES")
        {
            InitializeLaunchCodes();
            launchCodesCanvas.gameObject.SetActive(true);
            launchCodesCanvas.interactable = true;
            activeCanvas = launchCodesCanvas;
        }

        if (PawzzleType == "KNIFE")
        {
            knifeCanvas.gameObject.SetActive(true);
            knifeCanvas.interactable = true;
            activeCanvas = knifeCanvas;
            InitializeKnife();
        }

        if (PawzzleType == "STOVE")
        {
            stoveCanvas.gameObject.SetActive(true);
            stoveCanvas.interactable = true;
            activeCanvas = stoveCanvas;
            InitializeKnife();
        }
    }

    void Initialize18Carrot()
    {
        //Pick from left or right table, this information is used in KNIFE pawzzle
        leftOrRightTable = Random.Range(0, 2);
        int rng = Random.Range(0, eighteenCarrotLeftDisplays.Length - 1);

        if (leftOrRightTable == 0)
        {
            textToDisplay = eighteenCarrotLeftDisplays[rng]; //Set the desired text to a random text on the left
            eighteenCarrotAdjective = eighteenCarrotLeftAdjectives[rng];
        }

        else if(leftOrRightTable == 1)
        {
            textToDisplay = eighteenCarrotRightDisplays[rng]; //Set the desired text to a random text on the right
            eighteenCarrotAdjective = eighteenCarrotRightAdjectives[rng];
        }

        eighteenCarrotText.text = textToDisplay;  //Set the desired text to the display text
    }
    
    void InitializeRadioCafe()  //Pick random answer for radiocafe
    {
        int rng = Random.Range(0, radioCafeWords.Length - 1);

        radioWord = radioCafeWords[rng];
        radioAudioClipToPlay = radioAudioClips[rng];
    }

    void InitializeLaunchCodes()  //Pick random code for launch codes
    {
        stageNo = 1;

        displayChars[0] = chars[randomVal1];
        displayChars[1] = chars[randomVal2];

        launchCodesDisplay.text = displayChars[0] + displayChars[1];
    }
    
    void InitializeKnife()  //Start scrolling text for knife
    {
        cmdScroll.Show("Meowcrosoft Weendows" + "\n \n" + "C:\\MeowtiTool\\KNIFE>");
    }

    public void SubmitInput()  //Player presses the submit button for primary pawzzle
    {
        if (currentPawzzleType == "18CARROT")
        {
            if (primaryInput.text == eighteenCarrotAdjective + " " + knifeNoun)
            {
                SolvePrimaryPawzzle("KNIFE");
            }
        }

        if (currentPawzzleType == "RADIOCAFE")
        {
            if (primaryInput.text == radioWord + " " + knifeNoun)
            {
                SolvePrimaryPawzzle("KNIFE");
            }
        }

        /*if (currentPawzzleType == "LAUNCHCODES")
        {
            int inputValue = int.Parse(primaryInput.text);
            if ((rocketsLaunched == 0 && inputValue == knifeValue * 0.1) || (rocketsLaunched == 1 && inputValue == knifeValue) || (rocketsLaunched == 2 && inputValue == knifeValue * 1.5) ||
                (rocketsLaunched == 3 && inputValue == knifeValue * 2.5))
            {
                casualties = inputValue;
                SolvePrimaryPawzzle("KNIFE");
            }
        }*/
    }

    void SolvePrimaryPawzzle(string desiredApparatus)
    {
        GameManagerScript.instance.accessedApparatus = desiredApparatus;
        Debug.Log("Pawzzle solved! You accessed the " + desiredApparatus + ".");     
    }

    public void StartTyping()
    {
        isTyping = true;
    }

    public void StopTyping()
    {
        isTyping = false;
    }

    //Pawzzle-specific button methods below


    //RADIOCAFE
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
        radioCafeText.text = "PLAYING AUDIO...";
        radioCafeText.fontSize = 20;

        yield return new WaitForSeconds(radioAudioClipToPlay.length);
        isClipPlaying = false;
        radioCafeText.text = "PLAY AUDIO";
        radioCafeText.fontSize = 24;
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

        if (stageNo <= 3)
        {
            CheckLaunchCodesPart1();
        }
    }

    void CheckLaunchCodesPart1()
    {
        if ((displayChars[0] == displayChars[1]) || (randomVal1 - 26 == randomVal2) || (randomVal2 - 26 == randomVal1))  //Check if the letters are the same
        {
            correctButtonType = "Tapped";
            CheckLaunch();
        }

        else if ((randomVal1 - 1 == randomVal2) || (randomVal1 + 1 == randomVal2) || (randomVal2 + 1 == randomVal2) || (randomVal1 - 27 == randomVal2) || (randomVal1 - 25 == randomVal2)
            || (randomVal2 - 27 == randomVal1) || (randomVal2 - 25 == randomVal1)) //If letters in consequetive order in alphabet
        {
            correctButtonType = "Held";
            CheckLaunch();
        }

        else if ((randomVal1 == 0 || randomVal1 == 4 || randomVal1 == 8 || randomVal1 == 14 || randomVal1 == 20)
            || (randomVal2 == 0 || randomVal2 == 4 || randomVal2 == 8 || randomVal2 == 14 || randomVal2 == 20))  //If either letter is both a vowel and a capital letter
        {
            if (launchedPreviousRockets)
            {
                correctButtonType = "Held";
                CheckLaunch();
            }

            if (!launchedPreviousRockets)
            {
                correctButtonType = "Tapped";
                CheckLaunch();
            }
        }

        else if (randomVal1 > 26 && randomVal2 > 26)  //If both letters are capital
        {
            correctButtonType = "Tapped";
            CheckLaunch();
        }

        else if ((randomVal1 > 20 && randomVal1 < 26) || (randomVal2 > 20 && randomVal2 < 26) || (randomVal1 > 46) || (randomVal2 > 46))  // If either letter comes after the 20th letter
        {
            correctButtonType = "Held";
            CheckLaunch();
        }

        else if (randomVal1 < 4 || randomVal2 < 4 || (randomVal1 > 25 && randomVal1 < 30) || (randomVal2 > 25 && randomVal2 < 30))  //If either letter comes before the 5th letter
        {
            correctButtonType = "Tapped";
            CheckLaunch();
        }

        else  //Otherwise
        {
            if (launchedPreviousRockets)
            {
                correctButtonType = "Held";
                CheckLaunch();
            }

            if (!launchedPreviousRockets)
            {
                correctButtonType = "Tapped";
                CheckLaunch();
            }
        }

        if (stageNo == 3)
        {
            Debug.Log("All stages complete!");
            redButton.interactable = false;
        }

        if (stageNo < 3)
        {
            Debug.Log("Proceeding to stage " + (stageNo + 1));  
        }
        stageNo++;
    }

    void CheckLaunch()
    {
        if (heldOrTapped == "Tapped")
        {
            launchedPreviousRockets = true;
            rocketsLaunched++;
            launchCount.text = rocketsLaunched.ToString();
        }

        if (correctButtonType == heldOrTapped)
        {
            Debug.Log("Success!");
        }

        else if (correctButtonType != heldOrTapped)
        {
            Debug.Log("You operated the rocket incorrectly!");
        }

        //Generate a different, random launch code
        if (stageNo < 3)
        {
            randomVal1 = Random.Range(0, chars.Length - 1);
            randomVal2 = Random.Range(0, chars.Length - 1);
            displayChars[0] = chars[randomVal1];
            displayChars[1] = chars[randomVal2];

            launchCodesDisplay.text = displayChars[0] + displayChars[1];
        }

        if (stageNo == 3)
        {
            launchCodesDisplay.text = "";
        }
    }


    //KNIFE
    public string SolveKnife()
    {
        if (primaryPawzzleType == "18CARROT") //If primary was 18 Carrot and...
        {
            if (leftOrRightTable == 0) //table chosen was on the left
            {
                if (cmd.text == "spread(1, 3);")
                {
                    return "CUT/SLICE";
                }

                if (cmd.text == "Slice( 0,1)")
                {
                    return "SPREAD";
                }
            }

            if (leftOrRightTable == 1) //table chosen was on the right
            {
                if (cmd.text == "spread(3, 5);")
                {
                    return "CUT/SLICE";
                }

                if (cmd.text == "Slice( 1,0)")
                {
                    return "SPREAD";
                }
            }
        }

        if (primaryPawzzleType == "RADIOCAFE") //If primary pawzzle was Radio Comms...
        {
            //int firstFreqDigit = ((radioFrequency % 1000) - (radioFrequency % 100)) / 100;

            if (cmd.text == "spread(" + audioPlayCount + ", 7);")
            {
                return "CUT/SLICE";
            }

            /*if (cmd.text == "Slice( " + firstFreqDigit + ",2)")
            {
                return "SPREAD";
            }*/

            return null;
        }

        if (primaryPawzzleType == "LAUNCHCODES")
        {
            int firstCasDigit = ((casualties % 1000) - (casualties % 100)) / 100;

            if (firstCasDigit == 0)
            {
                firstCasDigit = ((casualties % 100) - (casualties % 10)) / 10;
            }

            int lastCasDigit = casualties % 10;
            if (cmd.text == "spread(" + rocketsLaunched + ", " + (3 - rocketsLaunched) + ");")
            {
                return "CUT/SLICE";
            }

            if (cmd.text == "Slice( " + firstCasDigit + "," + lastCasDigit + ")")
            {
                return "SPREAD";
            }

            return null;
        }

        else return null;
    }
}
