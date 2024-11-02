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
    public List<DiceUnit> attackTargets => _attackTargets;

    protected virtual void Awake()
    {
        _skillData = Utility.GetSkillDataSO(_data.skillID); // ���� ���⿡ ����� ��ų ��������
    }

    protected virtual void Update()
    {
        _attackTargets.Clear();
        UpdateAttackTargets();
    }

    public virtual void BindWeapon(Player player)
    {
        _player = player;
        _player.GetModule<PlayerSkillModule>().SetSkill(_data.skillID);
    }

    public virtual bool IsAttackable()
    {
        return _attackTargets.Count > 0;
    }

    protected abstract void UpdateAttackTargets();
    public abstract void Attack(); // �⺻ ���� ����
    public abstract void UseSkill(Skill skill);
}
