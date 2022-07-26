using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiceCooker : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip beepA;
    public AudioClip beepB;
    public AudioClip wrongSfx;

    Container container;
    Interactable interactable;
    public ProgressBar progressBar;
    public Image progressFill;
    public Image lightIndicator;

    public Light cookerLight;

    public GameObject cookingLid;
    GameObject spawnedLid;
    public GameObject cookerPanel;
    public GameObject cookerButton;
    public GameObject cookedRiceObj;
    public GameObject uncookedRiceObj;
    public GameObject waterObj;

    public bool hasFlashed = false;
    bool isCooking = false;
    bool isButtonPressed = false;
    bool isLidMoving = false;
    bool isBarFlashing = false;
    bool isIndicatorFlashing = false;

    public Transform lidSpawnPoint;
    public Transform lidTranslationPoint;
    Transform translationPoint;

    public Color32 indicatorNormalCol;
    public Color32 indicatorFlashCol;

    string buttonStatus;
    string correctOrWrong;
    public string procedure;
    public string[] procedures;

    public float buttonPressedDur = 0;
    float cookingDuration = 0;
    public float cookingThreshold;

    Animator lidAnim;

    // Start is called before the first frame update
    void Start()
    {
        container = GetComponent<Container>();
        interactable = GetComponent<Interactable>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isButtonPressed)
        {
            buttonPressedDur += Time.deltaTime;
        }

        if (isLidMoving)
        {
            MoveLid();
        }

        if (container.itemContained != null && container.itemContained.GetComponent<Interactable>().objectName == "cookerPot" && !GameManagerScript.instance.orders.nasiLemakPrep.isPotInCooker)
        {
            GameManagerScript.instance.orders.nasiLemakPrep.isPotInCooker = true;

            if (GameManagerScript.instance.orders.nasiLemakPrep.isPotFilledWithWater)
            {
                container.itemContained.GetComponent<Interactable>().isPickup = false;
            }

            interactable.isPreparable = true;
        }

        if (isCooking)
        {
            cookingDuration += Time.deltaTime;
            progressBar.AddProgress(Time.deltaTime);

            if (!hasFlashed)
            {
                cookingDuration += Time.deltaTime * 2;
                progressBar.AddProgress(Time.deltaTime * 2);
            }

            if (progressBar.slider.value >= 14 && !isBarFlashing)
            {
                StartCoroutine(FlashBar());
            }

            if (cookingDuration > cookingThreshold)
            {
                GameManagerScript.instance.orders.dishQualityBar.AddProgress(-Time.deltaTime);
            }
        }

        if (progressBar.slider.value >= progressBar.slider.maxValue && !isIndicatorFlashing)
        {
            ResetCookerBar();

            procedure = procedures[Random.Range(0, procedures.Length)];
            int rng = Random.Range(0, 2);

            if (procedure == "<2")
            {
                if (rng == 0)
                {
                    StartCoroutine(StartFlashSequence("A", "B", " "));
                    StartCoroutine(StartBeepSequence(" ", "a", "a"));
                }

                else if (rng == 1)
                {
                    StartCoroutine(StartFlashSequence("B", " ", "A"));
                    StartCoroutine(StartBeepSequence(" ", "b", " "));
                }
            }

            else if (procedure == "=3")
            {
                if (rng == 0)
                {
                    StartCoroutine(StartFlashSequence("A", "A", "A"));
                    StartCoroutine(StartBeepSequence("b", " ", "a"));
                }

                else if (rng == 1)
                {
                    StartCoroutine(StartFlashSequence("A", "B", " "));
                    StartCoroutine(StartBeepSequence(" ", "a", " "));
                }
            }

            else if (procedure == "=2")
            {
                if (rng == 0)
                {
                    StartCoroutine(StartFlashSequence("A", " ", "A"));
                    StartCoroutine(StartBeepSequence("b", " ", "a"));
                }

                else if (rng == 1)
                {
                    StartCoroutine(StartFlashSequence("B", " ", "A"));
                    StartCoroutine(StartBeepSequence(" ", "a", "a"));
                }
            }
        }
    }

    public void CookerButtonPressed()
    {
        //Start cooking sequence
        if (!isCooking)
        {
            isCooking = true;
            cookerButton.GetComponent<Interactable>().raycastAction = "Press";

            spawnedLid = Instantiate(cookingLid, lidSpawnPoint.position, Quaternion.identity);
            lidAnim = spawnedLid.GetComponent<Animator>();
            translationPoint = lidTranslationPoint;
            isLidMoving = true;
            cookerPanel.SetActive(true);
            progressBar = cookerPanel.GetComponentInChildren<ProgressBar>();
            ResetCookerBar();
            Camera.main.GetComponent<CamTransition>().MoveCamera(interactable.camPoint2);
        }

        if (isCooking)
        {
            buttonPressedDur = 0;
            isButtonPressed = true;
        }
    }

    public void CookerButtonReleased()
    {
        isButtonPressed = false;

        if (isCooking)
        {
            if (buttonPressedDur > 0.4f)
            {
                buttonStatus = "held";
            }

            else
            {
                buttonStatus = "tapped";
            }

            CheckCookerButton();

            if (correctOrWrong == "correct")
            {
                isCooking = false;
                cookedRiceObj.SetActive(true);
                waterObj.SetActive(false);
                uncookedRiceObj.SetActive(false);
                GameManagerScript.instance.orders.nasiLemakPrep.isRiceCooked = true;

                container.itemContained.GetComponent<Interactable>().isPickup = false;

                lidAnim.SetTrigger("StopCooking");
                translationPoint = lidSpawnPoint;
                isLidMoving = true;
                Camera.main.GetComponent<CamTransition>().ResetCameraTransform();
            }

            else if (correctOrWrong == "wrong")
            {
                GameManagerScript.instance.orders.dishQualityBar.AddProgress(-15f);
                GameManagerScript.instance.orders.sfxAudioSource.PlayOneShot(wrongSfx);
            }

            buttonPressedDur = 0f;
        }
    }

    //Returns "correct" if correct, "wrong" if wrong
    void CheckCookerButton()  
    {
        if (GameManagerScript.instance.orders.nasiLemakPrep.riceType == "White")
        {
            if (procedure == "<2")
            {
                if (GameManagerScript.instance.orders.isShowing1Star || GameManagerScript.instance.orders.isShowingLatest)
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "correct";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "wrong";
                    }
                }

                else
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "wrong";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "correct";
                    }
                }
            }

            else if (procedure == "=3")
            {
                if (GameManagerScript.instance.orders.isShowing3Star)
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "wrong";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "correct";
                    }
                }

                else
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "correct";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "wrong";
                    }
                }
            }

            else if (procedure == "=2")
            {
                if (GameManagerScript.instance.orders.isShowing2Star)
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "correct";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "wrong";
                    }
                }

                else
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "correct";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "wrong";
                    }
                }
            }
        }


        else if (GameManagerScript.instance.orders.nasiLemakPrep.riceType == "Brown")
        {
            if (procedure == "<2")
            {
                if (GameManagerScript.instance.orders.dishQualityBar.slider.value == GameManagerScript.instance.orders.dishQualityBar.slider.maxValue)
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "correct";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "wrong";
                    }
                }

                else
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "wrong";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "correct";
                    }
                }
            }

            else if (procedure == "=3")
            {
                if (GameManagerScript.instance.orders.dishQualityBar.slider.value < GameManagerScript.instance.orders.dishQualityBar.slider.maxValue / 2)
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "wrong";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "correct";
                    }
                }

                else
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "correct";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "wrong";
                    }
                }
            }

            else if (procedure == "=2")
            {
                if (GameManagerScript.instance.orders.dishQualityBar.slider.value < GameManagerScript.instance.orders.dishQualityBar.slider.maxValue
                    && GameManagerScript.instance.orders.dishQualityBar.slider.value > GameManagerScript.instance.orders.dishQualityBar.slider.maxValue / 2)
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "correct";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "wrong";
                    }
                }

                else
                {
                    if (buttonStatus == "tapped")
                    {
                        correctOrWrong = "wrong";
                    }

                    else if (buttonStatus == "held")
                    {
                        correctOrWrong = "correct";
                    }
                }
            }
        }
    }

    void ResetCookerBar()
    {
        progressBar.ResetProgress();
        progressBar.ResetValue();
    }

    IEnumerator FlashBar()
    {
        isBarFlashing = true;
        progressFill.color = new Color32(255, 205, 0, 255);
        yield return new WaitForSeconds(0.25f);
        progressFill.color = new Color32(248, 234, 176, 255);
        yield return new WaitForSeconds(0.25f);
        progressFill.color = new Color32(255, 205, 0, 255);
        yield return new WaitForSeconds(0.25f);
        progressFill.color = new Color32(248, 234, 176, 255);
        isBarFlashing = false;
    }

    IEnumerator StartFlashSequence(string flash1, string flash2, string flash3)
    {
        if (!hasFlashed)
        {
            hasFlashed = true;
        }
        isIndicatorFlashing = true;
        string[] flashSequence = { flash1, flash2, flash3 };
        float time = 0;
        bool wasPreviousB = false;

        for (int i = 0; i < 3; i++)
        {
            if (time >= 1.5f)
            {
                break;
            }

            else if (flashSequence[i] == "A")
            {
                cookerLight.enabled = true;
                lightIndicator.color = indicatorFlashCol;
                yield return new WaitForSeconds(0.35f);
                lightIndicator.color = indicatorNormalCol;
                cookerLight.enabled = false;
                yield return new WaitForSeconds(0.15f);
                time += 0.5f;
            }

            else if (flashSequence[i] == "B")
            {
                cookerLight.enabled = true;
                lightIndicator.color = indicatorFlashCol;
                yield return new WaitForSeconds(0.85f);
                lightIndicator.color = indicatorNormalCol;
                cookerLight.enabled = false;
                yield return new WaitForSeconds(0.15f);
                time += 1;
                wasPreviousB = true;
            }

            else if (flashSequence[i] == " " && !wasPreviousB)
            {
                yield return new WaitForSeconds(0.5f);
                time += 0.5f;
                wasPreviousB = false;
            }
        }

        isIndicatorFlashing = false;
    }


    IEnumerator StartBeepSequence(string beep1, string beep2, string beep3)
    {
        string[] beepSequence = { beep1, beep2, beep3 };
        bool wasPreviousB = false;
        float time = 0;

        for (int i = 0; i < 3; i++)
        {
            if (time >= 1.5f)
            {
                break;
            }

            else if (beepSequence[i] == "a")
            {
                audioSource.PlayOneShot(beepA);
                yield return new WaitForSeconds(0.5f);
                time += 0.5f;
            }

            else if (beepSequence[i] == "b")
            {
                audioSource.PlayOneShot(beepB);
                yield return new WaitForSeconds(1f);
                time += 1;
                wasPreviousB = true;
            }

            else if (beepSequence[i] == " " && !wasPreviousB)
            {
                yield return new WaitForSeconds(0.5f);
                time += 0.5f;
                wasPreviousB = false;
            }
        }
    }

    void MoveLid()
    {
        spawnedLid.transform.position = Vector3.Lerp(spawnedLid.transform.position, translationPoint.position, Time.deltaTime * 5);

        if (Vector3.Distance(spawnedLid.transform.position, translationPoint.position) < 0.005f)
        {
            if (!GameManagerScript.instance.orders.nasiLemakPrep.isRiceCooked)
            {
                lidAnim.SetTrigger("StartCooking");
            }

            if (GameManagerScript.instance.orders.nasiLemakPrep.isRiceCooked)
            {
                Destroy(spawnedLid);
            }

            isLidMoving = false;
        }
    }
}
