using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CheckPoint : MonoBehaviour
{
    public string checkPointId;
    public bool activeCheckPoint;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            ActivatedCheckPoint();
        }
    }

    public void ActivatedCheckPoint()
    {
        activeCheckPoint = true;
        Debug.LogWarning("playerchecked");
    }
   [ContextMenu("gerenrateId")]
    public void GenerateId()
    {
        checkPointId = System.Guid.NewGuid().ToString();
    }
}
