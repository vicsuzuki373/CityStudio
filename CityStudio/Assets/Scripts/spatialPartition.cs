using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spatialPartition : MonoBehaviour
{
    [Header("Partition")]
    public bool LogInitializationActivity;
    public Vector2Int regionSize;
    public float chunkSize;

    List<GameObject> region;
    List<List<GameObject>> regionY;
    List<List<List<GameObject>>> regionXY;

    [System.NonSerialized]
    public bool regionReady = false;

    [Header("Target")]
    public Transform player;
    [Range(0, 10)]
    public int loadRange = 0;
    Vector2 currentChunk;
    Vector2 previousChunk;

    void Start()
    {
        regionSize.x = Mathf.Abs(regionSize.x);
        regionSize.y = Mathf.Abs(regionSize.y);

        regionXY = new List<List<List<GameObject>>>();
        regionY = new List<List<GameObject>>();
        region = new List<GameObject>();  

        for (int i = 0; i < regionSize.x; i++)
        {
            for (int j = 0; j < regionSize.y; j++)
            {
                regionY.Add(region);
                region = new List<GameObject>();
            }
            regionXY.Add(regionY);
            regionY = new List<List<GameObject>>();

            if(LogInitializationActivity)
                Debug.Log("Region Created Size of " + regionSize.x * chunkSize + " by " + regionSize.y * chunkSize);
        }

        regionReady = true; // notify spatialParitionObjects that the region is ready
    }

    void OnDrawGizmosSelected()
    {
        regionSize.x = Mathf.Abs(regionSize.x);
        regionSize.y = Mathf.Abs(regionSize.y);

        //Highlights range of spatial partition
        Gizmos.color = Color.blue;
        for(int i = 0; i < regionSize.x + 1; i++)
            Gizmos.DrawLine(new Vector3(transform.position.x + (i * chunkSize), transform.position.y, transform.position.z), new Vector3(transform.position.x + (i * chunkSize), transform.position.y, transform.position.z + (regionSize.y * chunkSize)));

        Gizmos.color = Color.red;
        for (int j = 0; j < regionSize.y + 1; j++)
            Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z + (j * chunkSize)), new Vector3(transform.position.x + (regionSize.x * chunkSize), transform.position.y, transform.position.z + (j * chunkSize)));
    }

    public void sendStaticPosition(Vector3 _position, GameObject _sender)
    {
        if (_position.x > gameObject.transform.position.x && _position.x < gameObject.transform.position.x + (regionSize.x * chunkSize) && _position.z > gameObject.transform.position.z && _position.z < gameObject.transform.position.z + (regionSize.y * chunkSize))
        {
            Vector3 temp = new Vector3(Mathf.Abs(gameObject.transform.position.x - _position.x), _position.y, Mathf.Abs(gameObject.transform.position.z - _position.z));

            regionXY[(int)(temp.x / chunkSize)][(int)(temp.z / chunkSize)].Add(_sender);
            _sender.SetActive(false);

            if(LogInitializationActivity)
                Debug.Log(_sender.name + " added at chunk: " + (int)(temp.x / chunkSize) + " " + (int)(temp.z / chunkSize));          
        }
        else if (LogInitializationActivity)
            Debug.Log(_sender.name + " is not within partition region and has not been added.");
    }

    void loadChunks(Vector2 _chunk, bool active)
    {
        if (_chunk.x < regionSize.x && _chunk.x >= 0 && _chunk.y < regionSize.y && _chunk.y >= 0)
        {
            for(int i = -loadRange; i <= loadRange; i++)
            {
                for(int j = -loadRange; j <= loadRange; j++)
                {
                    try
                    {
                        foreach (GameObject _subject in regionXY[(int)_chunk.x + i][(int)_chunk.y + j])
                        {
                            _subject.SetActive(active);
                        }
                    }
                    catch { };
                }
            } 
        }
    }

    public void externalReloadChunk(Vector2 chunk)
    {
        loadChunks(chunk, true);
    }

    void checkEnter()
    {
        regionSize.x = Mathf.Abs(regionSize.x);
        regionSize.y = Mathf.Abs(regionSize.y);

        if (player.transform.position.x > gameObject.transform.position.x && player.transform.position.x < gameObject.transform.position.x + (regionSize.x * chunkSize) && player.transform.position.z > gameObject.transform.position.z && player.transform.position.z < gameObject.transform.position.z + (regionSize.y * chunkSize))
        {
            Vector3 temp = new Vector3(Mathf.Abs(gameObject.transform.position.x - player.transform.position.x), player.transform.position.y, Mathf.Abs(gameObject.transform.position.z - player.transform.position.z));

            previousChunk = currentChunk;
            currentChunk = new Vector2((int)(temp.x / chunkSize), (int)(temp.z / chunkSize));

            if (previousChunk != currentChunk)
            {
                loadChunks(previousChunk, false);
                loadChunks(currentChunk, true);
            }
        }
        else if(currentChunk != new Vector2(-1, -1))
        {
            currentChunk = new Vector2(-1, -1);
            loadChunks(previousChunk, false);
        }
    }

    void Update()
    {
        if (player != null)
            checkEnter();
        else if (LogInitializationActivity)
            Debug.Log("Player Object reference missing, Spatial Partition is paused.");
    }
}
