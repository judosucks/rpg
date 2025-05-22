using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisibilityController : MonoBehaviour
{
    public Transform player; // Assign the player's Transform in the Inspector
    public Tilemap groundTilemap; // Assign the ground Tilemap in the Inspector
    public float thresholdY = 0f; // Set this to the Y-level where the Tilemap should be hidden
    private Transform playerT;
    private void Start()
    {
        if (PlayerManager.instance != null)
        {
          playerT = PlayerManager.instance.player.transform;
        }
        else
        {
            Debug.LogError("p i is null");
        }
        //make sure the gorund is visible at the start
        if (groundTilemap != null)
        {
            groundTilemap.GetComponent<Renderer>().enabled = true;
        }
    }

    private void Update()
    {
        if (playerT == null || groundTilemap == null)
        {
            Debug.LogWarning("no playet no tilemap");
            return;
        } // Add this
    

        float playerY = playerT.position.y;
        groundTilemap.GetComponent<Renderer>().enabled = (playerY < thresholdY);
    }
}