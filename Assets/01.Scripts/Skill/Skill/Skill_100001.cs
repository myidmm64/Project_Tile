using UnityEngine;

public class Skill_100001 : Skill
{
    public override bool IsUsable(DiceUnit owner)
    {
        return true;
    }

    protected override void SkillLogic(DiceUnit owner)
    {
    }

    public void Attack(RangeDataSO rangeData)
    {
        Debug.Log("ATTACk!!!");
    }
}
