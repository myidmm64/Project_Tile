using DG.Tweening;
using TMPro;
using UnityEngine;

public abstract class CommonEnemy : Enemy
{
    [SerializeField]
    private TextMeshPro _hpText = null;
    private Sequence _hpAniSeq = null;

    public override void Damage(int damage)
    {
        int startHP = CurHP;
        int destHP = startHP - damage;
        destHP = Mathf.Clamp(destHP, 0, MaxHP);

        _hpAniSeq.Kill();
        _hpAniSeq = DOTween.Sequence();
        _hpAniSeq.Append(DOTween.To(() => startHP, x => { _hpText.SetText(x.ToString()); }, destHP, _hpAniDuration))
            .SetEase(Ease.Linear);

        PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
        popup.Popup(damage.ToString(), transform.position + Vector3.up * 0.3f);
        CurHP = destHP;
    }

    protected override void Start()
    {
        base.Start();
        _hpText.SetText(CurHP.ToString());

        ChangeDice(new Vector2Int(2, 2));
        transform.position = dice.transform.position;
    }
}
