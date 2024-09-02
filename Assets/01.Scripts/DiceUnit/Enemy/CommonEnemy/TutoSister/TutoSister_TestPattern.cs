using System.Collections;
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
        _diceUnit.autoFlip = false;
        NormalAttack(1f, 0.4f, "Attack", new System.Collections.Generic.List<Vector2Int>() { new Vector2Int(1, 1) }, 1, ()=>
        {
            isEnded = true;
            _diceUnit.autoFlip = true;
        });;
    }

    public override void Exit()
    {
        isEnded = false;
    }

    public override float GetCooltime()
    {
        return 2f;
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
