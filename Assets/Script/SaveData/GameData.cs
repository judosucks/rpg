using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public int currency;
    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, bool> skillTree;
    public List<string> equipmentId;
    public string closetCheckPointId;
    public SerializableDictionary<string, bool> checkPoints;
    public GameData()
    {
        this.currency = 0;
        skillTree = new SerializableDictionary<string, bool>();
        inventory = new SerializableDictionary<string, int>();
        equipmentId = new List<string>();
        
        closetCheckPointId = string.Empty;
        checkPoints = new SerializableDictionary<string, bool>();
    }
}
