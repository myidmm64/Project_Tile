using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

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

    // �⺻ ������
    protected virtual bool Move(Vector2Int dest, Action Callback)
    {
        bool moved = _diceUnit.ChangeDice(dest);
        if (moved)
        {
            Sequence _moveSeq = DOTween.Sequence();
            _moveSeq.Append(_diceUnit.transform.DOMove(_diceUnit.dice.groundPos, 0.15f)).SetEase(Ease.Linear);
            _moveSeq.AppendCallback(() =>
            {
                Callback?.Invoke();
            });
        }

        return moved;
    }
    
    protected virtual void NormalAttack(float telegraphTime, float preWaitTime, string animationName, List<Vector2Int> attackRange, int damage, Action Callback)
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
                telegraph.StartTelepgraph(_diceUnit.diceGrid, attackPositionKey, telegraphTime, ()=>
                {
                    List<Vector2Int> telegraphPosKey = new List<Vector2Int>();
                    telegraphPosKey.Add(attackPositionKey);
                    var units = _diceUnit.diceGrid.GetDiceUnits(telegraphPosKey);
                    foreach (var unit in units)
                    {
                        if (unit is Player) // �÷��̾�
                        {
                            Player player = unit as Player;
                            player.Damage(damage);
                        }
                    }
                    Callback?.Invoke();
                });
            }
        }

        yield return new WaitForSeconds(preWaitTime);
        // _diceUnit.animator.Play(animationName);
    }
}