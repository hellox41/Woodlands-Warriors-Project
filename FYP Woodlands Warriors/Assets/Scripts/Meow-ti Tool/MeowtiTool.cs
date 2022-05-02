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
    public CanvasGroup radioCommsCanvas;
    public CanvasGroup launchCodesCanvas;
    public CanvasGroup activeCanvas;

    public AudioSource toolAudioSource;

    [Header("Debug")]
    public string pawzzleTypeOverride;

    [Header("Apparatus Values")]
    public int knifeValue;

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

    [Header("Radio Comms")]
    string[] radioCommsWords = { "BEATS", "BELFRY", "BELLS", "BENIGN", "COMIC", "COMMIE", "COMMIT", "CULLS", "CURED", "CURRY", "RUINED", "RUSHES", "RUSSET", "RUSSIA", "RUSTIC", "RUSTY" };
    int[] radioCommsFrequencies = { 619, 261, 173, 506, 092, 452, 369, 992, 142, 748, 338, 410, 028, 645, 100 };
    public AudioClip[] radioAudioClips;

    public string radioWord;
    public int radioFrequency;
    public AudioClip radioAudioClipToPlay;
    public int audioPlayCount = 0;
    bool isClipPlaying = false;

    public TMP_Text radioCommText;

    // Start is called before the first frame update
    void Start()
    {
        huhCanvas.gameObject.SetActive(false);
        radioCommsCanvas.gameObject.SetActive(false);
        //launchCodesCanvas.gameObject.SetActive(false);

        currentPawzzleType = GameManagerScript.instance.PrimaryPawzzles[Random.Range(0, GameManagerScript.instance.PrimaryPawzzles.Length - 1)];  //Randomly generate the first pawzzle

        if (pawzzleTypeOverride != null)  //Control which pawzzle gets generated (debug)
        {
            currentPawzzleType = pawzzleTypeOverride;
        }

        knifeValue = 100;
        InitializePawzzle(currentPawzzleType);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            huhCanvas.interactable = true;
            activeCanvas = radioCommsCanvas;
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
}
