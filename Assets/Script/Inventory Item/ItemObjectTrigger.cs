using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    private ItemObjects myItemObject => GetComponentInParent<ItemObjects>();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (other.GetComponent<CharacterStats>().isDead)
                return;
            Debug.Log("Picked up item");
            myItemObject.PickUpItem();
        }
    }
}
