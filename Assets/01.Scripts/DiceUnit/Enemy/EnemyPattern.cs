using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyPattern
{
    protected EnemyDiceUnit _diceUnit = null;
    public bool isEnded { get; protected set; }

    public EnemyPattern(EnemyDiceUnit diceUnit)
    {
        _diceUnit = diceUnit;
        Initialize();
    }

    // ���� �Ʒ����� ��� ��� SO data ����ϵ��� �����ϱ�. Load�ϵ���
    public abstract int GetPatternPriority();
    public abstract float GetCooltime();
    public abstract bool CanStartPattern();

    public abstract void Initialize();

    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
    
    protected void NormalAttack(float telegraphTime, float preWaitTime, string animationName, List<Vector2Int> attackRange, int damage, Action Callback)
    {
        _diceUnit.StartCoroutine(NormalAttackCoroutine(telegraphTime, preWaitTime, animationName, attackRange, damage, Callback));
    }

    private IEnumerator NormalAttackCoroutine(float telegraphTime, float preWaitTime, string animationName, List<Vector2Int> attackRange, int damage, Action Callback)
    {
        // �ڷ��׷��� ����
        foreach(var attackPositionKey in attackRange)
        {
            if(_diceUnit.diceGrid.grid.ContainsKey(attackPositionKey))
            {
                var telegraph = PoolManager.Inst.Pop(EPoolType.DiceTelegraph) as DiceTelegraph;
                telegraph.StartTelepgraph(_diceUnit.diceGrid, attackPositionKey, telegraphTime, null);
            }
        }

        yield return new WaitForSeconds(preWaitTime);

        _diceUnit.animator.Play(animationName);
        yield return new WaitForSeconds(_diceUnit.animator.GetCurrentAnimatorStateInfo(0).length);

        foreach (var attackPositionKey in attackRange)
        {
            if(_diceUnit.diceGrid.diceUnitGrid.TryGetValue(attackPositionKey, out var diceUnit))
            {
                if(diceUnit is PlayerDiceUnit) // �÷��̾�
                {
                    IDamagable damageable = diceUnit.GetComponent<IDamagable>();
                    if(damageable != null)
                    {
                        damageable.Damage(damage);
                    }
                }
            }
        }

        Callback?.Invoke();
    }
}