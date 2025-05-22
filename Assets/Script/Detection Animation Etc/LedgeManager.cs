using UnityEngine;

public class LedgeManager : MonoBehaviour
{
    public static LedgeManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public Vector2 ActiveLedgePosition { get; private set; }
    public Vector2 ActiveLedgeDownPosition { get; private set; }

    public void UpdateLedgePosition(Vector2 position)
    {
        ActiveLedgePosition = position; // Updates when player touches a new ledge
        
    }
    public void UpdateLedgeDownPosition(Vector2 position)
    {
        ActiveLedgeDownPosition = position; // Updates when player touches a new ledge
        
    }

    public void ClearLedge()
    {
        ActiveLedgePosition = Vector2.zero; // Clear ledge when player leaves
    }
    public void ClearLedgeDown()
    {
        ActiveLedgeDownPosition = Vector2.zero; // Clear ledge when player leaves
    }
}
