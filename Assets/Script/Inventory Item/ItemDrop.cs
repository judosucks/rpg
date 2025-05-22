using UnityEngine;
using System.Collections.Generic;
public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleDropItem;
    [SerializeField] private ItemData[] possibleItems;
    private List<ItemData> dropList = new List<ItemData>();
    [SerializeField] private GameObject dropPrefab;


    public virtual void GenerateDrop()
    {
        

        for (int i = 0; i < possibleItems.Length; i++)
        {
            if (Random.Range(0, 100) <= possibleItems[i].dropChance)
            {
                dropList.Add(possibleItems[i]);
            }

            for (int j = 0; j < possibleDropItem; j++)
            {
                if (dropList.Count == 0)
                {
                    Debug.LogWarning("Drop list is empty. Skipping GenerateDrop.");
                    break;
                }
                ItemData randomItem = dropList[Random.Range(0, dropList.Count -1)];
                dropList.Remove(randomItem);
                DropItem(randomItem);
            }
        }
    }
    protected void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-5f, 5f), Random.Range(15f, 20f));
        newDrop.GetComponent<ItemObjects>().SetupItem(_itemData, randomVelocity);
    }

}
