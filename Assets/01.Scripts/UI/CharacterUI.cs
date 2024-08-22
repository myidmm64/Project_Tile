using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MainUIElement
{
    public SliderUI dpSlider { get; private set; }
    public SliderUI hpSlider { get; private set; }
    public Image mainSkillImage { get; private set; }
    public Image specialSkillImage { get; private set; }
    public Image counterSkillImage { get; private set; }

    public override void BindUI()
    {
        dpSlider = transform.FindComponentInChildren<SliderUI>("DPSlider");
        hpSlider = transform.FindComponentInChildren<SliderUI>("HPSlider");
        mainSkillImage = transform.FindComponentInChildren<Image>("Skill/MainSkillImage");
        specialSkillImage = transform.FindComponentInChildren<Image>("Skill/SpecialSkillImage");
        counterSkillImage = transform.FindComponentInChildren<Image>("Skill/CounterSkillImage");
    }
}
