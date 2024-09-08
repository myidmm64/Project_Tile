using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Animator _animator = null;

    [SerializeField]
    private Vector2 _addPos = Vector2.zero;

    [SerializeField]
    private List<Vector2> _skillAddPos = new List<Vector2>();
    [SerializeField]
    private List<int> _skillAddRot = new List<int>();

    private int _idx = 0;
    private PlayerDiceUnit _player = null;

    [SerializeField]
    private int _attackSkillID = 0;
    private SkillDataSO _attackSkillData = null;
    private SUseSkillData _curSkillData;

    private void Awake()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _attackSkillData = Utility.GetSkillDataSO(_attackSkillID);
    }

    public bool Attack(PlayerDiceUnit player, Vector2Int positionKey, EDirection direction)
    {
        _player = player;
        if (player.diceGrid.grid.ContainsKey(positionKey) == false) return false;

        // 방향 및 position 세팅
        Vector2 diceGroundPos = player.diceGrid.grid[positionKey].groundPos;
        transform.position = diceGroundPos + new Vector2(_addPos.x * Utility.DirectionToXMiltiflier(direction), _addPos.y);
        Utility.SetLocalScaleByDirection(transform, direction);

        _animator.Play($"Attack{_idx}");

        _curSkillData = new SUseSkillData();
        _curSkillData.owner = _player;
        _curSkillData.direction = direction;
        _curSkillData.spawnPositionKey = positionKey;

        Action attackCallback = () => { _player.skillModule.IncreaseDP(20); };

        Vector2 skillAddPos = _skillAddPos[_idx];
        Vector2 skillPosition = diceGroundPos + new Vector2(skillAddPos.x * Utility.DirectionToXMiltiflier(direction), skillAddPos.y);
        Quaternion skillRotation = Quaternion.Euler(0, 0, _skillAddRot[_idx] * Utility.DirectionToXMiltiflier(direction));

        Dictionary<string, object> otherDatas = new Dictionary<string, object>();
        otherDatas.Add("AttackCallback", attackCallback);
        otherDatas.Add("Position", skillPosition);
        otherDatas.Add("Rotation", skillRotation);
        otherDatas.Add("Index", _idx);

        _curSkillData.otherDatas = otherDatas;
        _idx = (_idx + 1) % 3;
        return true;
    }

    public void SpawnAttackObj()
    {
        if (_player.diceGrid.grid.ContainsKey(_curSkillData.spawnPositionKey))
        {
            Skill skill = _attackSkillData.GetSkill();
            skill.UseSkill(_curSkillData);
        }
    }
}
