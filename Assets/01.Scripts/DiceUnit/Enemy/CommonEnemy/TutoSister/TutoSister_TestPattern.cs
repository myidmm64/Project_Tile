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
        var telegraph = PoolManager.Inst.Pop(EPoolType.DiceTelegraph) as DiceTelegraph;
        telegraph.StartTelepgraph(_diceUnit.diceGrid, _diceUnit.positionKey, 1f, null);
        _diceUnit.StartCoroutine(AnimationWaitAndDamage());
    }

    private IEnumerator AnimationWaitAndDamage()
    {
        _diceUnit.autoFlip = false;
        yield return new WaitForSeconds(0.4f);
        _diceUnit.animator.Play("Attack");
        yield return new WaitForSeconds(_diceUnit.animator.GetCurrentAnimatorStateInfo(0).length);
        _diceUnit.Damage(1);
        isEnded = true;
        _diceUnit.autoFlip = true;
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
