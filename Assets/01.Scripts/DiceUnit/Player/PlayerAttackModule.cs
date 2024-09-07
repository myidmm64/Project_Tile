using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackModule : PlayerModule
{
    [SerializeField]
    private PlayerWeapon _weapon = null;

    [SerializeField]
    private float _attackDelay = 0.5f;
    private float _attackTimer = 0f;
    protected override void Awake()
    {
        base.Awake();
        _player = GetComponent<PlayerDiceUnit>();
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;

        if(Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private int _idx = 0;

    public void Attack()
    {
        if (_player.moveModule.isMoving) return;
        if (_attackTimer >= _attackDelay)
        {
            _weapon.Attack(_player, _player.positionKey + Vector2Int.right, _player.GetDirection());
            _player.animator.Play($"Attack{_idx}");
            _idx = (_idx + 1) % 3;
            _attackTimer = 0f;
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
