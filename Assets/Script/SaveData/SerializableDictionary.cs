using System.Runtime.Serialization;
using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class SerializableDictionary<TKey,TValue>:Dictionary<TKey,TValue>,ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();
     public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (KeyValuePair<TKey,TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();
        if (keys.Count != values.Count)
        {
            Debug.Log("keys count is not equial to values count");
        }

        for (int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i],values[i]);
        }
    }
}
