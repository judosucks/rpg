using System;
using UnityEngine;

public class LedgeTriggerDetection : MonoBehaviour
{
    public static bool isTouchingLedge { get; private set; }
    public static Vector2 ledgePosition; // 用于记录悬崖的具体位置
    private Player player;
    private PlayerData playerData => PlayerManager.instance.playerData;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Make sure we are detecting only objects with exact criteria (Ledge layer specifically)
        if (other.gameObject.layer == LayerMask.NameToLayer("Ledge"))
        {
            float distanceToLedge = Vector2.Distance(transform.position, other.transform.position);

            // Adjust this threshold to the desired range of ledge detection
            if (distanceToLedge < playerData.ledgeDistance) // Example: Ensure object is within 1 unit distance of the ledge
            {
                Debug.Log("Touching Ledge within valid range.");
                isTouchingLedge = true;
                ledgePosition = other.ClosestPoint(transform.position);
                LedgeManager.Instance.UpdateLedgePosition(ledgePosition);
            }
            else
            {
                Debug.LogWarning("Ledge detected but too far away - ignoring!");
            }
        }
        

        // 检测是否碰到 Player
        // if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        // {
        //     Debug.Log("touching ledge player");
        //     isTouchingLedge = true;
        //     ledgePosition = other.ClosestPoint(transform.position); // Get this ledge's position
        //     LedgeManager.Instance.UpdateLedgePosition(ledgePosition); // Update the manager
        //
        // }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 离开悬崖
        if (other.gameObject.layer == LayerMask.NameToLayer("Ledge"))
        {
            Debug.Log("Not Touching Ledge");
            isTouchingLedge = false;
            LedgeManager.Instance.ClearLedge(); // Clear active ledge
        }
       
        // if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        // {
        //     Debug.Log("Not Touching Ledge player");
        //     isTouchingLedge = false;
        //     LedgeManager.Instance.ClearLedge(); // Clear active ledge
        // }

    }
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.magenta;
    //     Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().size);
    // }
    
}
