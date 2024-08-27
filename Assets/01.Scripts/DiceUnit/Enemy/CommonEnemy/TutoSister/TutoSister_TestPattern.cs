using UnityEngine;

public class TutoSister_TestPattern : EnemyPattern
{
    public TutoSister_TestPattern(EnemyDiceUnit diceUnit) : base(diceUnit)
    {
    }

    public override bool CanStartPattern()
    {
        return true;
    }

    public override void Enter()
    {
        isEnded = true;
    }

    public override void Exit()
    {
        isEnded = false;
    }

    public override float GetCooltime()
    {
        return 1.0f;
    }

    public override int GetPatternPriority()
    {
        return 0;
    }

    public override void Initialize()
    {

    }

    public override void Update()
    {

    }
}
