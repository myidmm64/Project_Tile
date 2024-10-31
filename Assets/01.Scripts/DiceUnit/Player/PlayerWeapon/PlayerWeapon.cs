using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class PlayerWeapon : MonoBehaviour
{
    protected Player _player = null;
    [SerializeField]
    protected PlayerWeaponDataSO _data = null;
    public PlayerWeaponDataSO Data => _data;
    
    protected SkillDataSO _skillData = null;
    protected List<DiceUnit> _attackTargets = new List<DiceUnit>();

    protected virtual void Awake()
    {
        _skillData = Utility.GetSkillDataSO(_data.skillID); // 현재 무기에 연결된 스킬 가져오기
    }

    protected virtual void Update()
    {
        _attackTargets.Clear();
        UpdateAttackTargets();
        LookTarget();
    }

    private void LookTarget()
    {
        if (_attackTargets.Count > 0)
        {
            _player.sprite.LookAt(_attackTargets[0].positionKey);
        }
        else
        {
            _player.sprite.LookAt(_player.grid.FindClosestUnit<DiceUnit>(_player.positionKey));
        }
    }

    public void BindWeapon(Player player)
    {
        _player = player;
        _player.GetModule<PlayerSkillModule>().SetSkill(_data.skillID);
    }

    protected abstract void UpdateAttackTargets();
    public virtual bool IsAttackable()
    {
        return _attackTargets.Count > 0;
    }
    public abstract void Attack();
    public abstract void UseSkill();
}
