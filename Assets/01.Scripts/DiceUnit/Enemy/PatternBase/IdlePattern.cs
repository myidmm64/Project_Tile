using System.Collections;
using UnityEngine;

public class IdlePattern : EnemyPattern
{
    private IdlePatternDataSO _iData => data as IdlePatternDataSO;

    public override void Enter()
    {
        StartCoroutine(IdleCoroutine());
    }

    private IEnumerator IdleCoroutine()
    {
        yield return new WaitForSeconds(_iData.idleDuration);
        isEnded = true;
    }

    public override void Exit()
    {
    }

    public override void PatternUpdate()
    {
    }

    public override bool Startable()
    {
        return true;
    }
}
