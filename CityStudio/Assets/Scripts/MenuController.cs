using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour
{
    [Header("Menu objects")]
    public GameObject MenuCamera;
    public GameObject MenuCanvas;

    [Header("Car/Gameplay objects")]
    public GameObject Car;
    public GameObject CarCanvas;

    [Header("Buttons")]
    public GameObject MenuPlay;
    public GameObject EditorReturn;
    public GameObject GameResume;
    public GameObject GameExit;

    private bool isPlaying = false;
    public static bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaying)
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
                    Time.timeScale = 0;
                    GameResume.SetActive(true);
                    GameExit.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    paused = true;
                }
            }
        }
    }

    public void Play()
    {
        isPlaying = true;
        MenuCamera.SetActive(false);
        MenuCanvas.SetActive(false);
        Car.SetActive(true);
        CarCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Editor()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(EditorReturn);
        MenuCamera.SetActive(false);
        MenuCanvas.SetActive(false);
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
        MenuCanvas.SetActive(true);
        igEditor.startEditor = true;
    }

    public void ReturnFromGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        GameResume.SetActive(false);
        GameExit.SetActive(false);
        paused = false;
    }
}
