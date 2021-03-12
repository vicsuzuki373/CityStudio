using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Radio : MonoBehaviour
{
    bool power = false;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>().Play();
        gameObject.GetComponent<AudioSource>().mute = true;
        
        //DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Radio");
        //FileInfo[] info = dir.GetFiles("*.mp3*");
        //foreach (FileInfo f in info)
        //{
        //    print(f.Name);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        GameController.cupinteract = 3;
        if (Input.GetMouseButtonDown(0))
        {
            if (power)
                gameObject.GetComponent<AudioSource>().mute = true;
            else
                gameObject.GetComponent<AudioSource>().mute = false;
            power = !power;
        }
    }
}
