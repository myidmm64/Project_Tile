using UnityEngine;

public abstract class BossEnemyDiceUnit : EnemyDiceUnit
{
    [SerializeField]
    private float _hpTextAnimatingDuration = 0.2f;
    [SerializeField]
    private string _bossPreName = string.Empty; // 추후 SO로 분리 필요
    [SerializeField]
    private string _prefix = " - ";  // 추후 SO로 분리 필요
    [SerializeField]
    private string _bossName = string.Empty; // 추후 SO로 분리 필요

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
        MaxHP = data.maxHP;
        CurHP = MaxHP;
    }

    protected override void Start()
    {
        var grid = GameObject.FindAnyObjectByType<DiceGrid>();
        SetDiceGrid(grid);
        base.Start();
        MainUI.Inst.GetUIElement<EnemyUI>().nameText.SetText(_bossPreName == string.Empty ? $"{_bossName}" : $"{_bossPreName}{_prefix}{_bossName}");
        MainUI.Inst.GetUIElement<EnemyUI>().hpSlider.Initialize(MaxHP);

        ChangeDice(new Vector2Int(2, 2));
        transform.position = dice.transform.position;
    }
}
