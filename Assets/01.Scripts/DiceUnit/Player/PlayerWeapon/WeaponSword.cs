using UnityEngine;

public class WeaponSword : PlayerWeapon
{
    private int _atkIdx = 0;
    private int _playerAimIdx = 0;

    [SerializeField]
    private WeaponSprite _weaponSprite = null;

    protected override void Awake()
    {
        base.Awake();
        if(_weaponSprite == null) _weaponSprite = transform.Find("Sprite").GetComponent<WeaponSprite>();
    }

    private void Start()
    {
        _weaponSprite.spriteRenderer.enabled = false;
    }

    public override void Attack()
    {
        _weaponSprite.spriteRenderer.enabled = true;
        gameObject.SetActive(true);
        _weaponSprite.FadeIn();

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
        _weaponSprite.animator.Play($"Attack{idx}");
        _weaponSprite.animator.Update(0);
    }
}
