using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    [SerializeField, TextArea]
    protected string _atkRange = string.Empty;

    public abstract EWeaponType WeaponType { get; protected set; } // 이거에 따라서 공격 방식 달라짐
    protected Player _player = null;
    [SerializeField]
    protected int _skillID = 0;
    protected SkillDataSO _skillData = null;

    // 무기마다 카운터 애니메이션 들어가야함.

    private void Awake()
    {
        _skillData = Utility.GetSkillDataSO(_skillID); // 현재 무기에 연경된 스킬 가져오기
    }

    public abstract bool IsAttackable();
    public abstract void Attack();

    /*
    public bool Attack(Player player, Vector2Int positionKey, EDirection direction)
    {
        _player = player;
        if (player.grid.dices.ContainsKey(positionKey) == false) return false;

        // 방향 및 position 세팅
        Vector2 diceGroundPos = player.grid.dices[positionKey].groundPos;
        transform.position = diceGroundPos + new Vector2(_addPos.x * Utility.DirectionToXMiltiflier(direction), _addPos.y);
        Utility.SetLocalScaleByDirection(transform, direction);

        _animator.Play($"Attack{_idx}");

        _useSkillData = new SUseSkillData();
        _useSkillData.owner = _player;
        _useSkillData.direction = direction;
        _useSkillData.spawnPositionKey = positionKey;

        Action attackCallback = () => { _player.GetModule<PlayerSkillModule>().IncreaseDP(20); };

        Vector2 skillAddPos = _skillAddPos[_idx];
        Vector2 skillPosition = diceGroundPos + new Vector2(skillAddPos.x * Utility.DirectionToXMiltiflier(direction), skillAddPos.y);
        Quaternion skillRotation = Quaternion.Euler(0, 0, _skillAddRot[_idx] * Utility.DirectionToXMiltiflier(direction));

        Dictionary<string, object> otherDatas = new Dictionary<string, object>();
        otherDatas.Add("AttackCallback", attackCallback);
        otherDatas.Add("Position", skillPosition);
        otherDatas.Add("Rotation", skillRotation);
        otherDatas.Add("Index", _idx);

        _useSkillData.otherDatas = otherDatas;
        _idx = (_idx + 1) % 3;
        return true;
    }

    public void SpawnAttackObj()
    {
        if (_player.grid.dices.ContainsKey(_useSkillData.spawnPositionKey))
        {
            Skill skill = _skillData.GetSkill();
            skill.UseSkill(_useSkillData);
        }
    }
    */
}
