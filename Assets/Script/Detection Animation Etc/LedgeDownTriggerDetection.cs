using UnityEngine;

public class LedgeDownTriggerDetection : MonoBehaviour
{
    public BoxCollider2D ledgeCollider;
    public Vector2 originalOffset;
    public bool isTouchingLedgeDown;
    public Vector2 ledgePositionDown;

    private void Start()
    {
        if (ledgeCollider == null)
            ledgeCollider = GetComponent<BoxCollider2D>();
        
        originalOffset = ledgeCollider.offset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Ledge")) return;

        // Check distance to avoid false positives
        float distance = Vector2.Distance(transform.position, other.transform.position);
        if (distance < 1f)
        {
            isTouchingLedgeDown = true;
            Debug.LogWarning("istouchingledgedown"+isTouchingLedgeDown);
            ledgePositionDown = other.ClosestPoint(transform.position);

            // Get the ledge's offset if needed (ensure it has a BoxCollider2D)
            var ledgeCol = other.GetComponent<BoxCollider2D>();
            if (ledgeCol != null)
            {
                Debug.LogWarning("ledge collider not null");
                ledgeCollider.offset = ledgeCol.offset; // Apply immediately
            }
            else
            {
                Debug.LogWarning("no collider");
            }

            // Notify LedgeManager
            LedgeManager.Instance?.UpdateLedgeDownPosition(ledgePositionDown);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isTouchingLedgeDown && other.gameObject.layer == LayerMask.NameToLayer("Ledge"))
        {
            // Continuously update if needed (e.g., moving ledges)
            ledgeCollider.offset = other.GetComponent<BoxCollider2D>().offset;
            ledgePositionDown = other.ClosestPoint(transform.position);
            LedgeManager.Instance?.UpdateLedgeDownPosition(ledgePositionDown);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ledge"))
        {
            isTouchingLedgeDown = false;
            
            LedgeManager.Instance?.ClearLedgeDown();
        }
    }
}