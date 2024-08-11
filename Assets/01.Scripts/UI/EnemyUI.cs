using TMPro;
using UnityEngine;

public class EnemyUI : MainUIElement
{
    public TextMeshProUGUI nameText { get; private set; }
    public SliderUI hpSlider { get;private set; }

    public override void BindUI()
    {
        nameText = transform.FindComponentInChildren<TextMeshProUGUI>("EnemyHPBar/EnemyNameText");
        hpSlider = transform.FindComponentInChildren<SliderUI>("EnemyHPBar/HpSlider");
    }
}
