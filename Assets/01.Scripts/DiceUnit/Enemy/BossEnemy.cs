using UnityEngine;

public abstract class BossEnemy : Enemy
{
    public override void Damage(int damage)
    {
        int startHP = CurHP;
        int destHP = startHP - damage;
        destHP = Mathf.Clamp(destHP, 0, MaxHP);

        PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
        popup.Popup(damage.ToString(), transform.position + Vector3.up * 0.3f);
        CurHP = destHP;

        MainUI.Inst.GetUIElement<EnemyUI>().hpSlider.SetValueWithAnimation(CurHP, _hpAniDuration);
    }

    protected override void Start()
    {
        // 추후 그리드 선택 부분과 Change Dice 부분 다른 곳에서 하기
        var grid = GameObject.FindAnyObjectByType<DiceGrid>();
        SetDiceGrid(grid);
        base.Start();
        MainUI.Inst.GetUIElement<EnemyUI>().nameText.SetText(data.unitName);
        MainUI.Inst.GetUIElement<EnemyUI>().hpSlider.Initialize(MaxHP);

        ChangeDice(new Vector2Int(2, 2));
        transform.position = dice.transform.position;
    }
}