using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackModule : PlayerModule
{
    [SerializeField]
    private int _weaponID = 0;
    private PlayerWeaponDataSO _curWeaponData = null;
    private PlayerWeapon _curWeapon = null;
    
    [SerializeField]
    private float _attackDelay = 0.5f;
    private float _attackTimer = 0f;

    private bool _attackKeyPress = false;

    protected override void Awake()
    {
        base.Awake();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        ChangeWeapon(_weaponID);
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;
        if (_attackKeyPress) Attack();
    }

    public void ChangeWeapon(int weaponID)
    {
        _curWeaponData = Utility.GetPlayerWeaponDataSO(weaponID);
        _curWeapon = _curWeaponData.GetWeapon();
        _curWeapon.BindWeapon(_player);
    }

    public void Attack()
    {
        if (_player.isMoving || _curWeapon == null) return;
        if (_attackTimer >= _attackDelay && _curWeapon.IsAttackable())
        {
            _curWeapon.Attack();
            _attackTimer = 0f;
        }
    }

    public List<DiceUnit> GetAttackTargets() // 이거 삭제하셈
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
            if (_player.grid.units.ContainsKey(attackPositionKey))
            {
                IDamagable damagable = _player.grid.units[attackPositionKey].GetComponent<IDamagable>();
                if (damagable != null)
                {
                    result.Add(_player.grid.units[attackPositionKey]);
                }
            }
        }

        return result;
    }

    public void AttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _attackKeyPress = true;
        }
        else if (context.canceled)
        {
            _attackKeyPress= false;
        }
    }
}
