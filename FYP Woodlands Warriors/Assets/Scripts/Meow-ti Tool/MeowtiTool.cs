using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class MeowtiTool : MonoBehaviour
{
    public string currentPawzzleType;
    public string currentPawzzleAdjective;

    public TMP_InputField primaryInput;

    public TMP_Text currentApparatusLabel;

    public GameObject accessedPanel;
    public GameObject inputPanel;

    [Header("Canvases")]
    public CanvasGroup primaryToolCanvas;
    public CanvasGroup eighteenCarrotCanvas;
    public CanvasGroup radioCafeCanvas;
    public CanvasGroup vitaminsCanvas;
    public CanvasGroup f5Canvas;
    public CanvasGroup healthyPlateCanvas;
    public CanvasGroup blindSpotsCanvas;
    public CanvasGroup aporkalypseCanvas;

    public CanvasGroup activeCanvas;

    public AudioSource toolAudioSource;

    public bool isTyping = false;

    [Header("Tool Heads")]
    public GameObject knifeHead;
    public GameObject spatulaHead;
    public GameObject strainerHead;
    public GameObject pestleHead;

    public Animator knifeAnimator;
    public Animator spatulaAnimator;
    public Animator strainerAnimator;
    public Animator pestleAnimator;

    [Header("Debug")]
    public string pawzzleTypeOverride;

    [Header("Apparatus Nouns")]
    public string[] possibleApparatusNouns = { "MONKEY", "CARROT", "GRANDMA", "CHICKEN", "BANANA", "POTATO", "SOTONG" };
    List<string> apparatusNouns = new List<string>();
    string knifeNoun;   //index 0
    string spatulaNoun; //index 1
    string strainerNoun; //index 2
    string pestleNoun; //index 3
    public TMP_Text apparatusNounsText;

    [Header("F5")]
    //validF5Foods = { "ZUCCHINI", "SHALLOT", "PEACH", "BELL PEPPER", "DURIAN" };
    //invalidF5Foods = { "CUCUMBER", "ONION", "NECTARINE", "JALAPENO", "JACKFRUIT" };
    [SerializeField] Sprite[] validF5Images;
    [SerializeField] Sprite[] invalidF5Images;
    [SerializeField] Image f5Image1;
    [SerializeField] Image f5Image2;

    [Header("18 Carrot")]
    public TMP_Text eighteenCarrotText;
    string[] eighteenCarrotLeftDisplays = { "CARROT", "CARET", "THE WORD CARROT", "SEE EH AR AR OH TEE WORDS ONLY", "KAY AR AR EE TEE", "CARROT SYMBOL",
        "JUST THE WORD CARROT", "CARET VEGETABLE BUT THE WORD ^ IS GOLD THEN THE SECOND CARAT IS THE SYMBOL",
        "CARET WITH AN A", "KARAT", "THE WORD LIKE THE GOLD CARROT BUT THE CARET IS THE VEGETABLE", "K A Y SPACE A Y SPACE A R SPACE E E SPACE T E E", "K A Y  A Y  A R  E E  T E E", };

    string[] eighteenCarrotRightDisplays = { "CARAT", "^", "THE LETTERS C A R R O T ONE WORD", "SEE EH AR AR OH TEE",
        "KAY AY AR EE TEE LETTERS ONLY", "CARROT SYMBOL TWO WORDS", "SEE EH AR AY OH TEE", "CARROT VEGETABLE", "LITERALLY JUST THE WORD CARROT LIKE THE VEGETABLE",
        "CARROT WITH AN E", "S E E SPACE E H SPACE A R SPACE A R SPACE O H SPACE T E E ALL WORDS THIRTEEN LETTERS IN TOTAL", "KAY AY AR EE TEE NINE WORDS IN TOTAL",
        "^ TWO WORDS" };

    string[] eighteenCarrotLeftAdjectives = { "COLD", "HOT", "STINKY", "COMPLEX", "RAW", "FLYING", "THICK", "BURNT", "ARTIFICIAL", "FRIED", "BOILED", "BLACK", "WHOLE" };
    string[] eighteenCarrotRightAdjectives = { "SMELLY", "SPICY", "SMALL", "SCARY", "FROZEN", "BROWN", "ROTTEN", "STRANGE", "JUICY", "FIERY", "HOLY", "HEAVY", "SEXY" };

    public string textToDisplay;
    public int leftOrRightTable;
    public string eighteenCarrotAdjective;

    [Space]

    [Header("Radio Cafe")]
    string[] radioCafeWords = { "SMALL", "SMART", "COATED", "BENDY", "BEEFY", "TASTY", "CORNED", "SPICY", "TACKY", "SPIKY", "SMOKY", "RUSTY", "CURED", "CORKED", "ROAST", "CURRY" };
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

    [Header("Healthy Plate")]  //    0        1         2        3        4         5         6          7         8        9
    //                            { "MILK", "BREAD", "TOMATO", "BACON", "BARLEY", "LEMON", "BROCCOLI", "STEAK", "CARROT", "FISH" }
    [SerializeField] List<string> healthyPlateFoods = new List<string>();
    [SerializeField] List<Sprite> healthyPlateSprites = new List<Sprite>();
    [SerializeField] List<Sprite> selectionRangeImages = new List<Sprite>();
    public string unassignedCategory;
    [SerializeField] Image staticFoodImage;
    [SerializeField] Image selectionRangeImage;
    [SerializeField] List<string> selectionRangeFoods = new List<string>();
    List<string> assignableFoods = new List<string>();
    public string staticFood;
    public string unknownFood;
    public string isolatedFood;
    public int isolatedFoodValue;
    List<int> selectionRangeValues = new List<int>();
    List<string> fruitAndVegetables = new List<string>();
    List<string> meatAndOthers = new List<string>();
    List<string> grains = new List<string>();
    int imageIndex = 0;

    [Header("Blind Spots")]
    public AudioClip[] blindSpotsClips;  //0 = stirFrying, 1 = heavyRain, 2 = boiling, 3 = deepFrying
    public GameObject soundSpotObj;
    public List<string> blindSpotsFoods = new List<string>();
    public List<GameObject> soundSpots;
    public string blindSpotsFood;
    public Transform blindSpotSpawnPoint;
    public GameObject blindSpotsPanel;

    [Header("Aporkalypse")]
    public TMP_Text labelText;
    public TMP_Text replyField;
    public List<string> possibleLabels = new List<string>();
    public List<string> aporkalypseReplies = new List<string>();
    public Color32[] labelColors;  //[0] = green, [1] = red, [2] = blue, [3] = yellow
    public List<int> dateValues = new List<int>();  //[0] = day (tens), [1] = day (ones), [2] = month
    public string porkCut;
    public string reply;
    public string omenType;

    //PAWZZLE SYSTEM BREAKDOWN: Start(): Pick a random pawzzle from the List<> of pawzzles in GameManagerScript, then initialize apparatus nouns and the chosen pawzzle type.

    // Start is called before the first frame update
    void Start()
    {
        eighteenCarrotCanvas.gameObject.SetActive(false);
        radioCafeCanvas.gameObject.SetActive(false);
        vitaminsCanvas.gameObject.SetActive(false);
        f5Canvas.gameObject.SetActive(false);
        healthyPlateCanvas.gameObject.SetActive(false);
        blindSpotsCanvas.gameObject.SetActive(false);

        //Randomly generate the first pawzzle
        GeneratePawzzle();

        if (pawzzleTypeOverride != "")  //Control which pawzzle gets generated (debug)
        {
            currentPawzzleType = pawzzleTypeOverride;
            Debug.Log(currentPawzzleType + " pawzzle created through debug settings");
        }

        for (int i = 0; i < 4; i++)  //Initialize apparatus nouns
        {
            InitializeNoun(i);
        }
        apparatusNounsText.text = "APPARATUS NOUNS \n \n KNIFE - " + knifeNoun + "\n SPATULA - " + spatulaNoun + "\n STRAINER - " + strainerNoun +
            "\n PESTLE - " + pestleNoun;

        InitializePawzzle(currentPawzzleType);
    }

    //Generate a random pawzzle, depending on the level's difficulty (1 includes F5, 18Carrot, radioCafe)
    void GeneratePawzzle()
    {
        if (GameManagerScript.instance.pawzzleDifficulty == 1)
        {
            currentPawzzleType = GameManagerScript.instance.PrimaryPawzzles[Random.Range(0, 3)];
        }

        if (GameManagerScript.instance.pawzzleDifficulty == 2)
        {
            currentPawzzleType = GameManagerScript.instance.PrimaryPawzzles[Random.Range(3, 5)];
        }
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

        else if (apparatusIndex == 1)
        {
            spatulaNoun = possibleApparatusNouns[rng];
        }

        else if (apparatusIndex == 2)
        {
            strainerNoun = possibleApparatusNouns[rng];
        }

        else if (apparatusIndex == 3)
        {
            pestleNoun = possibleApparatusNouns[rng];
        }

        apparatusNouns.Add(possibleApparatusNouns[rng]);
        RemoveElement(ref possibleApparatusNouns, rng);
    }

    void InitializePawzzle(string PawzzleType)  //Activate the pawzzle's respective canvas and pick a random answer for them
    {
        if (PawzzleType == "F5")
        {
            InitializeF5();
            f5Canvas.gameObject.SetActive(true);
            f5Canvas.interactable = true;
            activeCanvas = f5Canvas;
        }

        else if (PawzzleType == "18CARROT")
        {
            Initialize18Carrot();
            eighteenCarrotCanvas.gameObject.SetActive(true);
            eighteenCarrotCanvas.interactable = true;
            activeCanvas = eighteenCarrotCanvas;
        }

        else if (PawzzleType == "RADIOCAFE")
        {
            InitializeRadioCafe();
            radioCafeCanvas.gameObject.SetActive(true);
            radioCafeCanvas.interactable = true;
            activeCanvas = radioCafeCanvas;
        }

        else if (PawzzleType == "VITAMINS")
        {
            InitializeVitamins();
            vitaminsCanvas.gameObject.SetActive(true);
            vitaminsCanvas.interactable = true;
            activeCanvas = vitaminsCanvas;
        }

        else if (PawzzleType == "HEALTHYPLATE")
        {
            InitializeHealthyPlate();
            healthyPlateCanvas.gameObject.SetActive(true);
            healthyPlateCanvas.interactable = true;
            activeCanvas = healthyPlateCanvas;
        }

        else if (PawzzleType == "BLINDSPOTS")
        {
            InitializeBlindSpots();
            blindSpotsCanvas.gameObject.SetActive(true);
            blindSpotsCanvas.interactable = true;
            activeCanvas = blindSpotsCanvas;
        }

        else if (PawzzleType == "APORKALYPSE")
        {
            InitializeAporkalypse();
            aporkalypseCanvas.gameObject.SetActive(true);
            aporkalypseCanvas.interactable = true;
            activeCanvas = aporkalypseCanvas;
        }
    }

    void InitializeF5()
    {
        //Picks a random value of 0 or 1
        int validRng = Random.Range(0, 2);

        //Decides the image and food of both F5 images
        int rng = Random.Range(0, validF5Images.Length - 1);
        int rng2 = Random.Range(0, invalidF5Images.Length - 1);

        //if rng was 0, valid food shown is on the left
        if (validRng == 0)
        {
            f5Image1.sprite = validF5Images[rng];

            //Make sure the invalid food does not match the valid food
            do
            {
                rng2 = Random.Range(0, invalidF5Images.Length - 1);
            }
            while (rng2 == rng);

            f5Image2.sprite = invalidF5Images[rng2];
        }

        //else, valid food is on the right
        else if (validRng == 1)
        {
            f5Image2.sprite = validF5Images[rng];

            //Make sure the invalid food does not match the valid food
            do
            {
                rng2 = Random.Range(0, invalidF5Images.Length - 1);
            }
            while (rng2 == rng);

            f5Image1.sprite = invalidF5Images[rng2];
        }

        AssignF5Adjective();
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

        else if (leftOrRightTable == 1)
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
                vitaminImage1.sprite = vitaminImages[rng]; ;
            }

            else if (i == 1)
            {
                vitaminFood2 = vitaminFoods[rng];
                vitaminImage2.sprite = vitaminImages[rng];
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

    void InitializeHealthyPlate()
    {
        //First, categorize all the foods into ther respective category
        fruitAndVegetables.Add(healthyPlateFoods[2]); //Add tomato
        fruitAndVegetables.Add(healthyPlateFoods[5]); //Add lemon
        fruitAndVegetables.Add(healthyPlateFoods[6]); //Add broccoli
        fruitAndVegetables.Add(healthyPlateFoods[8]); //Add carrot

        meatAndOthers.Add(healthyPlateFoods[0]); //Add milk
        meatAndOthers.Add(healthyPlateFoods[3]); //Add bacon
        meatAndOthers.Add(healthyPlateFoods[7]); //Add steak
        meatAndOthers.Add(healthyPlateFoods[9]); //Add fish

        grains.Add(healthyPlateFoods[1]); //Add bread
        grains.Add(healthyPlateFoods[4]); //Add barley
        grains.Add(healthyPlateFoods[10]); //Add cereal

        assignableFoods.Clear();

        //Then, pick a random category to be the unassigned category
        string[] categories = { "FRUIT & VEGETABLES", "MEAT & OTHERS", "GRAINS" };

        unassignedCategory = categories[Random.Range(0, categories.Length)];

        //Based on the unassigned category, add in elements into a list that holds the possible foods called assignableFood
        if (unassignedCategory == "FRUIT & VEGETABLES")
        {
            assignableFoods.AddRange(meatAndOthers);
            assignableFoods.AddRange(grains);
        }

        //Add non-meat&others
        else if (unassignedCategory == "MEAT & OTHERS")
        {
            assignableFoods.AddRange(fruitAndVegetables);
            assignableFoods.AddRange(grains);
        }

        //Add non-grains
        else if (unassignedCategory == "GRAINS")
        {
            assignableFoods.AddRange(fruitAndVegetables);
            assignableFoods.AddRange(meatAndOthers);
        }

        //Assign the static food to a random assignableFood
        int randomNum = Random.Range(0, assignableFoods.Count);
        staticFood = assignableFoods[randomNum];
        staticFoodImage.sprite = healthyPlateSprites[healthyPlateFoods.IndexOf(assignableFoods[randomNum])];
        assignableFoods.Remove(staticFood);

        //Assign the selectionRangeFoods, including the isolatedFood
        bool isIsolatedAssigned = false;
        for (int i = 0; i < 3; i++)
        {
            //Assign the isolated food
            if (!isIsolatedAssigned)
            {
                if (unassignedCategory == "FRUIT & VEGETABLES")
                {
                    if (meatAndOthers.Contains(staticFood))
                    {
                        int randomNo = Random.Range(0, grains.Count);
                        isolatedFood = grains[randomNo];
                        selectionRangeFoods.Add(isolatedFood);
                    }

                    else if (grains.Contains(staticFood))
                    {
                        int randomNo = Random.Range(0, meatAndOthers.Count);
                        isolatedFood = meatAndOthers[randomNo];
                        selectionRangeFoods.Add(meatAndOthers[randomNo]);
                    }
                }

                else if (unassignedCategory == "MEAT & OTHERS")
                {
                    if (fruitAndVegetables.Contains(staticFood))
                    {
                        int randomNo = Random.Range(0, grains.Count);
                        isolatedFood = grains[randomNo];
                        selectionRangeFoods.Add(grains[randomNo]);
                    }

                    else if (grains.Contains(staticFood))
                    {
                        int randomNo = Random.Range(0, fruitAndVegetables.Count);
                        isolatedFood = fruitAndVegetables[randomNo];
                        selectionRangeFoods.Add(fruitAndVegetables[randomNo]);
                    }
                }

                else if (unassignedCategory == "GRAINS")
                {
                    if (meatAndOthers.Contains(staticFood))
                    {
                        int randomNo = Random.Range(0, fruitAndVegetables.Count);
                        isolatedFood = fruitAndVegetables[randomNo];
                        selectionRangeFoods.Add(fruitAndVegetables[randomNo]);
                    }

                    else if (fruitAndVegetables.Contains(staticFood))
                    {
                        int randomNo = Random.Range(0, meatAndOthers.Count);
                        isolatedFood = meatAndOthers[randomNo];
                        selectionRangeFoods.Add(meatAndOthers[randomNo]);
                    }
                }
                assignableFoods.Remove(isolatedFood);
                isolatedFoodValue = AssignHealthyPlateFoodValue(isolatedFood);

                isIsolatedAssigned = true;
            }

            else
            {
                //if isolated = fruit&veg
                if (fruitAndVegetables.Contains(isolatedFood))
                {
                    if (unassignedCategory == "MEAT & OTHERS")  //Assign to grains
                    {
                        string temp1 = grains[Random.Range(0, grains.Count)];
                        do
                        {
                            int rng = Random.Range(0, grains.Count);
                            temp1 = grains[rng];
                        }
                        while (selectionRangeFoods.Contains(temp1));

                        selectionRangeFoods.Add(temp1);
                    }

                    if (unassignedCategory == "GRAINS") //Assign to meat&others
                    {
                        string temp1 = meatAndOthers[Random.Range(0, meatAndOthers.Count)];
                        do
                        {
                            int rng = Random.Range(0, meatAndOthers.Count);
                            temp1 = meatAndOthers[rng];
                        }
                        while (selectionRangeFoods.Contains(temp1));

                        selectionRangeFoods.Add(temp1);
                    }
                }

                //if isolated = meat&others
                if (meatAndOthers.Contains(isolatedFood))
                {
                    if (unassignedCategory == "FRUIT & VEGETABLES") //Assign to grains
                    {
                        string temp1 = grains[Random.Range(0, grains.Count)];
                        do
                        {
                            int rng = Random.Range(0, grains.Count);
                            temp1 = grains[rng];
                        }
                        while (selectionRangeFoods.Contains(temp1));

                        selectionRangeFoods.Add(temp1);
                    }

                    if (unassignedCategory == "GRAINS") //Assign to fruit&veg
                    {
                        string temp1 = fruitAndVegetables[Random.Range(0, fruitAndVegetables.Count)];
                        do
                        {
                            int rng = Random.Range(0, fruitAndVegetables.Count);
                            temp1 = fruitAndVegetables[rng];
                        }
                        while (selectionRangeFoods.Contains(temp1));

                        selectionRangeFoods.Add(temp1);
                    }
                }

                //if isolated = grains
                if (grains.Contains(isolatedFood))
                {
                    if (unassignedCategory == "MEAT & OTHERS") //Assign to fruit&veg
                    {
                        string temp1 = fruitAndVegetables[Random.Range(0, fruitAndVegetables.Count)];
                        do
                        {
                            int rng = Random.Range(0, fruitAndVegetables.Count);
                            temp1 = fruitAndVegetables[rng];
                        }
                        while (selectionRangeFoods.Contains(temp1));

                        selectionRangeFoods.Add(temp1);
                    }

                    if (unassignedCategory == "FRUIT & VEGETABLES") //Assign to meat&others
                    {
                        string temp1 = meatAndOthers[Random.Range(0, meatAndOthers.Count)];
                        do
                        {
                            int rng = Random.Range(0, meatAndOthers.Count);
                            temp1 = meatAndOthers[rng];
                        }
                        while (selectionRangeFoods.Contains(temp1));

                        selectionRangeFoods.Add(temp1);
                    }
                }
            }
        }
        //Shuffle the selectionRangeFoods
        Shuffle(selectionRangeFoods);
        for (int i = 0; i < selectionRangeFoods.Count; i++)
        {
            selectionRangeImages.Add(healthyPlateSprites[healthyPlateFoods.IndexOf(selectionRangeFoods[i])]);
            selectionRangeValues.Add(AssignHealthyPlateFoodValue(selectionRangeFoods[i]));
        }

        //Default the first display image of the selection range to the first selectionRange food
        selectionRangeImage.sprite = selectionRangeImages[0];

        //Assign the unknown food
        if (unassignedCategory == "FRUIT & VEGETABLES")
        {
            if (meatAndOthers.Contains(staticFood))
            {
                unknownFood = "CARROT";
            }

            else if (grains.Contains(staticFood))
            {
                unknownFood = "TOMATO";
            }
        }

        else if (unassignedCategory == "MEAT & OTHERS")
        {
            if (fruitAndVegetables.Contains(staticFood))
            {
                unknownFood = "BACON";
            }

            else if (grains.Contains(staticFood))
            {
                unknownFood = "STEAK";
            }
        }

        else if (unassignedCategory == "GRAINS")
        {
            if (fruitAndVegetables.Contains(staticFood))
            {
                unknownFood = "BARLEY";
            }

            else if (meatAndOthers.Contains(staticFood))
            {
                unknownFood = "BREAD";
            }
        }

        //Assign the key food by finding the diff between unknown food and isolated food
        int unknownFoodValue = AssignHealthyPlateFoodValue(unknownFood);
        int random = Random.Range(0, selectionRangeFoods.Count);

        int keyFoodValue;

        if (unknownFoodValue > isolatedFoodValue)
        {
            keyFoodValue = unknownFoodValue - isolatedFoodValue;
            AssignHealthyPlateAdjective(keyFoodValue);
        }

        else if (unknownFoodValue < isolatedFoodValue)
        {
            keyFoodValue = isolatedFoodValue - unknownFoodValue;
            AssignHealthyPlateAdjective(keyFoodValue);
        }
    }

    void InitializeBlindSpots()
    {
        blindSpotsFood = blindSpotsFoods[Random.Range(0, blindSpotsFoods.Count)];

        CreateSoundSpots();
    }

    void InitializeAporkalypse()
    {
        //Set the label to a random text and color
        labelText.text = possibleLabels[Random.Range(0, possibleLabels.Count)];
        labelText.color = labelColors[Random.Range(0, labelColors.Length)];

        int rng1 = Random.Range(0, 2);  //Used for deciding bolding
        int rng2 = Random.Range(0, 2);  //Used for deciding underline

        //Randomize if the text is italic and/or underlined
        if (rng1 == 1 && rng2 == 0)
        {
            labelText.fontStyle = FontStyles.Italic;
        }

        else if (rng1 == 0 && rng2 == 1)
        {
            labelText.fontStyle = FontStyles.Underline;
        }

        else if (rng1 == 1 && rng2 == 1)
        {
            labelText.fontStyle = FontStyles.Italic | FontStyles.Underline;
        }

        AssignAporkalypseDate(rng1, rng2);
        AssignAporkalypseCut();
    }
    public void SubmitInput()  //Player presses the submit button for primary pawzzle
    {
        bool isSolved = false;
        for (int i = 0; i < apparatusNouns.Count; i++)
        {
            if (primaryInput.text == currentPawzzleAdjective + " " + apparatusNouns[i]) //If correct
            {
                if (apparatusNouns[i] == knifeNoun)  //Solving for knife
                {
                    SolvePrimaryPawzzle("KNIFE");
                }

                if (apparatusNouns[i] == spatulaNoun) //Solving for spatula
                {
                    SolvePrimaryPawzzle("SPATULA");
                }

                if (apparatusNouns[i] == strainerNoun)//Solving for strainer
                {
                    SolvePrimaryPawzzle("STRAINER");
                }

                if (apparatusNouns[i] == pestleNoun)//Solving for pestle
                {
                    SolvePrimaryPawzzle("PESTLE");
                }

                if (currentPawzzleType == "BLINDSPOTS")
                {
                    for (int n = 0; i < soundSpots.Count; i++)
                    {
                        Destroy(soundSpots[n]);
                    }

                    soundSpots.Clear();
                }

                isSolved = true;
                inputPanel.SetActive(false);

                currentApparatusLabel.text = GameManagerScript.instance.accessedApparatus;
                accessedPanel.SetActive(true);
                break;
            }

            if (primaryInput.text != currentPawzzleAdjective + " " + apparatusNouns[i] && i == apparatusNouns.Count - 1 && !isSolved)  //If wrong
            {
                GameManagerScript.instance.ErrorMade();
                break;
            }
        }
    }

    //Assign the accessed apparatus from the pawzzle and extrude it out (animation) from the meow-ti tool
    void SolvePrimaryPawzzle(string desiredApparatus)
    {
        GameManagerScript.instance.accessedApparatus = desiredApparatus;
        //Debug.Log("Pawzzle solved! You accessed the " + desiredApparatus + ".");

        if (desiredApparatus == "KNIFE")
        {
            knifeHead.SetActive(true);
            knifeAnimator.SetTrigger("Extrude");
        }

        if (desiredApparatus == "SPATULA")
        {
            spatulaHead.SetActive(true);
            spatulaAnimator.SetTrigger("Extrude");
        }

        if (desiredApparatus == "STRAINER")
        {
            strainerHead.SetActive(true);
            strainerAnimator.SetTrigger("Extrude");
        }

        if (desiredApparatus == "PESTLE")
        {
            pestleHead.SetActive(true);
            pestleAnimator.SetTrigger("Extrude");
        }
    }

    public void StartTyping()
    {
        isTyping = true;
    }

    public void StopTyping()
    {
        isTyping = false;
    }


    //Reset the apparatus, change the MTT canvas, play revert anim, then generate another pawzzle
    //This method is called from Revert Button in Input and Access (in Meow-ti Tool gameObject)
    public void RevertApparatus()
    {
        StartCoroutine(PlayRevertAnim());

        GameManagerScript.instance.accessedApparatus = null;
        primaryInput.text = null;
        activeCanvas.gameObject.SetActive(false);
        accessedPanel.SetActive(false);
        inputPanel.SetActive(true);

        GeneratePawzzle();
        InitializePawzzle(currentPawzzleType);  //Initialize the generated pawzzle
    }

    IEnumerator PlayRevertAnim()
    {
        if (GameManagerScript.instance.accessedApparatus == "KNIFE")
        {
            knifeAnimator.SetTrigger("Revert");
            yield return new WaitForSeconds(1f);
            knifeHead.SetActive(false);
        }

        if (GameManagerScript.instance.accessedApparatus == "SPATULA")
        {
            spatulaAnimator.SetTrigger("Revert");
            yield return new WaitForSeconds(1f);
            spatulaHead.SetActive(false);
        }

        if (GameManagerScript.instance.accessedApparatus == "STRAINER")
        {
            strainerAnimator.SetTrigger("Revert");
            yield return new WaitForSeconds(1f);
            strainerHead.SetActive(false);
        }

        if (GameManagerScript.instance.accessedApparatus == "PESTLE")
        {
            pestleAnimator.SetTrigger("Revert");
            yield return new WaitForSeconds(1f);
            pestleHead.SetActive(false);
        }
    }

    //Pawzzle-specific button methods below
    //F5
    void AssignF5Adjective()
    {
        //If valid food was...

        //Zucchini
        if (f5Image1.sprite == validF5Images[0] || f5Image2.sprite == validF5Images[0])
        {
            currentPawzzleAdjective = "BLAND";
        }

        //Shallot
        if (f5Image1.sprite == validF5Images[1] || f5Image2.sprite == validF5Images[1])
        {
            currentPawzzleAdjective = "BLANCHED";
        }

        //Peach
        if (f5Image1.sprite == validF5Images[2] || f5Image2.sprite == validF5Images[2])
        {
            currentPawzzleAdjective = "CLASSY";
        }

        //Bell Pepper
        if (f5Image1.sprite == validF5Images[3] || f5Image2.sprite == validF5Images[3])
        {
            currentPawzzleAdjective = "LONELY";
        }

        //Durian
        if (f5Image1.sprite == validF5Images[4] || f5Image2.sprite == validF5Images[4])
        {
            currentPawzzleAdjective = "BROILED";
        }
    }

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

        yield return new WaitForSeconds(radioAudioClipToPlay.length);
        isClipPlaying = false;
        radioCafeText.text = "PLAY AUDIO";
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

        else if ((vitaminFood1 == "BELL PEPPER" && vitaminFood2 == "BROCCOLI") || (vitaminFood1 == "BROCCOLI" && vitaminFood2 == "BELL PEPPER"))  //Bs and E present in both foods
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

    bool CheckVitamins(string vitamin, int numberOfChecks)
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


    //HEALTHYPLATE
    int AssignHealthyPlateFoodValue(string input)
    {
        //                   Food Values   7        4         2        5        1         10        9          3         6        8       11
        //string[] healthyPlateFoods = { "MILK", "BREAD", "TOMATO", "BACON", "BARLEY", "LEMON", "BROCCOLI", "STEAK", "CARROT", "FISH", "CEREAL" }; //In order of appearance in recipe book

        //Assigning values
        if (input == "MILK")
        {
            return 7;
        }

        else if (input == "BREAD")
        {
            return 4;
        }

        else if (input == "TOMATO")
        {
            return 2;
        }

        else if (input == "BACON")
        {
            return 5;
        }

        else if (input == "BARLEY")
        {
            return 1;
        }

        else if (input == "LEMON")
        {
            return 10;
        }

        else if (input == "BROCCOLI")
        {
            return 9;
        }

        else if (input == "STEAK")
        {
            return 3;
        }

        else if (input == "CARROT")
        {
            return 6;
        }

        else if (input == "FISH")
        {
            return 8;
        }

        else
        {
            return 0;
        }
    }
    public void ChangeSelectionRangeFoodUp()
    {
        imageIndex++;

        if (imageIndex > selectionRangeImages.Count - 1)
        {
            imageIndex = 0;
        }

        selectionRangeImage.sprite = selectionRangeImages[imageIndex];
    }

    public void ChangeSelectionRangeFoodDown()
    {
        imageIndex--;

        if (imageIndex < 0)
        {
            imageIndex = selectionRangeImages.Count - 1;
        }

        selectionRangeImage.sprite = selectionRangeImages[imageIndex];
    }

    void AssignHealthyPlateAdjective(int keyFoodValue)
    {
        if (keyFoodValue == 7)  //MILK
        {
            currentPawzzleAdjective = "ECSTATIC";
        }

        else if (keyFoodValue == 4)
        {
            currentPawzzleAdjective = "JOYOUS";
        }

        else if (keyFoodValue == 2)
        {
            currentPawzzleAdjective = "CHEERFUL";
        }

        else if (keyFoodValue == 5)
        {
            currentPawzzleAdjective = "POSITIVE";
        }

        else if (keyFoodValue == 1)
        {
            currentPawzzleAdjective = "INDIFFERENT";
        }

        else if (keyFoodValue == 10)
        {
            currentPawzzleAdjective = "NEUTRAL";
        }

        else if (keyFoodValue == 9)
        {
            currentPawzzleAdjective = "NEGATIVE";
        }

        else if (keyFoodValue == 3)
        {
            currentPawzzleAdjective = "SAD";
        }

        else if (keyFoodValue == 6)
        {
            currentPawzzleAdjective = "MISERABLE";
        }

        else if (keyFoodValue == 8)
        {
            currentPawzzleAdjective = "DEPRESSED";
        }
    }

    void Shuffle<T>(List<T> inputList)
    {
        for (int i = 0; i < inputList.Count; i++)
        {
            T temp = inputList[i];
            int rand = Random.Range(0, inputList.Count);
            inputList[i] = inputList[rand];
            inputList[rand] = temp;
        }
    }


    //BLIND SPOTS
    public void CreateSoundSpots()
    {
        //stirFry = 0, heavyRain = 1, boiling = 2, deepFry = 3
        if (blindSpotsFood == "CARROT")
        {
            GenerateSoundClip("heavyRain", blindSpotsClips[1], false);
            GenerateSoundClip("deepFrying", blindSpotsClips[3], false);
            currentPawzzleAdjective = "GRATED";
        }

        else if (blindSpotsFood == "SALMON")
        {
            GenerateSoundClip("heavyRain", blindSpotsClips[1], true);
            GenerateSoundClip("deepFrying", blindSpotsClips[3], false);
            currentPawzzleAdjective = "UMAMI";
        }

        else if (blindSpotsFood == "CHICKEN")
        {
            GenerateSoundClip("heavyRain", blindSpotsClips[1], true);
            GenerateSoundClip("boiling", blindSpotsClips[2], false);
            currentPawzzleAdjective = "TENDER";
        }

        else if (blindSpotsFood == "BEEF")
        {
            GenerateSoundClip("heavyRain", blindSpotsClips[1], true);
            GenerateSoundClip("boiling", blindSpotsClips[2], true);
            currentPawzzleAdjective = "ICKY";
        }

        else if (blindSpotsFood == "CAULIFLOWER")
        {
            GenerateSoundClip("stirFrying", blindSpotsClips[0], false);
            GenerateSoundClip("deepFrying", blindSpotsClips[3], false);
            GenerateSoundClip("boiling", blindSpotsClips[2], false);
            currentPawzzleAdjective = "BITTERSWEET";
        }

        else if (blindSpotsFood == "EGGS")
        {
            GenerateSoundClip("stirFrying", blindSpotsClips[0], true);
            GenerateSoundClip("deepFrying", blindSpotsClips[3], false);
            GenerateSoundClip("boiling", blindSpotsClips[2], false);
            currentPawzzleAdjective = "COMPRESSED";
        }

        else if (blindSpotsFood == "SPINACH")
        {
            GenerateSoundClip("stirFrying", blindSpotsClips[0], false);
            GenerateSoundClip("heavyRain", blindSpotsClips[1], false);
            GenerateSoundClip("deepFrying", blindSpotsClips[3], true);
            currentPawzzleAdjective = "BLAND";
        }
    }

    void GenerateSoundClip(string clipName, AudioClip audioClip, bool isLeft)
    {
        BlindSpotsSound soundSpot = Instantiate(soundSpotObj, blindSpotsPanel.transform).GetComponent<BlindSpotsSound>();
        soundSpot.soundName = clipName;
        soundSpot.soundClip = audioClip;
        soundSpot.isLeft = isLeft;

        bool isFarEnoughAway = false;

        do
        {
            if (soundSpot.isLeft)
            {
                soundSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(-400, -1600), Random.Range(-600, 600));
            }

            else if (!soundSpot.isLeft)
            {
                soundSpot.GetComponent<RectTransform>().anchoredPosition = new Vector2(Random.Range(400, 1600), Random.Range(-600, 600));
            }

            soundSpots.Add(soundSpot.gameObject);

            if (soundSpots.Count == 1)
            {
                isFarEnoughAway = true;
            }

            if (soundSpots.Count > 1)
            {
                for (int i = 0; i < soundSpots.Count; i++)
                {
                    if (Vector2.Distance(soundSpot.GetComponent<RectTransform>().anchoredPosition, soundSpots[i].GetComponent<RectTransform>().anchoredPosition) > 350)
                    {
                        isFarEnoughAway = true;
                    }
                }
            }
        }
        while (!isFarEnoughAway);
    }

    void AssignAporkalypseDate(int italic, int underline)
    {
        //if label color is green
        if (labelText.color == labelColors[0])
        {
            if (labelText.text == possibleLabels[0])  //green prediction
            {
                SetAporkalypseDate(17, 7);
            }

            else if (labelText.text == possibleLabels[1]) //green signal
            {
                SetAporkalypseDate(31, 1);
            }

            else if (labelText.text == possibleLabels[2]) //green premonition
            {
                SetAporkalypseDate(22, 1);
            }

            else if (labelText.text == possibleLabels[3]) //green sign
            {
                SetAporkalypseDate(13, 3);
            }

            else if (labelText.text == possibleLabels[4]) //green prophecy
            {
                SetAporkalypseDate(2, 9);
            }

            else if (labelText.text == possibleLabels[5]) //green indication
            {
                SetAporkalypseDate(15, 4);
            }
        }

        //if label color is red
        if (labelText.color == labelColors[1])
        {
            if (labelText.text == possibleLabels[0])  //red prediction
            {
                SetAporkalypseDate(21, 11);
            }

            else if (labelText.text == possibleLabels[1]) //red signal
            {
                SetAporkalypseDate(5, 8);
            }

            else if (labelText.text == possibleLabels[2]) //red premonition
            {
                SetAporkalypseDate(23, 6);
            }

            else if (labelText.text == possibleLabels[3]) //red sign
            {
                SetAporkalypseDate(22, 2);
            }

            else if (labelText.text == possibleLabels[4]) //red prophecy
            {
                SetAporkalypseDate(27, 8);
            }

            else if (labelText.text == possibleLabels[5]) //red indication
            {
                SetAporkalypseDate(14, 2);
            }
        }

        //if label color is blue
        if (labelText.color == labelColors[3])
        {
            if (labelText.text == possibleLabels[0])  //blue prediction
            {
                SetAporkalypseDate(31, 3);
            }

            else if (labelText.text == possibleLabels[1]) //blue signal
            {
                SetAporkalypseDate(16, 5);
            }

            else if (labelText.text == possibleLabels[2]) //blue premonition
            {
                SetAporkalypseDate(9, 11);
            }

            else if (labelText.text == possibleLabels[3]) //blue sign
            {
                SetAporkalypseDate(27, 7);
            }

            else if (labelText.text == possibleLabels[4]) //blue prophecy
            {
                SetAporkalypseDate(20, 12);
            }

            else if (labelText.text == possibleLabels[5]) //blue indication
            {
                SetAporkalypseDate(7, 7);
            }
        }

        //if label color is yellow
        if (labelText.color == labelColors[3])
        {
            if (labelText.text == possibleLabels[0])  //yellow prediction
            {
                SetAporkalypseDate(15, 9);
            }

            else if (labelText.text == possibleLabels[1]) //yellow signal
            {
                SetAporkalypseDate(1, 4);
            }

            else if (labelText.text == possibleLabels[2]) //yellow premonition
            {
                SetAporkalypseDate(31, 12);
            }

            else if (labelText.text == possibleLabels[3]) //yellow sign
            {
                SetAporkalypseDate(2, 2);
            }

            else if (labelText.text == possibleLabels[4]) //yellow prophecy
            {
                SetAporkalypseDate(1, 5);
            }

            else if (labelText.text == possibleLabels[5]) //yellow indication
            {
                SetAporkalypseDate(21, 10);
            }
        }

        //If the text is italic...
        if (italic == 1)
        {
            int temp;
            temp = dateValues[1]; //store ones number of day in a temporary int
            dateValues[1] = dateValues[2]; //swap the month number and the ones digit 
            dateValues[2] = temp;
        }

        //If the text is underlined...
        if (underline == 1)
        {
            int temp;
            temp = dateValues[0]; //store tens number of day in a temporary int
            dateValues[0] = dateValues[1]; //swap the tens and ones digit in day number
            dateValues[1] = temp;

            if (dateValues[0] * 10 + dateValues[1] > 31)  //Defaulting the date to 31st of the month (total days in a month doesn't matter anyway)
            {
                dateValues[0] = 3;
                dateValues[1] = 1;
            }
        }
    }

    void SetAporkalypseDate(int day, int month)
    {
        dateValues.Add((day - (day % 10)) / 10);
        dateValues.Add(day % 10);
        dateValues.Add(month);
    }

    void AssignAporkalypseCut()
    {
        int dayNum = dateValues[0] * 10 + dateValues[1];
        if (dayNum <= 7) //1st week
        {
            if (dateValues[2] <= 3) //jan to mar
            {
                porkCut = "UPPER HEAD";
            }

            if (dateValues[2] > 3 && dateValues[2] <= 6) //apr to jun
            {
                porkCut = "NECK";
            }

            if (dateValues[2] > 6 && dateValues[2] <= 9) //jul to sep
            {
                porkCut = "LOIN";
            }

            if (dateValues[2] > 9) //oct to dec
            {
                porkCut = "UPPER HAM";
            }
        }

        if (dayNum > 7 && dayNum <= 14) //2nd week
        {
            if (dateValues[2] <= 3) //jan to mar
            {
                porkCut = "HEAD";
            }

            if (dateValues[2] > 3 && dateValues[2] <= 6) //apr to jun
            {
                porkCut = "SHOULDER";
            }

            if (dateValues[2] > 6 && dateValues[2] <= 9) //jul to sep
            {
                porkCut = "NECK";
            }

            if (dateValues[2] > 9) //oct to dec
            {
                porkCut = "HAM";
            }
        }

        if (dayNum > 14 && dayNum <= 21) //3rd week
        {
            if (dateValues[2] <= 3) //jan to mar
            {
                porkCut = "LOWER HEAD";
            }

            if (dateValues[2] > 3 && dateValues[2] <= 6) //apr to jun
            {
                porkCut = "BLADE";
            }

            if (dateValues[2] > 6 && dateValues[2] <= 9) //jul to sep
            {
                porkCut = "BACON";
            }

            if (dateValues[2] > 9) //oct to dec
            {
                porkCut = "LOWER HAM";
            }
        }

        if (dayNum > 21) //4th+ week
        {
            if (dateValues[2] <= 3) //jan to mar
            {
                porkCut = "JOWL";
            }

            if (dateValues[2] > 3 && dateValues[2] <= 6) //apr to jun
            {
                porkCut = "FRONT HOCK";
            }

            if (dateValues[2] > 6 && dateValues[2] <= 9) //jul to sep
            {
                porkCut = "BELLY";
            }

            if (dateValues[2] > 9) //oct to dec
            {
                porkCut = "HIND HOCK";
            }
        }

        if (porkCut == "LOIN" || porkCut == "HIND HOCK" || porkCut == "LOWER HEAD" || porkCut == "BLADE" || porkCut == "NECK" || porkCut == "UPPER HAM")
        {
            omenType = "GOOD";
        }

        else if (porkCut == "BACON" || porkCut == "HAM" || porkCut == "BELLY" || porkCut == "HIND HOCK")
        {
            omenType = "NO";
        }

        else if (porkCut == "UPPER HEAD" || porkCut == "JOWL" || porkCut == "SHOULDER" || porkCut == "FRONT HOCK" || porkCut == "RIBS" || porkCut == "LOWER HAM")
        {
            omenType = "BAD";
        }
    }

    public void OmenButtonPressed(string buttonType)
    {
        if (buttonType != omenType)
        {
            GameManagerScript.instance.ErrorMade();
        }

        else if (buttonType == omenType)
        {
            reply = aporkalypseReplies[Random.Range(0, aporkalypseReplies.Count)];
            replyField.text = reply;
            AssignAporkalypseAdjective();
        }
    }

    void AssignAporkalypseAdjective()
    {
        //GOOD OMEN
        if (omenType == "GOOD")
        {
            if (reply == "ERRR")
            {
                currentPawzzleAdjective = "KNOWING";
            }

            else if (reply == "MMHMM")
            {
                currentPawzzleAdjective = "AMAZING";
            }

            else if (reply == "UHHHHH")
            {
                currentPawzzleAdjective = "TRUTHFUL";
            }

            else if (reply == "UHH")
            {
                currentPawzzleAdjective = "HONEST";
            }

            else if (reply == "ER")
            {
                currentPawzzleAdjective = "LAWFUL";
            }

            else if (reply == "UM")
            {
                currentPawzzleAdjective = "BEST";
            }

            else if (reply == "UHM")
            {
                currentPawzzleAdjective = "FAST";
            }

            else if (reply == "MMHM")
            {
                currentPawzzleAdjective = "SHALLOW";
            }

            else if (reply == "UH UH")
            {
                currentPawzzleAdjective = "RATIONAL";
            }

            else if (reply == "MHMM")
            {
                currentPawzzleAdjective = "CALM";
            }
        }

        //NO OMEN
        else if (omenType == "NO")
        {
            if (reply == "ERRR")
            {
                currentPawzzleAdjective = "UNSURE";
            }

            else if (reply == "MMHMM")
            {
                currentPawzzleAdjective = "MEDIOCRE";
            }

            else if (reply == "UHHHHH")
            {
                currentPawzzleAdjective = "DOUBTFUL";
            }

            else if (reply == "UHH")
            {
                currentPawzzleAdjective = "SECRETIVE";
            }

            else if (reply == "ER")
            {
                currentPawzzleAdjective = "NEUTRAL";
            }

            else if (reply == "UM")
            {
                currentPawzzleAdjective = "DECENT";
            }

            else if (reply == "UHM")
            {
                currentPawzzleAdjective = "AVERAGE";
            }

            else if (reply == "MMHM")
            {
                currentPawzzleAdjective = "DEEP";
            }

            else if (reply == "UH UH")
            {
                currentPawzzleAdjective = "COLLECTED";
            }

            else if (reply == "MHMM")
            {
                currentPawzzleAdjective = "FREE";
            }
        }

        //BAD OMEN
        else if (omenType == "BAD")
        {
            if (reply == "ERRR")
            {
                currentPawzzleAdjective = "UNAWARE";
            }

            else if (reply == "MMHMM")
            {
                currentPawzzleAdjective = "HORRIBLE";
            }

            else if (reply == "UHHHHH")
            {
                currentPawzzleAdjective = "LYING";
            }

            else if (reply == "UHH")
            {
                currentPawzzleAdjective = "DECEITFUL";
            }

            else if (reply == "ER")
            {
                currentPawzzleAdjective = "CHAOTIC";
            }

            else if (reply == "UM")
            {
                currentPawzzleAdjective = "WORST";
            }

            else if (reply == "UHM")
            {
                currentPawzzleAdjective = "SLUGGISH";
            }

            else if (reply == "MMHM")
            {
                currentPawzzleAdjective = "BOTTOMLESS";
            }

            else if (reply == "UH UH")
            {
                currentPawzzleAdjective = "BERZERK";
            }

            else if (reply == "MHMM")
            {
                currentPawzzleAdjective = "HYSTERICAL";
            }
        }
    }
}
