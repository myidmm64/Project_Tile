using UnityEngine;

public class CharacterUI : MainUIElement
{
    public SliderUI mpSlider { get; private set; }
    public SliderUI hpSlider { get; private set; }

    public override void BindUI()
    {
        mpSlider = transform.FindComponentInChildren<SliderUI>("MPSlider");
        hpSlider = transform.FindComponentInChildren<SliderUI>("HPSlider");
    }
}
