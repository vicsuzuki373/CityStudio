using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    public static int action = 0;
    public List<AudioClip> sounds = new List<AudioClip>();
    public Text soundName;

    private bool power = false;
    private int currentSong = 0;

    void Start()
    {
        sounds.Add(Resources.Load<AudioClip>("Radio/MyHeart"));
        sounds.Add(Resources.Load<AudioClip>("Radio/Glue"));
        gameObject.GetComponent<AudioSource>().clip = sounds[currentSong];

        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<AudioSource>().mute = true;

        //DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Radio");
        //FileInfo[] info = dir.GetFiles("*.mp3*");
        //foreach (FileInfo f in info)
        //{
        //    print(f.Name);
        //}
    }
    
    void Update()
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
        if(!gameObject.GetComponent<AudioSource>().isPlaying)
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
