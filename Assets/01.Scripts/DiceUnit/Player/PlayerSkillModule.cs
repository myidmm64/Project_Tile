using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSkillModule : PlayerModule
{
    [SerializeField]
    private float _dpAniDuration = 0.2f;

    public List<int> skillIDs = new List<int>();
    private Dictionary<EPlayerSkillType, PlayerSkillDataSO> _skillDatas = new();

    public int curDP = 0;

    public void SetSkill(int skillID)
    {
        PlayerSkillDataSO skillData = Utility.GetSkillDataSO(skillID) as PlayerSkillDataSO;
        var characterUI = MainUI.Inst.GetUIElement<CharacterUI>();

        switch (skillData.skillType)
        {
            case EPlayerSkillType.None:
                break;
            case EPlayerSkillType.Main:
                curDP = 0;
                characterUI.dpSlider.Initialize(skillData.maxDP);
                characterUI.dpSlider.SetValueImmediate(curDP);
                characterUI.mainSkillImage.sprite = skillData.skillImage;
                break;
            case EPlayerSkillType.Special:
                characterUI.specialSkillImage.sprite = skillData.skillImage;
                break;
            case EPlayerSkillType.Counter:
                characterUI.counterSkillImage.sprite = skillData.skillImage;
                break;
            default:
                break;
        }

        _skillDatas[skillData.skillType] = skillData;
        Debug.Log($"SET SKILL {skillData.skillType}, {skillID}");
    }

    public void IncreaseDP(int amount)
    {
        var data = _skillDatas[EPlayerSkillType.Main];

        curDP += amount;
        curDP = Mathf.Clamp(curDP, 0, data.maxDP);
        MainUI.Inst.GetUIElement<CharacterUI>().dpSlider.SetValueWithAnimation(curDP, _dpAniDuration);
    }

    public void UseMainSkill()
    {
        var data = _skillDatas[EPlayerSkillType.Main];
        if (curDP < data.maxDP) return;
        Skill skill = data.GetSkill();
        if(skill.UseSkill(_player))
        {
            _player.GetModule<PlayerAttackModule>().CurWeapon.UseSkill(skill);
            curDP = 0;
            MainUI.Inst.GetUIElement<CharacterUI>().dpSlider.SetValueWithAnimation(curDP, _dpAniDuration);
        }
    }

    public void SkillInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            UseMainSkill();
    }
}
