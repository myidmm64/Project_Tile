using DG.Tweening;
using TMPro;
using UnityEngine;

public class TestEnemy : DiceUnit
{
    public TextMeshPro hpText = null;
    public float hpTextAnimatingDuration = 0.2f;
    private Sequence hpTextAnimationSeq = null;

    protected override void Start()
    {
        base.Start();
        TestInit();
        if (hpText != null)
        {
            hpText.SetText(_curHP.ToString());
        }
    }

    private void TestInit()
    {
        ChangeMyDice(new Vector2Int(2, 2));
        transform.position = dice.transform.position;
        SetSpriteSortingOrder();
    }

    public override void Damage(int damage)
    {
        int startHP = _curHP;
        int destHP = startHP - damage;
        destHP = Mathf.Clamp(destHP, 0, _maxHP);
        if (hpText != null)
        {
            hpTextAnimationSeq.Kill();
            hpTextAnimationSeq = DOTween.Sequence();
            hpTextAnimationSeq.Append(DOTween.To(() => startHP, x => { hpText.SetText(x.ToString()); }, destHP, hpTextAnimatingDuration))
                .SetEase(Ease.Linear);
        }
        PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
        popup.Popup(damage.ToString(), transform.position + Vector3.up * 0.3f);
        _curHP = destHP;
    }
}