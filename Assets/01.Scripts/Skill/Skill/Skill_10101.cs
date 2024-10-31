using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_10101 : Skill
{
    private List<DiceUnit> _targets = new List<DiceUnit>();
    [SerializeField]
    private float _percentDamage = 120f;
    private Animator _animator = null;

    private void Awake()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
    }

    public override void UseSkill(DiceUnit owner)
    {
        if (_targets.Count == 0) return;
        DiceUnit target = _targets[0];
        owner.Attack(target, EAttackType.Physical, _percentDamage, out var cri);
        PlayAndAction(_animator, "Attack", () => Destroy(gameObject));
    }

    protected override bool ChildIsUsable(DiceUnit owner)
    {
        _targets = DiceGrid.Inst.GetIncludedDiceUnits(_rangeData, owner);
        return _targets.Count > 0;
    }
}
