using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [Header("Menu objects")]
    public GameObject MenuCamera;
    public GameObject MenuPlay;
    public GameObject MenuEditor;
    public GameObject MenuExit;

    [Header("Car/Gameplay objects")]
    public GameObject car;
    public GameObject CarCanvas;
    public GameObject phone;
    public GameObject radio;
    public GameObject carspawner;

    [Header("Buttons")]
    public GameObject GameResume;
    public GameObject GameRestart;
    public GameObject GameExit;

    [Header("Sessions")]
    public GameObject Results;
    public Text Infractions;
    public Text Distractions;
    public Text Total;
    public Text Sessions;
    public Text Name;

    private bool isPlaying = false;
    public static bool paused = true;
    public static bool gameover = false;
    private string sessionspath = "Assets/Resources/sessions.txt";

    // Start is called before the first frame update
    void Start()
    {
        //create text file if there isnt one
        StreamWriter writer = new StreamWriter(sessionspath, true);
        writer.Close();

        StreamReader reader = new StreamReader(sessionspath);
        Sessions.text = reader.ReadToEnd();
        reader.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
            {
                if (paused)
                {
                    Resume();
                }
                else
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(GameResume);
                    GameResume.SetActive(true);
                    GameRestart.SetActive(true);
                    GameExit.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    paused = true;
                    phone.GetComponent<AudioSource>().Pause();
                    radio.GetComponent<AudioSource>().Pause();
                    Time.timeScale = 0;
                }
            }
        }
        if (gameover)
        {
            int overspeedint = (int)GameController.amountOverSpeed;
            int wrongwayint = (int)LaneChecker.amount;
            Results.SetActive(true);
            Infractions.text = TrafficLightChecker.amount.ToString() + "\n"
                + Car.amountcollided.ToString() + "\n"
                + overspeedint.ToString() + "\n"
                + wrongwayint.ToString();
            int phoneint = (int)Phone.timeDistracted;
            int radioint = (int)Radio.timeDistracted;
            int beveragecupint = (int)BeverageCup.timeDistracted;
            Distractions.text = phoneint.ToString() + "\n"
                + radioint.ToString() + "\n"
                + beveragecupint.ToString();
            int total = phoneint + radioint + beveragecupint;
            Total.text = total.ToString();

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(GameRestart);
            CarCanvas.SetActive(false);
            GameResume.SetActive(false);
            GameRestart.SetActive(true);
            GameExit.SetActive(true);
            phone.GetComponent<AudioSource>().Pause();
            radio.GetComponent<AudioSource>().Pause();
            paused = true;
            isPlaying = false;
            gameover = false;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }

        float LeftJoystickY = Input.GetAxis("LeftJoystickY");
        if(LeftJoystickY < -0.5f && MenuPlay.activeSelf && EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(MenuPlay);
        else if(LeftJoystickY < -0.5f && GameResume.activeSelf && EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(GameResume);
        else if (LeftJoystickY < -0.5f && Results.activeSelf && EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(GameRestart);

    }

    public void Play()
    {
        Restart();
        isPlaying = true;
        paused = false;
        MenuCamera.SetActive(false);
        MenuPlay.SetActive(false);
        MenuEditor.SetActive(false);
        MenuExit.SetActive(false);
        car.SetActive(true);
        CarCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Editor()
    {
        EventSystem.current.SetSelectedGameObject(null);
        MenuCamera.SetActive(false);
        MenuPlay.SetActive(false);
        MenuEditor.SetActive(false);
        MenuExit.SetActive(false);
        igEditor.startEditor = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ReturnFromEditor()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MenuPlay);
        MenuCamera.SetActive(true);
        MenuPlay.SetActive(true);
        MenuEditor.SetActive(true);
        MenuExit.SetActive(true);
        igEditor.startEditor = true;
    }

    public void ReturnFromGame()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MenuPlay);
        Restart();
        isPlaying = false;
        paused = true;
        MenuCamera.SetActive(true);
        MenuPlay.SetActive(true);
        MenuEditor.SetActive(true);
        MenuExit.SetActive(true);
        car.SetActive(false);
        CarCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameResume.SetActive(false);
        GameRestart.SetActive(false);
        GameExit.SetActive(false);
        paused = false;
        phone.GetComponent<AudioSource>().UnPause();
        radio.GetComponent<AudioSource>().UnPause();
        Time.timeScale = 1;
    }

    public void Restart()
    {
        LaneChecker.amount = 0;
        TrafficLightChecker.amount = 0;
        Radio.restart = true;
        Phone.restart = true;
        BeverageCup.restart = true;
        Car.restart = true;
        GameController.restart = true;
        TrafficLightController.restart = true;
        CarAI.amountcar = 0;
        CarCanvas.SetActive(true);
        for (int i = 0; i < carspawner.transform.childCount; i++)
        {
            carspawner.transform.GetChild(i).GetComponent<CarAI>().restart = true;
        }
        Results.SetActive(false);
        isPlaying = true;
        gameover = false;
        Resume();
    }

    public void Save()
    {
        StreamWriter writer = new StreamWriter(sessionspath, true);
        writer.WriteLine(Total.text + "(s) " + Name.text);
        writer.Close();

        StreamReader reader = new StreamReader(sessionspath);
        Sessions.text = reader.ReadToEnd();
        reader.Close();
    }
}