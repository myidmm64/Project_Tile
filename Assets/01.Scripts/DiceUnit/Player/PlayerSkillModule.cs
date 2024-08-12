using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSkillModule : PlayerModule
{
    [SerializeField]
    private float _mpSliderAnimationDuration = 0.2f;
    public int curMP = 0;
    public int maxMP = 100;

    private void Start()
    {
        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.Initialize(maxMP);
        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.SetValueImmediate(0);
    }

    public void IncreaseMP(int amount)
    {
        curMP += amount;
        curMP = Mathf.Clamp(curMP, 0, maxMP);
        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.SetValueWithAnimation(curMP, _mpSliderAnimationDuration);
    }

    public void UseSkill()
    {
        if (curMP < maxMP) return;

        curMP = 0;
        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.SetValueWithAnimation(0, _mpSliderAnimationDuration);
        Debug.Log("SKILL");
    }

    public void SkillInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            UseSkill();
    }
}
