using System.Collections.Generic;
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

    public bool Attack(PlayerDiceUnit player, Vector2Int positionKey, EDirection direction, int idx = -1)
    {
        _player = player;
        if (player.diceGrid.grid.ContainsKey(positionKey) == false) return false;
        if (idx == -1) idx = _idx;

        Vector2 diceGroundPos = player.diceGrid.grid[positionKey].groundPos;
        transform.position = diceGroundPos + new Vector2(_addPos.x * GetScaleByDirection(direction), _addPos.y);
        //transform.position = diceGroundPos + _addPos;
        SetScaleByDirection(transform, direction);

        _animator.Play($"Attack{idx}");

        _curSkillData = new SUseSkillData();
        _curSkillData.owner = _player;
        _curSkillData.direction = direction;
        _curSkillData.spawnPositionKey = positionKey;
        _curSkillData.specialActions = new Dictionary<string, System.Action>();
        _curSkillData.specialActions.Add("SuccessAttack", () => _player.skillModule.IncreaseDP(20));
        _curSkillData.otherData = _idx;

        _idx = (_idx + 1) % 3;
        return true;
    }

    private void SetScaleByDirection(Transform trm, EDirection direction)
    {
        Vector3 localScale = trm.localScale;
        localScale.x = GetScaleByDirection(direction);
        trm.localScale = localScale;
    }

    private float GetScaleByDirection(EDirection direction)
    {
        return direction == EDirection.Right ? 1f : -1f;
    }

    public void SpawnAttackObj()
    {
        if (_player.diceGrid.grid.ContainsKey(_curSkillData.spawnPositionKey))
        {
            Skill skill = _attackSkillData.GetSkill();
            skill.UseSkill(_curSkillData);

            Vector2 addPos = _skillAddPos[(int)_curSkillData.otherData];
            skill.transform.position = _player.diceGrid.grid[_curSkillData.spawnPositionKey].groundPos + 
                new Vector3(addPos.x * GetScaleByDirection(_curSkillData.direction), addPos.y);

            skill.transform.rotation = Quaternion.Euler(0, 0, _skillAddRot[(int)_curSkillData.otherData] * GetScaleByDirection(_curSkillData.direction));

            SetScaleByDirection(skill.transform, _curSkillData.direction);
        }
    }
}
