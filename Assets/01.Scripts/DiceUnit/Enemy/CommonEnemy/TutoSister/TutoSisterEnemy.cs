using UnityEngine;

public class TutoSisterEnemy : BossEnemyDiceUnit
{
    protected override void BindPattern()
    {
        _patterns.Add(new TutoSister_TestPattern(this));
    }
}
