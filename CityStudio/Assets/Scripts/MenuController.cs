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
    public GameObject MenuSessions;
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
    public Text Stats;
    public Text Infractions;
    public Text Distractions;
    public Text Total;
    public Text Sessions;
    public Text Name;

    [Header("AllSessions")]
    public GameObject AllSessions;
    public GameObject SessionsReturn;
    public List<Text> SessionsStats = new List<Text>();

    private bool isPlaying = false;
    public static bool paused = true;
    public static bool gameover = false;
    private string sessionssimplepath = "Assets/Resources/sessionssimple.txt";
    private string sessionscompletepath = "Assets/Resources/sessionscomplete.txt";

    // Start is called before the first frame update
    void Start()
    {
        //create text file if there isnt one
        StreamWriter writer = new StreamWriter(sessionssimplepath, true);
        writer.Close();

        StreamReader reader = new StreamReader(sessionssimplepath);
        Sessions.text = reader.ReadToEnd();
        reader.Close();

        StreamWriter writer1 = new StreamWriter(sessionscompletepath, true);
        writer1.Close();

        StreamReader reader1 = new StreamReader(sessionscompletepath);
        reader1.Close();
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
            int sessionTimeint = (int)Car.sessionTime;
            int distanceDrivenint = (int)Car.DistanceDriven * 4;
            Stats.text = sessionTimeint.ToString() + "\n"
                + Car.GetAverageSpeed() + "\n"
                + distanceDrivenint.ToString() + "\n";
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
            float billboardfloat = 0;
            BillboardDistraction[] billboards = FindObjectsOfType(typeof(BillboardDistraction)) as BillboardDistraction[];
            foreach (var obj in billboards)
            {
                billboardfloat += obj.timeDistracted;
            }
            int billboardint = (int)billboardfloat;
            Distractions.text = phoneint.ToString() + "\n"
                + radioint.ToString() + "\n"
                + beveragecupint.ToString() + "\n"
                + billboardint.ToString();
            int total = phoneint + radioint + beveragecupint + billboardint;
            Total.text = total.ToString();
            CarCanvas.SetActive(false);
            GameResume.SetActive(false);
            GameRestart.SetActive(true);
            GameExit.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(GameRestart);
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
        else if (LeftJoystickY < -0.5f && AllSessions.activeSelf && EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(SessionsReturn);

    }

    public void Play()
    {
        Restart();
        isPlaying = true;
        paused = false;
        MenuCamera.SetActive(false);
        MenuPlay.SetActive(false);
        MenuEditor.SetActive(false);
        MenuSessions.SetActive(false);
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
        MenuSessions.SetActive(false);
        MenuExit.SetActive(false);
        igEditor.startEditor = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ReturnFromEditor()
    {
        MenuCamera.SetActive(true);
        MenuPlay.SetActive(true);
        MenuEditor.SetActive(true);
        MenuSessions.SetActive(true);
        MenuExit.SetActive(true);
        igEditor.startEditor = true;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MenuPlay);
    }

    public void ReturnFromSessions()
    {
        AllSessions.SetActive(false);
        MenuPlay.SetActive(true);
        MenuEditor.SetActive(true);
        MenuSessions.SetActive(true);
        MenuExit.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MenuPlay);
    }

        public void ReturnFromGame()
    {
        Restart();
        isPlaying = false;
        paused = true;
        MenuCamera.SetActive(true);
        MenuPlay.SetActive(true);
        MenuEditor.SetActive(true);
        MenuSessions.SetActive(true);
        MenuExit.SetActive(true);
        car.SetActive(false);
        CarCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(MenuPlay);
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
        pedestrians.restart = true;
        CarCanvas.SetActive(true);
        for (int i = 0; i < carspawner.transform.childCount; i++)
        {
            carspawner.transform.GetChild(i).GetComponent<CarAI>().restart = true;
        }
        BillboardDistraction[] billboards = FindObjectsOfType(typeof(BillboardDistraction)) as BillboardDistraction[];
        foreach (var obj in billboards)
            obj.restart = true;
        Results.SetActive(false);
        isPlaying = true;
        gameover = false;
        Resume();
    }

    public void Save()
    {
        StreamWriter writer = new StreamWriter(sessionssimplepath, true);
        writer.WriteLine(Total.text + "(s) " + Name.text);
        writer.Close();

        StreamReader reader = new StreamReader(sessionssimplepath);
        Sessions.text = reader.ReadToEnd();
        reader.Close();


        int sessionTimeint = (int)Car.sessionTime;
        int distanceDrivenint = (int)Car.DistanceDriven * 4;
        int overspeedint = (int)GameController.amountOverSpeed;
        int wrongwayint = (int)LaneChecker.amount;
        int phoneint = (int)Phone.timeDistracted;
        int radioint = (int)Radio.timeDistracted;
        int beveragecupint = (int)BeverageCup.timeDistracted;
        float billboardfloat = 0;
        BillboardDistraction[] billboards = FindObjectsOfType(typeof(BillboardDistraction)) as BillboardDistraction[];
        foreach (var obj in billboards)
        {
            billboardfloat += obj.timeDistracted;
        }
        int billboardint = (int)billboardfloat;
        
        StreamWriter writer1 = new StreamWriter(sessionscompletepath, true);
        writer1.WriteLine(Name.text + "|" 
            + sessionTimeint.ToString() + "|"
            + Car.GetAverageSpeed().ToString() + "|"
            + distanceDrivenint.ToString() + "|"
            + TrafficLightChecker.amount.ToString() + "|"
            + Car.amountcollided.ToString() + "|"
            + overspeedint.ToString() + "|"
            + wrongwayint.ToString() + "|"
            + phoneint.ToString() + "|"
            + radioint.ToString() + "|"
            + beveragecupint.ToString() + "|"
            + billboardint.ToString());
        writer1.Close();
    }

    public void SessionsButton()
    {
        AllSessions.SetActive(true);
        MenuPlay.SetActive(false);
        MenuEditor.SetActive(false);
        MenuSessions.SetActive(false);
        MenuExit.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(SessionsReturn);

        StreamReader reader1 = new StreamReader(sessionscompletepath);
        string temp = reader1.ReadToEnd();
        string[] lines = temp.Split('\n');
        foreach (string line in lines)
        {
            string[] stats = line.Split('|');
            for (int i = 0; i < stats.Length; i++)
            {
                SessionsStats[i].text += stats[i] + "\n";
            }
        }
        reader1.Close();
    }

    public void ResetData()
    {
        File.Create(sessionssimplepath).Close();
        File.Create(sessionscompletepath).Close();
        for(int i = 0; i < SessionsStats.Count; i++)
            SessionsStats[i].text = "";
    }
}