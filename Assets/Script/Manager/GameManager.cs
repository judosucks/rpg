using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;
    [SerializeField] private CheckPoint[] checkPoints;
    private Transform player;
    [SerializeField] private string closestCheckPointId;
    [SerializeField] private string closestCheckPointLoaded;
    [Header("lost currentcy")]
    [SerializeField] private GameObject lostcurrencyPrefab;

    public int lostCurrencyAmount;
    [SerializeField] private float lostCurrencyX;
    [SerializeField] private float lostCurrencyY;
    

    private void Awake()
    {
        Debug.LogWarning("Awake called gamemanager");
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        checkPoints = FindObjectsByType<CheckPoint>(FindObjectsSortMode.None);
        Debug.LogWarning("Checkpoints found: " + checkPoints);
        player = PlayerManager.instance.player.transform;
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        Debug.LogWarning("Loading data");
        foreach (KeyValuePair<string, bool> pair in _data.checkPoints)
        {
            Debug.LogWarning("loading checkpoint data");
            foreach (CheckPoint checkPoint in checkPoints)
            {
                Debug.LogWarning("loading checkpoint data 2");
                if (checkPoint.checkPointId == pair.Key && pair.Value == true)
                {
                    Debug.LogWarning("CheckPoint activated");
                    checkPoint.ActivatedCheckPoint();
                }
            }
        }
        closestCheckPointId = _data.closetCheckPointId;

        Invoke("PlacePlayerAtClosestCheckPoint",0.1f);
    }

    private void PlacePlayerAtClosestCheckPoint()
    {
        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (closestCheckPointId== checkPoint.checkPointId)
            {
                Debug.LogWarning("loading checkpoint");
                player.position = checkPoint.transform.position;
                closestCheckPointLoaded = closestCheckPointId;
            }
        }
    }

    private IEnumerator LoadWithDelay(GameData _data)
    {
        yield return new WaitForSeconds(0.1f);
        LoadData(_data);
    }
    public void SaveData(ref GameData _data)
    {
     Debug.LogWarning("Saving data");

        // Find the closest *active* checkpoint
        CheckPoint closestActiveCheckPoint = FindClosestCheckPoint();

        // Check if we found one before trying to access its ID
        if (closestActiveCheckPoint != null)
        {
            _data.closetCheckPointId = closestActiveCheckPoint.checkPointId;
            Debug.LogWarning($"Saving closest checkpoint ID: {closestActiveCheckPoint.checkPointId}");
        }
        else
        {
            // Handle the case where no active checkpoint was found
            // Option 1: Save null or an empty string
            _data.closetCheckPointId = null; // Or string.Empty if the type is string
            // Option 2: Keep the previously saved checkpoint ID (if appropriate)
            // (No change needed here if _data already holds the last value)

            Debug.LogWarning("No active checkpoint found to save as closest.");
        }

        _data.checkPoints.Clear();
        foreach (CheckPoint checkPoint in checkPoints)
        {
            // This part seems okay, assuming checkPoints array is valid
            if (checkPoint != null && !string.IsNullOrEmpty(checkPoint.checkPointId)) // Added safety checks
            {
                // Debug.LogWarning($"Saving checkpoint state: {checkPoint.checkPointId} = {checkPoint.activeCheckPoint}"); // More detailed log
                _data.checkPoints.Add(checkPoint.checkPointId, checkPoint.activeCheckPoint);
            }
            else
            {
                Debug.LogWarning("Skipping saving a null or invalid checkpoint entry.");
            }
        }
    }

    private CheckPoint FindClosestCheckPoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckPoint = null; // Starts as null
        foreach (var checkPoint in checkPoints)
        {
            // Potential issue 1: PlayerManager.instance might be null if called during shutdown/scene change
            
            float distance = Vector2.Distance(player.position, checkPoint.transform.position);
            Debug.LogWarning("Distance to checkpoint " + checkPoint.checkPointId + ": " + distance); // Added name for clarity
            // This condition must be met to assign closestCheckPoint
            if (distance < closestDistance && checkPoint.activeCheckPoint == true)
            {
                Debug.LogWarning("closest checkpoint found: " + checkPoint.checkPointId); // Added name for clarity
                closestDistance = distance;
                closestCheckPoint = checkPoint;
            }
        }

        // If the loop finishes and NO checkpoints were found that were ALSO active,
        // closestCheckPoint will still be null here.
        Debug.LogWarning("Final closest checkpoint: " + (closestCheckPoint != null ? closestCheckPoint.checkPointId : "None"));
        return closestCheckPoint;
    }
}