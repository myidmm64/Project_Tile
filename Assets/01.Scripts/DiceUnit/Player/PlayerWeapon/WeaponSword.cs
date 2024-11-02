using UnityEngine;

public class WeaponSword : PlayerWeapon
{
    private int _atkIdx = 0;
    private int _playerAimIdx = 0;

    [SerializeField]
    private WeaponSprite _weaponSprite = null;
    [SerializeField]
    private SwordEffect _swordEffect = null;

    [SerializeField]
    private int _maxAttackCount = 3;
    [SerializeField]
    private float[] _attackPercentDamages = new float[3] { 100f, 100f, 100f };

    protected override void Awake()
    {
        base.Awake();
        if(_weaponSprite == null) _weaponSprite = transform.Find("Sprite").GetComponent<WeaponSprite>();
    }

    private void Start()
    {
        _weaponSprite.spriteRenderer.enabled = false;
    }

    protected virtual void ActiveWeapon()
    {
        _weaponSprite.spriteRenderer.enabled = true;
        gameObject.SetActive(true);
        _weaponSprite.FadeIn();
    }

    protected void WeaponPositionSet(DiceUnit target)
    {
        _player.sprite.LookAt(target.positionKey, force: true); // 일단 봐
        transform.position = target.dice.groundPos +
            new Vector3(-0.5f * (_player.sprite.direction == EDirection.Left ? -1f : 1f), 0.6f);// _player.transform.position + (Vector3)(dir * 1f) + Vector3.up * 0.6f;
        transform.localScale = _player.sprite.transform.localScale; // flip
    }

    public override void Attack()
    {
        _weaponSprite.spriteRenderer.enabled = true;
        gameObject.SetActive(true);
        _weaponSprite.FadeIn();

        DiceUnit target = _attackTargets[0];
        Vector2 dir = target.positionKey - _player.positionKey;

        WeaponPositionSet(target);

        _swordEffect.transform.position = target.dice.groundPos;
        _swordEffect.PlayEffect(_atkIdx);
        _player.Attack(target, EAttackType.Physical, _attackPercentDamages[_atkIdx], out var cri);

        _player.playerSprite.AdvanceMove(dir.normalized * 0.35f, 0.1f);
        _player.playerSprite.AttackAnimation(_playerAimIdx);
        AttackAnimation(_atkIdx);

        _playerAimIdx = (_playerAimIdx + 1) % 2;
        _atkIdx = (_atkIdx + 1) % _maxAttackCount;

        _player.GetModule<PlayerSkillModule>().IncreaseDP(20);
    }

    protected override void UpdateAttackTargets()
    {
        _attackTargets = DiceGrid.Inst.GetIncludedDiceUnits(_data.attackRange, _player);
    }

    private void AttackAnimation(int idx)
    {
        _weaponSprite.animator.Play($"Attack{idx}", -1, 0f); // 똑같은 애니메이션이 또 실행됐을 때 문제가 있으므로 무조건 처음부터 시작하도록
    }

    private void SkillAnimation()
    {
        _weaponSprite.animator.Play($"Skill", -1, 0f);
    }

    public override void UseSkill(Skill skill)
    {
        if (skill.targets.Count == 0) return;

        DiceUnit target = skill.targets[0];
        Vector2 dir = target.positionKey - _player.positionKey;

        WeaponPositionSet(target);
        SkillAnimation();

        skill.transform.position = transform.position;
        Vector3 skillLoSc = skill.transform.localScale;
        skill.transform.localScale = new Vector3(skillLoSc.x * (transform.localScale.x < 0 ? -1f : 1f) , skillLoSc.y);
        _player.playerSprite.AdvanceMove(dir.normalized * 0.35f, 0.1f);
        _player.playerSprite.SkillAnimation(0);
    }
}
