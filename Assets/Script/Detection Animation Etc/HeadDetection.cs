using UnityEngine;

public class HeadDetection : MonoBehaviour
{
   public bool isTouchingHead;

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("Ground"))
      {
         isTouchingHead = true;
      }
   }
   private void OnTriggerExit2D(Collider2D other)
   {
      if (other.CompareTag("Ground"))
      {
         isTouchingHead = false;
      }
   }
}
