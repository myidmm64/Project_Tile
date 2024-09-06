using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackModule : PlayerModule
{
    [SerializeField]
    private PlayerWeapon _weapon = null;

    [SerializeField]
    private int _attackSkillID = 0;
    private SkillDataSO _attackSkillData = null;

    [SerializeField]
    private float _attackDelay = 0.5f;
    private float _attackTimer = 0f;
    protected override void Awake()
    {
        base.Awake();
        _player = GetComponent<PlayerDiceUnit>();
        _attackSkillData = Utility.GetSkillDataSO(_attackSkillID);
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
            SUseSkillData useSkillData = new SUseSkillData();
            useSkillData.owner = _player;
            useSkillData.direction = _player.GetDirection();
            useSkillData.specialActions = new Dictionary<string, System.Action>();
            useSkillData.specialActions.Add("SuccessAttack", SuccessAttack);
            _attackSkillData.GetSkill().UseSkill(useSkillData);
            _attackTimer = 0f;

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 dir = mousePos - ((Vector2)_player.transform.position + Vector2.up * 0.5f);

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, angle + 90);

            Vector2 newPosition = ((Vector2)_player.transform.position + Vector2.up * 0.5f) + (dir.normalized * 1f);

            _weapon.transform.SetPositionAndRotation(newPosition, rot);
            _weapon.PlayAni();

            _player.animator.Play($"Attack{_idx}");
            
            _idx = (_idx + 1) % 3;
            /*
            foreach(var attackTarget in GetAttackTargets())
            {
                IDamagable damagable = attackTarget.GetComponent<IDamagable>();
                damagable.Damage(_player.dice.dicePip);
                _player.animator.Play("NormalAttack");

                return; // 현재 리스트 0번째만 때리고 중단함, 나중에 타겟 설정 함수 나오면 교체 
            }
            */
        }
    }

    private void SuccessAttack()
    {
        _player.skillModule.IncreaseDP(20);
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
