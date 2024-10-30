using System.Collections.Generic;
using UnityEngine;

public class Skill_10101 : Skill
{
    private List<DiceUnit> _targets = new List<DiceUnit>();

    public override void UseSkill(DiceUnit owner)
    {
        if (_targets.Count == 0) return;
        DiceUnit target = _targets[0];

    }

    protected override bool ChildIsUsable(DiceUnit owner)
    {
        _targets = DiceGrid.Inst.GetIncludedDiceUnits(_rangeData, owner);
        return _targets.Count > 0;
    }
}
