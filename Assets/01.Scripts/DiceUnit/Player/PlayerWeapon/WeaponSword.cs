using UnityEngine;

public class WeaponSword : PlayerWeapon
{
    private int _atkIdx = 0;
    private int _playerAimIdx = 0;

    private SpriteRenderer _renderer = null;
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        _renderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        _animator = _renderer.GetComponent<Animator>();
    }

    private void Start()
    {
        _renderer.enabled = false;
    }

    public override void Attack()
    {
        _renderer.enabled = true;
        gameObject.SetActive(true);

        DiceUnit target = _attackTargets[0];
        Vector2 dir = target.positionKey - _player.positionKey;

        transform.position = _player.transform.position + (Vector3)(dir * 1f) + Vector3.up * 0.6f;
        transform.localScale = _player.sprite.transform.localScale; // flip

        _player.playerSprite.AdvanceMove(dir * 0.35f, 0.1f);
        target.Damage(1);

        _player.playerSprite.AttackAnimation(_playerAimIdx);
        AttackAnimation(_atkIdx);
        _playerAimIdx = (_playerAimIdx + 1) % 2;
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
