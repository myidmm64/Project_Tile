using UnityEngine;

public abstract class BossEnemyDiceUnit : EnemyDiceUnit
{
    [SerializeField]
    private float _hpTextAnimatingDuration = 0.2f;
    [SerializeField]
    private string _bossPreName = string.Empty; // ���� SO�� �и� �ʿ�
    [SerializeField]
    private string _prefix = " - ";  // ���� SO�� �и� �ʿ�
    [SerializeField]
    private string _bossName = string.Empty; // ���� SO�� �и� �ʿ�
    [SerializeField]
    private int _maxHP = 0; // ���� SO�� �и� �ʿ�
    private int _curHP = 0;

    public override int CurHP { get => _curHP; set => _curHP = value; }
    public override int MaxHP { get => _maxHP; set => _maxHP = value; }

    public override void Damage(int damage)
    {
        int startHP = CurHP;
        int destHP = startHP - damage;
        destHP = Mathf.Clamp(destHP, 0, MaxHP);

        PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
        popup.Popup(damage.ToString(), transform.position + Vector3.up * 0.3f);
        CurHP = destHP;

        MainUI.Inst.GetUIElement<EnemyUI>().hpSlider.SetValueWithAnimation(CurHP, _hpTextAnimatingDuration);
    }

    protected override void Initialize()
    {
        MaxHP = _maxHP;
        CurHP = MaxHP;
    }

    protected override void Start()
    {
        base.Start();
        MainUI.Inst.GetUIElement<EnemyUI>().nameText.SetText(_bossPreName == string.Empty ? $"{_bossName}" : $"{_bossPreName}{_prefix}{_bossName}");
        MainUI.Inst.GetUIElement<EnemyUI>().hpSlider.Initialize(MaxHP);

        ChangeMyDice(new Vector2Int(2, 2));
        transform.position = dice.transform.position;
    }
}