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

        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.Initialize(_skillData.data.maxMP);
        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.SetValueImmediate(curMP);
        MainUI.Inst.GetUIElement<CharacterUI>().normalSkillImage.sprite = _skillData.data.SkillImage;
        Debug.Log($"Set Normal Skill / Skill Name : {_skillData.data.skillName}, Skill ID : {skillID}");
    }

    public void IncreaseMP(int amount)
    {
        curMP += amount;
        curMP = Mathf.Clamp(curMP, 0, _skillData.data.maxMP);
        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.SetValueWithAnimation(curMP, _mpSliderAnimationDuration);
    }

    public void UseSkill()
    {
        if (curMP < _skillData.data.maxMP) return;

        curMP = 0;
        MainUI.Inst.GetUIElement<CharacterUI>().mpSlider.SetValueWithAnimation(0, _mpSliderAnimationDuration);
        SUseSkillData useSkillData = new SUseSkillData();
        useSkillData.owner = _player;
        useSkillData.direction = _player.GetDirection();
        _skillData.GetSkill().UseSkill(useSkillData);
    }

    public void SkillInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            UseSkill();
    }
}
