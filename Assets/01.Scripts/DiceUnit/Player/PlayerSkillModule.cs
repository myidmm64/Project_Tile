using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSkillModule : PlayerModule
{
    [SerializeField]
    private float _dpSliderAnimationDuration = 0.2f;

    public List<int> skillIDs = new List<int>();
    private Dictionary<int, PlayerSkillDataSO> _skillDatas = new Dictionary<int, PlayerSkillDataSO>();

    public int curDP = 0;

    private void Start()
    {
        foreach(var skillID in skillIDs)
        {
            var skillData = Utility.GetSkillDataSO(skillID);
            _skillDatas.Add(skillID, skillData as PlayerSkillDataSO);
        }
        SetMainSkill(skillIDs[0]);
        SetSpecialSkill(skillIDs[1]);
        SetCounterSkill(skillIDs[2]);
    }

    public void SetMainSkill(int skillID) => SetSkill(skillID, "Main");
    public void SetSpecialSkill(int skillID) => SetSkill(skillID, "Special");
    public void SetCounterSkill(int skillID) => SetSkill(skillID, "Counter");

    public void SetSkill(int skillID, string skillType)
    {
        var data = _skillDatas[skillID];
        var characterUI = MainUI.Inst.GetUIElement<CharacterUI>();

        switch (skillType)
        {
            case "Main":
                characterUI.dpSlider.Initialize(data.maxDP);
                characterUI.dpSlider.SetValueImmediate(curDP);
                characterUI.mainSkillImage.sprite = data.skillImage;
                break;

            case "Special":
                characterUI.specialSkillImage.sprite = data.skillImage;
                break;

            case "Counter":
                characterUI.counterSkillImage.sprite = data.skillImage;
                break;
        }

        Debug.Log($"Set {skillType} Skill / Skill Name : {data.skillName}, Skill ID : {skillID}");
    }

    public void IncreaseDP(int amount)
    {
        var data = _skillDatas[skillIDs[0]];

        curDP += amount;
        curDP = Mathf.Clamp(curDP, 0, data.maxDP);
        MainUI.Inst.GetUIElement<CharacterUI>().dpSlider.SetValueWithAnimation(curDP, _dpSliderAnimationDuration);
    }

    public void UseMainSkill()
    {
        var data = _skillDatas[skillIDs[0]];

        if (curDP < data.maxDP) return;

        curDP = 0;
        MainUI.Inst.GetUIElement<CharacterUI>().dpSlider.SetValueWithAnimation(0, _dpSliderAnimationDuration);
        
        /*SUseSkillData useSkillData = new SUseSkillData();
        useSkillData.owner = _player;
        useSkillData.direction = _player.sprite.direction;
        data.GetSkill().UseSkill(useSkillData);*/

    }

    public void SkillInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            UseMainSkill();
    }
}
