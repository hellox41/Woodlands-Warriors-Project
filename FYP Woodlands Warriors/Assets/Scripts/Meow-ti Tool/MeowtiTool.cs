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
    public string currentPawzzleAdjective;

    public TMP_InputField primaryInput;

    public CanvasGroup primaryToolCanvas;
    public CanvasGroup eighteenCarrotCanvas;
    public CanvasGroup radioCafeCanvas;
    public CanvasGroup vitaminsCanvas;

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

    [Space]

    [Header ("Radio Cafe")]
    string[] radioCafeWords = { "SMALL", "SMART", "COATED", "BENDY", "BEEFY", "TASTY", "CORNED", "SPICY", "TACKY", "SPIKY", "SMOKY", "RUSTY", "CURED", "CORKED", "ROAST", "CURRY"};
    public AudioClip[] radioAudioClips;

    public string radioWord;

    public AudioClip radioAudioClipToPlay;
    public int audioPlayCount = 0;
    bool isClipPlaying = false;

    public TMP_Text radioCafeText;

    [Space]

    [Header("Vitamins")]
    string[] vitaminFoods = { "MILK", "CITRUS", "EGGS", "BELL PEPPER", "FISH", "BROCCOLI", "BREAD", "SEAWEED", "CARROT" };
    [SerializeField] Sprite[] vitaminImages;
    [SerializeField] Image vitaminImage1;
    [SerializeField] Image vitaminImage2;
    [SerializeField] List<string> vitaminsPresent = new List<string>();
    [SerializeField] List<string> vitaminsToAdd = new List<string>();
    string vitaminFood1;
    string vitaminFood2;

    [Header("Apparatus Pawzzles")]
    public string primaryPawzzleType;

    [Header("Knife")]
    public TMP_InputField cmd;
    public ScrollingText cmdScroll;
    public string knifeActionType;

    //PAWZZLE SYSTEM BREAKDOWN: Start(): Pick a random pawzzle from the List<> of pawzzles in GameManagerScript, then initialize apparatus nouns and the chosen pawzzle type.

    // Start is called before the first frame update
    void Start()
    {
        eighteenCarrotCanvas.gameObject.SetActive(false);
        radioCafeCanvas.gameObject.SetActive(false);
        vitaminsCanvas.gameObject.SetActive(false);

        currentPawzzleType = GameManagerScript.instance.PrimaryPawzzles[Random.Range(0, GameManagerScript.instance.PrimaryPawzzles.Length)];  //Randomly generate the first pawzzle

        if (pawzzleTypeOverride != "")  //Control which pawzzle gets generated (debug)
        {
            currentPawzzleType = pawzzleTypeOverride;
        }

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

    }

    void RemoveElement<T>(ref T[] arr, int index)  //Remove an element from an array and resize the array
    {
        for (int i = index; i < arr.Length - 1; i++)
        {
            arr[i] = arr[i + 1];
        }

        Array.Resize(ref arr, arr.Length - 1);
    }

    void InitializeNoun(int apparatusIndex)  //Create nouns for the kitchen apparatus on level start
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

        if (PawzzleType == "VITAMINS")
        {
            InitializeVitamins();
            vitaminsCanvas.gameObject.SetActive(true);
            vitaminsCanvas.interactable = true;
            activeCanvas = vitaminsCanvas;
        }

        if (PawzzleType == "KNIFE")
        {
            knifeCanvas.gameObject.SetActive(true);
            knifeCanvas.interactable = true;
            activeCanvas = knifeCanvas;
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
            currentPawzzleAdjective = eighteenCarrotLeftAdjectives[rng];
        }

        else if(leftOrRightTable == 1)
        {
            textToDisplay = eighteenCarrotRightDisplays[rng]; //Set the desired text to a random text on the right
            currentPawzzleAdjective = eighteenCarrotRightAdjectives[rng];
        }

        eighteenCarrotText.text = textToDisplay;  //Set the desired text to the display text
    }
    
    void InitializeRadioCafe()  //Pick random answer for radiocafe
    {
        int rng = Random.Range(0, radioCafeWords.Length - 1);

        currentPawzzleAdjective = radioCafeWords[rng];
        radioAudioClipToPlay = radioAudioClips[rng];
    }

    void InitializeVitamins()  //Pick two random foods for vitamins and assign the vitamins
    {
        for (int i = 0; i < 2; i++)  //Pick 2 foods, assigning them to variables and images, then remove them from the list of possible foods when picked.
        {
            int rng = Random.Range(0, vitaminFoods.Length - 1);

            if (i == 0)
            {
                vitaminFood1 = vitaminFoods[rng];
                vitaminImage1.sprite = vitaminImages[rng];
                vitaminImage1.SetNativeSize();
            }

            else if (i == 1)
            {
                vitaminFood2 = vitaminFoods[rng];
                vitaminImage2.sprite = vitaminImages[rng];
                vitaminImage2.SetNativeSize();
            }

            RemoveElement(ref vitaminFoods, rng);
            RemoveElement(ref vitaminImages, rng);
        }

        //Add vitamin types of the chosen foods to a list for later
        if (vitaminFood1 == "MILK" || vitaminFood2 == "MILK")
        {
            vitaminsToAdd.Add("A"); vitaminsToAdd.Add("Bs");
        }

        if (vitaminFood1 == "CITRUS" || vitaminFood2 == "CITRUS")
        {
            vitaminsToAdd.Add("Bs"); vitaminsToAdd.Add("C");
        }

        if (vitaminFood1 == "EGGS" || vitaminFood2 == "EGGS")
        {
            vitaminsToAdd.Add("A"); vitaminsToAdd.Add("Bs"); vitaminsToAdd.Add("K");
        }

        if (vitaminFood1 == "BELL PEPPER" || vitaminFood2 == "BELL PEPPER")
        {
            vitaminsToAdd.Add("A"); vitaminsToAdd.Add("Bs"); vitaminsToAdd.Add("C"); vitaminsToAdd.Add("E");
        }

        if (vitaminFood1 == "FISH" || vitaminFood2 == "FISH")
        {
            vitaminsToAdd.Add("Bs"); vitaminsToAdd.Add("D");
        }

        if (vitaminFood1 == "BROCCOLI" || vitaminFood2 == "BROCCOLI")
        {
            vitaminsToAdd.Add("A"); vitaminsToAdd.Add("Bs"); vitaminsToAdd.Add("D"); vitaminsToAdd.Add("E"); vitaminsToAdd.Add("K");
        }

        if (vitaminFood1 == "BREAD" || vitaminFood2 == "BREAD")
        {
            vitaminsToAdd.Add("Bs");
        }

        if (vitaminFood1 == "SEAWEED" || vitaminFood2 == "SEAWEED")
        {
            vitaminsToAdd.Add("A"); vitaminsToAdd.Add("C"); vitaminsToAdd.Add("E");
        }

        if (vitaminFood1 == "CARROT" || vitaminFood2 == "CARROT")
        {
            vitaminsToAdd.Add("Bs"); vitaminsToAdd.Add("C"); vitaminsToAdd.Add("K");
        }

        for (int i = 0; i < vitaminsToAdd.Count; i++)  //Add the vitamin types to a list where there are only 1 vitamin present for each vitamin
        {
            if (!vitaminsPresent.Contains(vitaminsToAdd[i]))
            {
                vitaminsPresent.Add(vitaminsToAdd[i]);
            }
        }

        AssignVitaminsAdjective();
    }
    
    void InitializeKnife()  //Start scrolling text for knife
    {
        cmdScroll.Show("Meowcrosoft Weendows" + "\n \n" + "C:\\MeowtiTool\\KNIFE>");
    }

    public void SubmitInput()  //Player presses the submit button for primary pawzzle
    {
        if (primaryInput.text == currentPawzzleAdjective + " " + knifeNoun)  //If correct
        {
            SolvePrimaryPawzzle("KNIFE");
        }

        else if (primaryInput.text != currentPawzzleAdjective + " " + knifeNoun)  //If wrong
        {
            GameManagerScript.instance.ErrorMade();
        }
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


    //VITAMINS
    void AssignVitaminsAdjective()
    {
        if (CheckVitamins("Bs", 1))  //Bs absent in either foods
        {
            currentPawzzleAdjective = "WEEDY";
        }

        else if (CheckVitamins("C", 2)) //C present in both foods
        {
            currentPawzzleAdjective = "MOSSY";
        }

        else if (vitaminsPresent.Contains("D") || vitaminsPresent.Contains("K"))  //D or K present in either foods
        {
            currentPawzzleAdjective = "BLIND";
        }

        else if (!vitaminsPresent.Contains("A")) //A absent in both foods
        {
            currentPawzzleAdjective = "AROMATIC";
        }

        else if ((vitaminFood1 == "BELL PEPPER" && vitaminFood2 == "BROCCOLI") ||(vitaminFood1 == "BROCCOLI" && vitaminFood2 == "BELL PEPPER"))  //Bs and E present in both foods
        {
            currentPawzzleAdjective = "STARCHY";
        }

        else if (vitaminsPresent.Contains("C"))  //C present in either foods
        {
            currentPawzzleAdjective = "TANGY";
        }

        else  //(no other requirement)
        {
            currentPawzzleAdjective = "MUNDANE";
        }
    }

    bool CheckVitamins(string vitamin,int numberOfChecks)
    {
        int numberOfOccurences = 0;

        for (int i = 0; i < vitaminsToAdd.Count; i++)
        {
            if (vitaminsToAdd[i] == vitamin)
            {
                numberOfOccurences++;
            }
        }

        if (numberOfOccurences == numberOfChecks)
        {
            return true;
        }

        else
        {
            return false;
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

        else return null;
    }
}
