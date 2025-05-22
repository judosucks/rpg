using TMPro;
using UnityEngine;

public class UIStatTooltip : UITooltip
{
    [SerializeField]private TextMeshProUGUI description;



    public void ShowStatTooltip(string _text)
    {
        description.text = _text;
        AdjustTooltiposition();
        gameObject.SetActive(true);
    }
    public void HideStatTooltip()
    {
        description.text = "";
        gameObject.SetActive(false);
    }
}
