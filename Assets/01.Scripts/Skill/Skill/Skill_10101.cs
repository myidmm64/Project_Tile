using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_10101 : Skill
{
    private List<DiceUnit> _targets = new List<DiceUnit>();

    public override void UseSkill(DiceUnit owner)
    {
        if (_targets.Count == 0) return;
        DiceUnit target = _targets[0];
        StartCoroutine(DamageCoroutine(target, owner));
    }

    private IEnumerator DamageCoroutine(DiceUnit target, DiceUnit owner)
    {
        for(int i = 0; i < 10; i++)
        {
            owner.Attack(target, EAttackType.Physical, 120, out var cri);
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);
    }

    protected override bool ChildIsUsable(DiceUnit owner)
    {
        _targets = DiceGrid.Inst.GetIncludedDiceUnits(_rangeData, owner);
        return _targets.Count > 0;
    }
}
