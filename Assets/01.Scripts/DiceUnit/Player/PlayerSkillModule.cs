using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSkillModule : PlayerModule
{
    [SerializeField]
    private float _mpSliderAnimationDuration = 0.2f;

    [SerializeField]
    private int _skillID = 0;
    private SkillDataSO _skillData = null;

    public int curMP = 0;

    private void Start()
    {
        SetNormalSkill(_skillID);
    }

    public void SetNormalSkill(int skillID)
    {
        _skillData = Utility.GetSkillDataSO(skillID);

        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.Initialize(_skillData.MaxMP);
        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.SetValueImmediate(curMP);
        MainUI.Inst.GetUIElement<CharacterUI>().normalSkillImage.sprite = _skillData.SkillImage;
        Debug.Log($"Set Normal Skill / Skill Name : {_skillData.SkillName}, Skill ID : {skillID}");
    }

    public void IncreaseMP(int amount)
    {
        curMP += amount;
        curMP = Mathf.Clamp(curMP, 0, _skillData.MaxMP);
        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.SetValueWithAnimation(curMP, _mpSliderAnimationDuration);
    }

    public void UseSkill()
    {
        if (curMP < _skillData.MaxMP) return;

        curMP = 0;
        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.SetValueWithAnimation(0, _mpSliderAnimationDuration);
        EDirection direction = _player.spriteRenderer.flipX ? EDirection.Left : EDirection.Right; // 나중에 타겟을 바라보도록 수정
        _skillData.GetSkill().UseSkill(_player, _player.diceGrid, direction);
    }

    public void SkillInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            UseSkill();
    }
}
