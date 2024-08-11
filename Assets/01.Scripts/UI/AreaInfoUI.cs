using TMPro;
using UnityEngine;

public class AreaInfoUI : MainUIElement
{
    public TextMeshProUGUI areaNameText { get; private set; }
    
    public string otherInfoText = string.Empty;

    public override void BindUI()
    {
        areaNameText = transform.FindComponentInChildren<TextMeshProUGUI>("BG/AreaText");
    }

    public void SetAreaName(string areaName)
    {
        areaNameText.SetText(areaName + $"\n{otherInfoText}");
    }
}
