using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class UISkillTreeSlot : MonoBehaviour,IPointerEnterHandler,
   IPointerExitHandler,ISaveManager
{
   private SkillManager skillManager;
   private UI ui;
   [SerializeField]private int skillCost;
   [SerializeField]private string skillName;
   [SerializeField]private string skillDescription;
   [SerializeField] private Color lockedSkillColor;
   public bool unlocked;
   [SerializeField] private UISkillTreeSlot[] shouldBeLocked;
   [SerializeField] private UISkillTreeSlot[] shouldBeUnlocked;
   private Image skillImage;
   public static bool IsInitialized { get; private set; } = false; // Check if initialized

   private void OnValidate()
   {
      gameObject.name = "UI Skill Tree Slot:"+skillName;
   }

   private void Awake()
   {
      skillManager = SkillManager.instance;
      InitializeSkillTreeSlot();
      GetComponent<Button>().onClick.AddListener(()=>UnlockSkillSlot());
      
   }

   private void Start()
   {
      ui = GetComponentInParent<UI>();
      skillImage = GetComponent<Image>();
      skillImage.color = lockedSkillColor;
      if (unlocked)
      {
         skillImage.color = Color.white;
         
      }
   }
   private void InitializeSkillTreeSlot()
   {
      // Perform any necessary initialization
      IsInitialized = true; // Mark as initialized
      
   }
   public void UnlockSkillSlot()
   {
      if (PlayerManager.instance.HaveEnoughExperience(skillCost) == false)
      {
         Debug.LogWarning("not enough experience");
         
         return;
      }
      
      for (int i = 0; i < shouldBeUnlocked.Length; i++)
      {
         if (shouldBeUnlocked[i].unlocked == false)
         {
            
            
            return;
         }
      }

      for (int i = 0; i < shouldBeLocked.Length; i++)
      {
         if (shouldBeLocked[i].unlocked == true)
         {

            Debug.Log("unlocked"+shouldBeLocked[i].name.ToString());
            return;
         }
      }
      unlocked = true;
      skillImage.color = Color.white;
      
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      ui.skillTreeTooltip.ShowSkillTreeTooltip(skillDescription, skillName,skillCost);
      
   }
   

   public void OnPointerExit(PointerEventData eventData)
   {
      ui.skillTreeTooltip.HideSkillTreeTooltip();
   }

   public void LoadData(GameData _data)
   {
      if (_data.skillTree.TryGetValue(skillName, out bool value))
      {
         Debug.LogWarning("loading skill tree data");
         unlocked = value;
      }
   }

   public void SaveData(ref GameData _data)
   {
      Debug.LogWarning("saving skill tree data");
      if (_data.skillTree.TryGetValue(skillName, out bool value))
      {
         _data.skillTree.Remove(skillName);
         _data.skillTree.Add(skillName,unlocked);
         
         Debug.LogWarning("saving skill tree data 2");
      }
      else
      {
         Debug.LogWarning("saving skill tree data else");
         _data.skillTree.Add(skillName, unlocked);
      }
   }
}
