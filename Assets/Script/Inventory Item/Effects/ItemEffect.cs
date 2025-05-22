
using UnityEngine;


public class ItemEffect : ScriptableObject
{
    [TextArea] public string effectDescription;
    public virtual void ExcutedEffect(Transform _enemyPosition)
    {
        Debug.Log("Item Effect");
    }
}
