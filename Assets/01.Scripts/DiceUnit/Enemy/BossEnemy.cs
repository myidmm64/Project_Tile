using UnityEngine;

public abstract class BossEnemy : Enemy
{
    /*
    public override void Damage(int damage, EAttackType attackType, EDamageType damageType)
    {
        base.Damage(damage, attackType, damageType);
        MainUI.Inst.GetUIElement<EnemyUI>().hpSlider.SetValueWithAnimation(CurHP, _hpAniDuration);
    }
    */

    protected override void Start()
    {
        // ���� �׸��� ���� �κа� Change Dice �κ� �ٸ� ������ �ϱ�
        var grid = GameObject.FindAnyObjectByType<DiceGrid>();
        base.Start();
        MainUI.Inst.GetUIElement<EnemyUI>().nameText.SetText(data.unitName);
        MainUI.Inst.GetUIElement<EnemyUI>().hpSlider.Initialize(MaxHP);

        ChangeDice(new Vector2Int(2, 2));
        transform.position = dice.transform.position;
    }
}