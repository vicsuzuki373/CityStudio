using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;
using System.IO;
using UnityEngine.UI;

using UnityEngine.Video;
using UnityEngine.Networking;


public class fileExplorer : MonoBehaviour
{
    DirectoryInfo di;
    DirectoryInfo[] diArr;

    public Dropdown dropDown;
    public List<string> dropDownOptions;

    public List<string> path;
    public string workingPath;

    FileInfo[] diFiles;
    string lastSelectedFile;
    List<AudioClip> _audio;
    int soundChannel;
    List<VideoPlayer> _video;

    void Start()
    {
        path.Add("c:\\");
        workingPath = null;

        di = new DirectoryInfo("c:\\");
        diArr = di.GetDirectories();
        diFiles = di.GetFiles();

        dropDown.ClearOptions();

        dropDownOptions.Add("");

        foreach (DirectoryInfo dri in diArr)
        {
            if (!dri.Name.Contains("$")) // remove recycle bin option
                dropDownOptions.Add(dri.Name);
        }

        dropDown.AddOptions(dropDownOptions);
    }


    void Update()
    {

    }

    public void checkSelection() // link to dropdown object
    {
        if (dropDownOptions[dropDown.value] == "") // check if default
        {
            return;
        }
        else if (dropDownOptions[dropDown.value] == "<b>BACK</b>") // check if user wants to go back a directory
        {
            path.RemoveAt(path.Count - 1);
        }
        else if (!dropDownOptions[dropDown.value].Contains(".")) // normal directory item
            path.Add("\\" + dropDownOptions[dropDown.value]);
        else // actual file
            lastSelectedFile = workingPath + "\\" + dropDownOptions[dropDown.value];

        workingPath = null;

        for (int i = 0; i < path.Count; i++)
        {
            workingPath += path[i];
        }

        di = new DirectoryInfo(workingPath);
        diArr = di.GetDirectories();
        diFiles = di.GetFiles();

        dropDown.ClearOptions();
        dropDownOptions.Clear();

        dropDownOptions.Add("");

        foreach (DirectoryInfo dri in diArr)
        {
            if (!dri.Name.Contains("$"))
                dropDownOptions.Add(dri.Name);
        }

        if (path.Count != 1) // make sure it isnt root directory
        {
            foreach (FileInfo dfi in diFiles)
                dropDownOptions.Add(dfi.Name);

            dropDownOptions.Add("<b>BACK</b>");
        }

        dropDown.AddOptions(dropDownOptions);
    }

    public void applySelection(GameObject _object, int _soundChannel) // link to button object
    {
        if (_object == null)
            return;

        if (_object.GetComponent<AudioSource>() && lastSelectedFile.Contains(".mp3"))
        {
            _audio = _object.GetComponent<Radio>().sounds;
            soundChannel = _soundChannel;
            StartCoroutine(GetAudioClip());
        }

        if (_object.GetComponentInChildren<VideoPlayer>() && lastSelectedFile.Contains(".mp4"))
        {
            _video = new List<VideoPlayer>();
            _object.GetComponentsInChildren(_video);

            foreach (VideoPlayer screens in _video)
            {
                screens.url = lastSelectedFile;
            }
        }
    }

    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + lastSelectedFile, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.result);
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (_audio.Count < soundChannel)
                    _audio[soundChannel] = clip;
                else
                    _audio.Add(clip);
                StopCoroutine(GetAudioClip());
            }
        }
    }
}