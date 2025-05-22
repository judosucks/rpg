// using System.Linq;
// using System;
// using System.Collections.Generic;
// using NUnit.Framework;
// using UnityEngine;
//
// public class SaveManager : MonoBehaviour
// {
//     public static SaveManager instance;
//     private GameData gameData;
//     private List<ISaveManager> saveManagers;
//     [SerializeField] private string fileName;
//     private FileDataHandler dataHandler;
//     private void Awake()
//     {
//         if (instance != null)
//         {
//             Destroy(instance.gameObject);
//         }
//         else
//         {
//             instance = this;
//         }
//     }
//
//     private void Start()
//     {
//         dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
//         saveManagers = FindAllSaveManagers();
//         LoadGame();
//     }
//
//     public void NewGame()
//     {
//         gameData = new GameData();
//     }
//
//     public void LoadGame()
//     {
//         gameData = dataHandler.Load();
//         if (this.gameData == null)
//         {
//             Debug.Log("no gamedada found");
//             NewGame();
//         }
//
//         foreach (ISaveManager saveManager in saveManagers)
//         {
//             saveManager.LoadData(gameData);
//         }
//         
//     }
//
//     public void SaveGame()
//     {
//         Debug.Log("save game");
//         foreach (ISaveManager saveManager in saveManagers)
//         {
//          saveManager.SaveData(ref gameData);   
//         }
//         dataHandler.Save(gameData);
//     }
//
//     private void OnApplicationQuit()
//     {
//         SaveGame();
//     }
//
//     private List<ISaveManager> FindAllSaveManagers()
//     {
//         //findobjectoftype has been depreated using findobjectbytype
//          saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).
//              OfType<ISaveManager>().ToList();
//          return new List<ISaveManager>(saveManagers);
//
//     }
// }
using System.Linq;
using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    private GameData gameData;
    private List<ISaveManager> saveManagers;
    [SerializeField] private string fileName;
    [SerializeField] private bool encryptData;
    private FileDataHandler dataHandler;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
            
        }
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);
        if (dataHandler==null)
        {
            Debug.LogWarning("datahandler is null");
            StartCoroutine(LoadDataHandlerWithDelay());
        }
        
        saveManagers = FindAllSaveManagers();
        LoadGame();
    }

    private IEnumerator LoadDataHandlerWithDelay()
    {
        while (dataHandler == null)
        {
            yield return null; // Wait for the next frame
        }
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        if(dataHandler!= null)
        {
            Debug.LogWarning("datahandler is not null");
        }
    }
    public void NewGame()
    {
        gameData = new GameData();
    }
    [ContextMenu("Delete Save")]
    public void DeleteSavedGame()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);
        dataHandler.Delete();
    }
    public void LoadGame()
    {
        if (dataHandler != null)
        {
          gameData = dataHandler.Load();
          if (this.gameData == null)
          {
            Debug.Log("no gamedada found");
            NewGame();
          }
            
        }
        else
        {
            Debug.LogWarning("datahandler is null");
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            Debug.LogWarning("loading data");
            saveManager.LoadData(gameData);
        }
        
    }

    public void SaveGame()
    {
        Debug.Log("save game");
        if (saveManagers == null) return; // Add this line
        foreach (ISaveManager saveManager in saveManagers) // Line 57
        {
            Debug.LogWarning("saving skill data");
            saveManager.SaveData(ref gameData);   
        }
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame(); // Line 66
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include, 
            FindObjectsSortMode.None
        ).OfType<ISaveManager>();

        return new List<ISaveManager>(saveManagers);
        // //findobjectoftype has been depreated using findobjectbytype
        // IEnumerable<ISaveManager> saveManagers = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).
        //     OfType<ISaveManager>().ToList();
        // Debug.LogWarning("Save managers found: " + saveManagers.ToString());
        // return new List<ISaveManager>(saveManagers);
    

    }

    public bool HasSavedData()
    {
        if (dataHandler.Load() != null)
        {
            Debug.Log("Saved data found");
            return true;
        }
        Debug.Log("No saved data found");
        return false;
    }
}