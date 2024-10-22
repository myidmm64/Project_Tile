using UnityEngine;

public class WeaponSword : PlayerWeapon
{
    private int _atkIdx = 0;

    private SpriteRenderer _renderer = null;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _renderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _animator = _renderer.GetComponent<Animator>();
    }

    public override void Attack()
    {
        DiceUnit target = _attackTargets[0];
        Vector2 dir = target.positionKey - _player.positionKey;
        _player.playerSprite.AdvanceMove(dir * 0.35f, 0.1f);
        target.Damage(1);

        _player.playerSprite.AttackAnimation(_atkIdx);
        AttackAnimation(_atkIdx);
        _atkIdx = (_atkIdx + 1) % 3;
    }

    protected override void UpdateAttackTargets()
    {
        foreach(var posKey in PosKeyUtil.StrPattern(_player.positionKey, _data.atkRange))
        {
            if(DiceGrid.Inst.units.TryGetValue(posKey, out var unit))
            {
                _attackTargets.Add(unit);
            }
        }
    }

    private void AttackAnimation(int idx)
    {
        _animator.Play($"Attack{idx}");
        _animator.Update(0);
    }
}
