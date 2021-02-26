using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class igEditorUI : MonoBehaviour
{
    List<VideoPlayer> videoPlayer;
    public int entityType; // 1 - billboard, 2 - speedsign
    
    void Start()
    {  
        try
        {
            videoPlayer = new List<VideoPlayer>();
            GetComponentsInChildren(videoPlayer);
        }
        catch
        {
        }
    }

    void Update()
    {
    }

    public void changeInfo(string _info)
    {
        switch (entityType)
        {
            case 1:
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
            default:
                Debug.Log("Error: " + gameObject.name + " entityType does not exist");
                break;
        }
    }
}
