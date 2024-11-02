using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_10101 : Skill
{
    [SerializeField]
    private float _percentDamage = 120f;
    private Animator _animator = null;

    protected override void SkillLogic(DiceUnit owner)
    {
        if (_targets.Count == 0) return;
        DiceUnit target = _targets[0];
        owner.Attack(target, EAttackType.Physical, _percentDamage, out var cri);
        PlayAndAction(_animator, "Attack", () => Destroy(gameObject));
    }

    private void Awake()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
    }
}
