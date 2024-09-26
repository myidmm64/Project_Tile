using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TutoSkeleton_TestPattern : EnemyPattern
{
    public TutoSkeleton_TestPattern(Enemy diceUnit) : base(diceUnit)
    {
    }

    public override bool CanStartPattern()
    {
        return true;
    }

    public override void Enter()
    {
        Action AttackAction = () =>
        {
            // _diceUnit.autoFlip = false;
            List<Vector2Int> attackRange = new List<Vector2Int>();
            int horizontalDiff = _enemy.GetPlayer().positionKey.x - _enemy.positionKey.x;
            if(horizontalDiff < 0)
            {
                attackRange.AddRange(PosKeyUtil.Line(_enemy.positionKey, EDirection.Left, 2));
                attackRange = attackRange.SubKeys(_enemy.positionKey).ToList();
            }
            else if (horizontalDiff > 0)
            {
                attackRange.AddRange(PosKeyUtil.Line(_enemy.positionKey, EDirection.Right, 2));
                attackRange = attackRange.SubKeys(_enemy.positionKey).ToList();
            }
            else
            {
                attackRange.AddRange(PosKeyUtil.Line(_enemy.positionKey, EDirection.Up, 2, true));
                attackRange = attackRange.SubKeys(_enemy.positionKey).ToList();
            }
            NormalAttack(0.5f, 0.1f, "Attack", attackRange, 1, () =>
            {
                isEnded = true;
                // _diceUnit.autoFlip = true;
            });
        };

        Vector2Int playerPosKey = _enemy.GetPlayer().positionKey;
        Vector2Int trackingPosKey = Utility.EDirectionToVector(PosKeyUtil.GetDirectionToTarget(_enemy.positionKey, playerPosKey));
        List<Vector2Int> moveKeys = new List<Vector2Int>();
        moveKeys.Add(_enemy.positionKey + trackingPosKey);
        moveKeys.Add(playerPosKey + Vector2Int.left);
        moveKeys.Add(playerPosKey + Vector2Int.right);
        moveKeys.Add(playerPosKey + Vector2Int.up);
        moveKeys.Add(playerPosKey + Vector2Int.down);

        bool moveSuccess = false;
        foreach(var moveKey in  moveKeys)
        {
            if (Move(moveKey, AttackAction))
            {
                moveSuccess = true;
                break;
            }
        }
        if(moveSuccess == false)
        {
            Debug.Log("½ÇÆÐ !!!!");
            isEnded = true;
        }
    }

    public override void Exit()
    {
        isEnded = false;
    }

    public override float GetCooltime()
    {
        return 1.5f;
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
