using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class phoneTexting : MonoBehaviour
{
    [Header("Camera")]
    public GameObject cam;
    public Volume worldVolume;
    DepthOfField worldDepth;
    float focalShiftSpeed = 15.0f;
    Raycast autoFocalShift;

    private Vector3 defaultPos;
    private Quaternion defaultRot;

    [Header("Phone")]
    public GameObject miniGameDisplay;
    public GameObject screen;
    public GameObject phoneCallImage;
    public GameObject backLitScreen;
    public TextMeshProUGUI phoneWordPrompt;
    public TextMeshProUGUI displayAnswer;
    string answer;
    public bool isInHand;
    public AudioClip correctAnswer;
    private AudioClip ringtone;

    [Header("Word Queue")]
    public List<string> eightLetterWords;
    List<char> letters;
    bool wordInQueue;

    //BUTTONS
    public List<Button> Buttons;
    bool aButtonPressed = false;

    void Start()
    {
        defaultPos = gameObject.transform.localPosition;
        defaultRot = gameObject.transform.localRotation;

        worldVolume.profile.TryGet(out worldDepth);
        autoFocalShift = cam.GetComponent<Raycast>();

        wordInQueue = false;

        letters = new List<char>();
        answer = "";

        ringtone = gameObject.GetComponent<AudioSource>().clip;
    }

    void Update()
    {
        if (isInHand)
        {
            startTexting();
        }

        if (Gamepad.current != null)
            playerControllerInput();
    }

    public void startTexting()
    {
        if (Gamepad.current == null)
            Cursor.lockState = CursorLockMode.None;
        MouseLook.stop = true;
        autoFocalShift.pauseFocalShift = true;

        miniGameDisplay.SetActive(true);
        screen.SetActive(true);
        phoneCallImage.SetActive(false);
        backLitScreen.SetActive(true);

        transform.position = Vector3.MoveTowards(gameObject.transform.position, cam.transform.position + (cam.transform.forward * 0.025f), 10 * Time.deltaTime);
        transform.LookAt(cam.transform.position);

        worldDepth.focalLength.value = Mathf.Lerp(worldDepth.focalLength.value, 0.1f, Time.deltaTime * focalShiftSpeed);
        worldDepth.focusDistance.value = Mathf.Lerp(worldDepth.focusDistance.value, 5.0f, Time.deltaTime * focalShiftSpeed);
        
        gameObject.GetComponent<AudioSource>().clip = correctAnswer;
        gameObject.GetComponent<AudioSource>().mute = false;
        gameObject.GetComponent<AudioSource>().loop = false;

        queueQuestion();
        checkAnswer();

        isInHand = true;
    }

    public void stopTexting()
    {
        Cursor.lockState = CursorLockMode.Locked;
        MouseLook.stop = false;
        autoFocalShift.pauseFocalShift = false;

        miniGameDisplay.SetActive(false);
        screen.SetActive(false);
        phoneCallImage.SetActive(true);
        backLitScreen.SetActive(false);

        gameObject.transform.localPosition = defaultPos;
        gameObject.transform.localRotation = defaultRot;

        worldDepth.focalLength.value = Mathf.Lerp(worldDepth.focalLength.value, 58.0f, Time.deltaTime * focalShiftSpeed);
        worldDepth.focusDistance.value = Mathf.Lerp(worldDepth.focusDistance.value, 5.0f, Time.deltaTime * focalShiftSpeed);

        gameObject.GetComponent<AudioSource>().clip = ringtone;
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<AudioSource>().mute = true;
        gameObject.GetComponent<AudioSource>().loop = true;

        isInHand = false;
    }

    void queueQuestion()
    {
        if (eightLetterWords.Count != 0 && wordInQueue == false)
        {
            phoneWordPrompt.text = eightLetterWords[Random.Range(0, eightLetterWords.Count)];

            shuffleLetters(phoneWordPrompt.text);

            wordInQueue = true;
        }
    }

    void checkAnswer()
    {
        if(answer == phoneWordPrompt.text) // they got the correct input
        {
            wordInQueue = false;
            gameObject.GetComponent<AudioSource>().Play();
        }
        else if(answer != null && answer.Length == 8)
        {
            answer = "";
            displayAnswer.text = answer;
        }
    }

    void playerControllerInput()
    {
        if (answer.Length < 8)
        {
            if (Gamepad.current.dpad.right.isPressed && Gamepad.current.dpad.up.isPressed && Gamepad.current.aButton.isPressed && aButtonPressed == false)
            {
                answer += Buttons[4].GetComponentInChildren<TextMeshProUGUI>().text;
                displayAnswer.text = answer;
                aButtonPressed = true;
            }
            else if (Gamepad.current.dpad.left.isPressed && Gamepad.current.dpad.up.isPressed && Gamepad.current.aButton.isPressed && aButtonPressed == false)
            {
                answer += Buttons[5].GetComponentInChildren<TextMeshProUGUI>().text;
                displayAnswer.text = answer;
                aButtonPressed = true;
            }
            else if (Gamepad.current.dpad.right.isPressed && Gamepad.current.dpad.down.isPressed && Gamepad.current.aButton.isPressed && aButtonPressed == false)
            {
                answer += Buttons[6].GetComponentInChildren<TextMeshProUGUI>().text;
                displayAnswer.text = answer;
                aButtonPressed = true;
            }
            else if (Gamepad.current.dpad.left.isPressed && Gamepad.current.dpad.down.isPressed && Gamepad.current.aButton.isPressed && aButtonPressed == false)
            {
                answer += Buttons[7].GetComponentInChildren<TextMeshProUGUI>().text;
                displayAnswer.text = answer;
                aButtonPressed = true;
            }
            else if (Gamepad.current.dpad.up.isPressed && Gamepad.current.aButton.isPressed && aButtonPressed == false)
            {
                answer += Buttons[0].GetComponentInChildren<TextMeshProUGUI>().text;
                displayAnswer.text = answer;
                aButtonPressed = true;
            }
            else if (Gamepad.current.dpad.down.isPressed && Gamepad.current.aButton.isPressed && aButtonPressed == false)
            {
                answer += Buttons[1].GetComponentInChildren<TextMeshProUGUI>().text;
                displayAnswer.text = answer;
                aButtonPressed = true;
            }
            else if (Gamepad.current.dpad.right.isPressed && Gamepad.current.aButton.isPressed && aButtonPressed == false)
            {
                answer += Buttons[2].GetComponentInChildren<TextMeshProUGUI>().text;
                displayAnswer.text = answer;
                aButtonPressed = true;
            }
            else if (Gamepad.current.dpad.left.isPressed && Gamepad.current.aButton.isPressed && aButtonPressed == false)
            {
                answer += Buttons[3].GetComponentInChildren<TextMeshProUGUI>().text;
                displayAnswer.text = answer;
                aButtonPressed = true;
            }
            else if (!Gamepad.current.aButton.isPressed)
                aButtonPressed = false;

            if (Gamepad.current.bButton.isPressed)
                stopTexting();
        }
    }

    public void uiButtons(int buttonIndex) // assign ui buttons an index
    {
        if (answer.Length < 8)
        {
            answer += Buttons[buttonIndex].GetComponentInChildren<TextMeshProUGUI>().text;
            displayAnswer.text = answer;
        }
    }

    void shuffleLetters(string prompt)
    {
        letters = new List<char>();

        for (int i = 0; i < prompt.Length; i++)
        {
            letters.Add(prompt.ToString()[i]);
        }

        for (int i = 0; i < letters.Count; i++)
        {
            char temp = letters[i];
            int index = Random.Range(i, letters.Count);
            letters[i] = letters[index];
            letters[index] = temp;
        }

        for (int i = 0; i < prompt.Length; i++)
        {
            Buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = letters[i].ToString();
        }
    }
}
