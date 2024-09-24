using DG.Tweening;
using TMPro;
using UnityEngine;

// 보스가 아닌 기본 유닛에 대한 것입니다.
public abstract class CommonEnemyDiceUnit : EnemyDiceUnit
{
    [SerializeField]
    private TextMeshPro _hpText = null;
    [SerializeField]
    private float _hpTextAnimatingDuration = 0.2f;

    private Sequence _hpTextAnimationSeq = null;

    [SerializeField]
    private int _maxHP = 0; // 추후 SO로 분리 필요
    private int _curHP = 0;

    public override int CurHP { get => _curHP; set => _curHP = value; }
    public override int MaxHP { get => _maxHP; set => _maxHP = value; }

    public override void Damage(int damage)
    {
        int startHP = CurHP;
        int destHP = startHP - damage;
        destHP = Mathf.Clamp(destHP, 0, MaxHP);

        _hpTextAnimationSeq.Kill();
        _hpTextAnimationSeq = DOTween.Sequence();
        _hpTextAnimationSeq.Append(DOTween.To(() => startHP, x => { _hpText.SetText(x.ToString()); }, destHP, _hpTextAnimatingDuration))
            .SetEase(Ease.Linear);

        PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
        popup.Popup(damage.ToString(), transform.position + Vector3.up * 0.3f);
        CurHP = destHP;
    }

    protected override void Initialize()
    {
        MaxHP = _maxHP;
        CurHP = MaxHP;
        _hpText.SetText(CurHP.ToString());
    }

    protected override void Start()
    {
        base.Start();
        ChangeDice(new Vector2Int(2, 2));
        transform.position = dice.transform.position;
    }
}
