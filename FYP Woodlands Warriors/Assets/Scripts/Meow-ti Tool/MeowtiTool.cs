using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MeowtiTool : MonoBehaviour
{
    public string currentPawzzleType;

    public TMP_InputField primaryInput;

    public CanvasGroup primaryToolCanvas;
    public CanvasGroup huhCanvas;
    public CanvasGroup radioCommsCanvas;
    public CanvasGroup launchCodesCanvas;

    public CanvasGroup knifeCanvas;
    public CanvasGroup activeCanvas;

    public AudioSource toolAudioSource;

    public bool isTyping = false;

    [Header ("Debug")]
    public string pawzzleTypeOverride;

    [Header ("Apparatus Values")]
    public int knifeValue;

    [Header("Primary Pawzzles")]
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
    int rocketsLaunched = 0;
    public Button redButton;
    public TMP_Text launchCount;
    int casualties;


    [Header("Apparatus Pawzzles")]
    public string primaryPawzzleType;

    [Header("Knife")]
    public TMP_InputField cmd;
    public ScrollingText cmdScroll;


    // Start is called before the first frame update
    void Start()
    {
        huhCanvas.gameObject.SetActive(false);
        radioCommsCanvas.gameObject.SetActive(false);
        launchCodesCanvas.gameObject.SetActive(false);

        currentPawzzleType = GameManagerScript.instance.PrimaryPawzzles[Random.Range(0, GameManagerScript.instance.PrimaryPawzzles.Length)];  //Randomly generate the first pawzzle

        if (pawzzleTypeOverride != "")  //Control which pawzzle gets generated (debug)
        {
            currentPawzzleType = pawzzleTypeOverride;
        }

        randomVal1 = Random.Range(0, chars.Length - 1);
        randomVal2 = Random.Range(0, chars.Length - 1);

        knifeValue = 100;
        InitializePawzzle(currentPawzzleType);
        primaryPawzzleType = currentPawzzleType;
    }

    private void Update()
    {
        if (isButtonPressed)
        {
            heldDuration += Time.deltaTime;
        }

        if (cmd.isFocused && cmd.text != "" && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log(SolveKnife());
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

        if (PawzzleType == "KNIFE")
        {
            knifeCanvas.gameObject.SetActive(true);
            knifeCanvas.interactable = true;
            activeCanvas = knifeCanvas;
            InitializeKnife();
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
    
    void InitializeKnife()
    {
        cmdScroll.Show("Meowcrosoft Weendows" + "\n \n" + "C:\\MeowtiTool\\KNIFE>");
    }

    public void SubmitInput()  //Player presses the submit button for primary pawzzle
    {
        if (currentPawzzleType == "HUH?")
        {
            int inputValue = int.Parse(primaryInput.text);
            if (inputValue + huhValue == knifeValue)
            {
                SolvePrimaryPawzzle("KNIFE");
            }
        }

        if (currentPawzzleType == "RADIOCOMMS")
        {
            if (radioFrequency > knifeValue)
            {
                int inputValue = int.Parse(primaryInput.text);
                if (radioFrequency - knifeValue == inputValue)
                {
                    SolvePrimaryPawzzle("KNIFE");
                }
            }

            if (radioFrequency < knifeValue)
            {
                int inputValue = int.Parse(primaryInput.text);
                if (knifeValue + radioFrequency == inputValue)
                {
                    SolvePrimaryPawzzle("KNIFE");
                }
            }

            if (radioFrequency == knifeValue)
            {
                int inputValue = int.Parse(primaryInput.text);
                if (inputValue == radioFrequency || inputValue == knifeValue)
                {
                    SolvePrimaryPawzzle("KNIFE");
                }
            }
        }

        if (currentPawzzleType == "LAUNCHCODES")
        {
            int inputValue = int.Parse(primaryInput.text);
            if ((rocketsLaunched == 0 && inputValue == knifeValue * 0.1) || (rocketsLaunched == 1 && inputValue == knifeValue) || (rocketsLaunched == 2 && inputValue == knifeValue * 1.5) ||
                (rocketsLaunched == 3 && inputValue == knifeValue * 2))
            {
                casualties = inputValue;
                SolvePrimaryPawzzle("KNIFE");
            }
        }
    }

    void SolvePrimaryPawzzle(string desiredApparatus)
    {
        Debug.Log("Pawzzle solved! You accessed the " + desiredApparatus + ".");

        InitializeApparatusPawzzle(desiredApparatus);
    }

    void InitializeApparatusPawzzle(string apparatusType)
    {
        activeCanvas.interactable = false;
        activeCanvas.gameObject.SetActive(false);

        if (apparatusType == "KNIFE")
        {
            knifeCanvas.gameObject.SetActive(true);
            knifeCanvas.interactable = true;
            activeCanvas = knifeCanvas;
            InitializeKnife();
        }
    }

    void SolveApparatusPawzzle(string apparatusType)
    {

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

        if (stageNo <= 3)
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

        else if ((randomVal1 - 1 == randomVal2) || (randomVal1 + 1 == randomVal2) || (randomVal2 + 1 == randomVal2) || (randomVal1 - 26 == randomVal2) || (randomVal1 - 24 == randomVal2)
            || (randomVal2 - 26 == randomVal1) || (randomVal2 - 24 == randomVal1)) //If letters in consequetive order in alphabet
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
            redButton.interactable = false;
        }

        if (stageNo < 3)
        {
            Debug.Log("Proceeding to stage " + (stageNo + 1));  
        }
        stageNo++;
    }

    void CheckLaunch(string success)
    {
        if (heldOrTapped == "Tapped")
        {
            launchedPreviousRockets = true;
            rocketsLaunched++;
            launchCount.text = rocketsLaunched.ToString();
        }

        if (success == heldOrTapped)
        {
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


    //KNIFE
    public string SolveKnife()
    {
        if (primaryPawzzleType == "HUH?") //If primary was Huh? and...
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

        if (primaryPawzzleType == "RADIOCOMMS") //If primary pawzzle was Radio Comms...
        {
            int firstFreqDigit = ((radioFrequency % 1000) - (radioFrequency % 100)) / 100;

            if (cmd.text == "spread(" + audioPlayCount + ", 7);")
            {
                return "CUT/SLICE";
            }

            if (cmd.text == "Slice( " + firstFreqDigit + ",2)")
            {
                return "SPREAD";
            }

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
