using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DebrisPool : MonoBehaviour
{
    public static DebrisPool Instance;
    public GameObject prefab;
    public int poolSize = 10;
    private Queue<GameObject> pool = new Queue<GameObject>();
    public Transform effectsParent; // 新增字段

    void Awake() => Instance = this;


    void Start()
    {
        if (effectsParent == null)
        {
            Debug.LogWarning("effectparent is null");
            effectsParent = transform;// if effectparent is not assigned use the current object
        }
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, effectsParent,false); // 设置父物体
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    private void Update()
    {
        // Debug.Log($"可用特效数量: {DebrisPool.Instance.pool.Count}");
    }

    public GameObject GetEffect()
    {
        GameObject obj;
        if (pool.Count == 0)
        {
            //pool is empty create a new one and assign the parent
            obj = Instantiate(prefab, effectsParent, false);
            
            return obj;
        }

        obj = pool.Dequeue();
        obj.SetActive(true);
        return obj;
        // if (pool.Count == 0) return Instantiate(prefab);
        // GameObject obj = pool.Dequeue();
        // obj.SetActive(true);
        // return obj;
    }

    public void ReturnEffect(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(effectsParent);
        //ensure the object is parented correctly when returned
        pool.Enqueue(obj);
    }
    public void DisableAllEffects()
    {
        foreach(Transform child in effectsParent)
        {
            child.gameObject.SetActive(false);
        }
    }
    void OnDrawGizmos() 
    {
        if(effectsParent != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(effectsParent.position, Vector3.one * poolSize/10f);
        }
    }
}