using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UICraftWindow : MonoBehaviour
{
  [SerializeField]private TextMeshProUGUI itemName;
  [SerializeField]private TextMeshProUGUI itemDescription;
  [SerializeField]private Image itemIcon;
  [SerializeField] private Image[] materialImages;
  [SerializeField]private Button craftButton;
  public void SetupCraftWindow(ItemDataEquipment _data)
  {
    craftButton.onClick.RemoveAllListeners();
    for (int i = 0; i < materialImages.Length; i++)
    {
      materialImages[i].color = Color.clear;
      materialImages[i].GetComponentInChildren<TextMeshProUGUI>();
    }

    for (int i = 0; i < _data.craftingMaterials.Count; i++)
    {
      if (_data.craftingMaterials.Count > materialImages.Length)
      {
        Debug.Log("Not enough slot return");
        return;
      }

      if (_data == null)
      {
        Debug.LogError("SetupCraftWindow: Passed _data is null!");
        return;
      }

      if (itemName == null)
      {
        Debug.LogError("SetupCraftWindow: itemName UI element not assigned!");
        return;
      }

      // 如果 _data 和 itemName 都存在，继续执行
      Debug.Log($"SetupCraftWindow: _data.itemName = {_data.itemName}");

      materialImages[i].sprite = _data.craftingMaterials[i].data.icon;
      materialImages[i].color = Color.white;
      TextMeshProUGUI materialSlotText = materialImages[i].GetComponentInChildren<TextMeshProUGUI>();
      
      materialSlotText.text= _data.craftingMaterials[i].stackSize.ToString();
      materialSlotText.color = Color.white;
    }
     itemIcon.sprite = _data.icon;
     itemName.text = _data.itemName;
     itemDescription.text = _data.GetDescription();
     craftButton.onClick.AddListener(()=> Inventory.instance.CanCraft(_data,_data.craftingMaterials));
  }
  
}
