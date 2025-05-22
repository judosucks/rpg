using System;
using UnityEngine;

public class EdgeTriggerDetection : MonoBehaviour
{
   public bool isNearLeftEdge = false;
   public bool isNearRightEdge = false;
   public bool isFacingRight;
   // Optional debounce logic (to avoid rapid toggling near edges)
   private float edgeDetectionCooldown = 0.1f;
   private float lastLeftEdgeTime, lastRightEdgeTime;
   private void Start()
   {
      
   }
   private void Update()
   {
      if (isFacingRight)
      {
         
      }
   }

   private void FixedUpdate()
   {
      isFacingRight = PlayerManager.instance?.player?.IsFacingRight() ?? true;
      
   }

   

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (CompareLayer(other, "Ground"))
      {
         if (gameObject.name == "GroundLeftCheck" && Time.time - lastLeftEdgeTime > edgeDetectionCooldown)
         {
            isNearLeftEdge = true;
            lastLeftEdgeTime = Time.time;
            Debug.Log("Enter left edge");
         }
         else if (gameObject.name == "GroundRightCheck"  && Time.time - lastRightEdgeTime > edgeDetectionCooldown)
         {
            isNearRightEdge = true;
            lastRightEdgeTime = Time.time;
            Debug.Log("Enter right edge");
         }
      }
   }

   private void OnTriggerExit2D(Collider2D other)
   {
      if (CompareLayer(other, "Ground"))
      {
         
         if (gameObject.name == "GroundLeftCheck"&& Time.time - lastLeftEdgeTime > edgeDetectionCooldown)
         {
            isNearLeftEdge = false;
            lastLeftEdgeTime = Time.time;
            Debug.Log("Exit left edge");
         }
         else if (gameObject.name == "GroundRightCheck"&& Time.time - lastRightEdgeTime > edgeDetectionCooldown)
         {
            isNearRightEdge = false;
            lastRightEdgeTime = Time.time;
            Debug.Log("Exit right edge");
         }
      }
   }
   public static bool CompareLayer(Collider2D other, string layerName)
   {
      return other.gameObject.layer == LayerMask.NameToLayer(layerName);
   }
}
