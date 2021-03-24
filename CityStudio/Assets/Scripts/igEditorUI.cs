using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class igEditorUI : MonoBehaviour
{
    List<VideoPlayer> videoPlayer;
    TextMeshPro speedNum;
    public int entityType; // 1 - billboard, 2 - speedsign
    
    void Start()
    {  switch (entityType)
        {
            case 1:
                try
                {
                    videoPlayer = new List<VideoPlayer>();
                    GetComponentsInChildren(videoPlayer);
                }
                catch
                { }
                break;
            case 2:
                try
                {
                    speedNum = GetComponentInChildren<TextMeshPro>();
                }
                catch
                { }
                break;
        }
    }

    public void changeInfo(string _info)
    {
        switch (entityType)
        {
            case 1: // case 1 has been outdated, using fileexplorer now
                try
                {
                    foreach (VideoPlayer _videoPlayer in videoPlayer)
                    {
                        _videoPlayer.url = _info;
                    }
                }
                catch
                {
                    Debug.Log("Error: " + gameObject.name + " either does not contain videoPlayer or valid filePath");
                }
                break;
            case 2:
                try
                {
                    speedNum.text = _info;
                }
                catch
                {
                    Debug.Log("Error: " + gameObject.name + " does not contain a speed# child obj");
                }
                break;
            default:
                Debug.Log("Error: " + gameObject.name + " entityType does not exist");
                break;
        }
    }
}
