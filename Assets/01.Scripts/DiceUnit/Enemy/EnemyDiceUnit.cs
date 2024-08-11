using DG.Tweening;
using TMPro;
using UnityEngine;

public class EnemyDiceUnit : DiceUnit, IDamagable
{
    public TextMeshPro hpText = null;
    public float hpTextAnimatingDuration = 0.2f;
    private Sequence hpTextAnimationSeq = null;

    private int _curHP = 0;
    [SerializeField]
    private int _maxHP = 0;

    public int CurHP { get => _curHP; set => _curHP = value; }
    public int MaxHP { get => _maxHP; set => _maxHP = value; }

    private void Start()
    {
        InitailizeHP();
        TestInit();
    }

    protected virtual void InitailizeHP()
    {
        MaxHP = _maxHP;
        CurHP = MaxHP;
        if (hpText != null)
        {
            hpText.SetText(_curHP.ToString());
        }

        MainUI.Inst.GetUIElement<EnemyUI>().nameText.SetText("보스 - 허수아비");
        MainUI.Inst.GetUIElement<EnemyUI>().hpSlider.Initialize(MaxHP);
    }

    private void TestInit()
    {
        ChangeMyDice(new Vector2Int(2, 2));
        transform.position = dice.transform.position;
        SetSpriteSortingOrder();
    }

    void IDamagable.Damage(int damage)
    {
        int startHP = CurHP;
        int destHP = startHP - damage;
        destHP = Mathf.Clamp(destHP, 0, MaxHP);
        if (hpText != null)
        {
            hpTextAnimationSeq.Kill();
            hpTextAnimationSeq = DOTween.Sequence();
            hpTextAnimationSeq.Append(DOTween.To(() => startHP, x => { hpText.SetText(x.ToString()); }, destHP, hpTextAnimatingDuration))
                .SetEase(Ease.Linear);
        }
        PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
        popup.Popup(damage.ToString(), transform.position + Vector3.up * 0.3f);
        CurHP = destHP;

        MainUI.Inst.GetUIElement<EnemyUI>().hpSlider.SetValueWithAnimation(CurHP, hpTextAnimatingDuration);
    }
}