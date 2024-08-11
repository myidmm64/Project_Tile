using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerAttackModule : MonoBehaviour
{
    // �÷��̾� �⺻ ����
    private PlayerDiceUnit _player = null;

    [SerializeField]
    private float _attackDelay = 0.5f;
    private float _attackTimer = 0f;
    private void Awake()
    {
        _player = GetComponent<PlayerDiceUnit>();
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;
    }

    public void Attack()
    {
        if (_player.moveModule.isMoving) return;
        if (_attackTimer >= _attackDelay)
        {
            foreach(var attackTarget in GetAttackTargets())
            {
                IDamagable damagable = attackTarget.GetComponent<IDamagable>();
                damagable.Damage(_player.dice.dicePip);
                _player.animator.Play("NormalAttack");
                _attackTimer = 0f;

                return; // ���� ����Ʈ 0��°�� ������ �ߴ���, ���߿� Ÿ�� ���� �Լ� ������ ��ü 
            }
        }
    }

    public List<DiceUnit> GetAttackTargets()
    {
        List<Vector2Int> attackRanges = new List<Vector2Int>()
            {
                Vector2Int.left,
                Vector2Int.right
            };

        List<DiceUnit> result = new List<DiceUnit>();
        foreach (var attackRange in attackRanges)
        {
            Vector2Int attackPositionKey = _player.positionKey + attackRange;
            if (_player.diceGrid.diceUnitGrid.ContainsKey(attackPositionKey))
            {
                IDamagable damagable = _player.diceGrid.diceUnitGrid[attackPositionKey].GetComponent<IDamagable>();
                if (damagable != null)
                {
                    result.Add(_player.diceGrid.diceUnitGrid[attackPositionKey]);
                }
            }
        }

        return result;
    }

    public void AttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            Attack();
    }
}
