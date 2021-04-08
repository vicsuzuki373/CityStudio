using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    public static float timeDistracted = 0;
    public static bool restart;
    public static int action = 0;
    public Text soundName;
    public List<AudioClip> sounds = new List<AudioClip>();

    private bool power = false;
    private int currentSong = 0;

    void Start()
    {
        sounds.Add(Resources.Load<AudioClip>("Radio/better-days"));
        if (sounds.Count > 0)
        {
            gameObject.GetComponent<AudioSource>().clip = sounds[currentSong];

            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<AudioSource>().mute = true;
        }

    }

    void Update()
    {
        if (!MenuController.paused)
        {
            if (sounds.Count > 0)
            {
                soundName.text = sounds[currentSong].name;
                if (action == 1)
                {
                    if (currentSong - 1 < 0)
                    {
                        gameObject.GetComponent<AudioSource>().clip = sounds[sounds.Count - 1];
                        gameObject.GetComponent<AudioSource>().Play();
                        currentSong = sounds.Count - 1;
                    }
                    else
                    {
                        gameObject.GetComponent<AudioSource>().clip = sounds[currentSong - 1];
                        gameObject.GetComponent<AudioSource>().Play();
                        currentSong -= 1;
                    }
                }
                else if (action == 2)
                {
                    if (power)
                    {
                        gameObject.GetComponent<AudioSource>().mute = true;
                        power = !power;
                    }
                    else
                    {
                        gameObject.GetComponent<AudioSource>().mute = false;
                        power = !power;
                    }
                }
                else if (action == 3)
                {
                    if (currentSong + 1 >= sounds.Count)
                    {
                        gameObject.GetComponent<AudioSource>().clip = sounds[0];
                        gameObject.GetComponent<AudioSource>().Play();
                        currentSong = 0;
                    }
                    else
                    {
                        gameObject.GetComponent<AudioSource>().clip = sounds[currentSong + 1];
                        gameObject.GetComponent<AudioSource>().Play();
                        currentSong += 1;
                    }
                }
                if (!gameObject.GetComponent<AudioSource>().isPlaying)
                {
                    if (currentSong + 1 >= sounds.Count)
                    {
                        gameObject.GetComponent<AudioSource>().clip = sounds[0];
                        gameObject.GetComponent<AudioSource>().Play();
                        currentSong = 0;
                    }
                    else
                    {
                        gameObject.GetComponent<AudioSource>().clip = sounds[currentSong + 1];
                        gameObject.GetComponent<AudioSource>().Play();
                        currentSong += 1;
                    }
                }
                action = 0;
            }
        }
        if (restart)
        {
            Restart();
            restart = false;
        }
    }

    private void OnMouseOver()
    {
        if (!MenuController.paused)
        {
            timeDistracted += Time.deltaTime;
        }
    }

    private void Restart()
    {
        timeDistracted = 0;
        action = 0;
        power = false;
        currentSong = 0;
        if (sounds.Count > 0)
        {
            gameObject.GetComponent<AudioSource>().clip = sounds[currentSong];
            gameObject.GetComponent<AudioSource>().Play();
            gameObject.GetComponent<AudioSource>().mute = true;
        }
    }
}
