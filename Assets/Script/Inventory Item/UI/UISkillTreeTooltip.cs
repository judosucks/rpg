using System;
using TMPro;
using UnityEngine;

public class UISkillTreeTooltip : UITooltip
{

   [SerializeField] private TextMeshProUGUI skillNameText;
   [SerializeField] private TextMeshProUGUI skillDescriptionText;
   [SerializeField] private TextMeshProUGUI skillCost;
   private float defaultNameFontSize;

   private void Start()
   {
      gameObject.SetActive(false);
      defaultNameFontSize = skillNameText.fontSize;
   }


   public void ShowSkillTreeTooltip(string _skillDescription, string _skillName,int _skillCost)
   {
      Debug.Log("show enter");
      skillNameText.text = _skillName;
      skillDescriptionText.text = _skillDescription;
      skillCost.text = "Cost: "+_skillCost.ToString();
      AdjustTooltiposition();
      AdjustFontSize(skillNameText);
      gameObject.SetActive(true);
   }

   public void HideSkillTreeTooltip()
   {
      skillNameText.fontSize = defaultNameFontSize;
      gameObject.SetActive(false);
   }


}
