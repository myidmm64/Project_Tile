using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MainUIElement
{
    public SliderUI mpSlider { get; private set; }
    public SliderUI hpSlider { get; private set; }
    public Image normalSkillImage { get; private set; }

    public override void BindUI()
    {
        mpSlider = transform.FindComponentInChildren<SliderUI>("MPSlider");
        hpSlider = transform.FindComponentInChildren<SliderUI>("HPSlider");
        normalSkillImage = transform.FindComponentInChildren<Image>("Skill/NormalSkillImage");
    }
}
