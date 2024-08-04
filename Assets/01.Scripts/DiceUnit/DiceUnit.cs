using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public abstract class DiceUnit : MonoBehaviour
{
    public int maxHP = 100;
    private int curHP = 0;
    public TextMeshPro hpText = null;
    public float hpTextAnimatingDuration = 0.2f;
    private Sequence hpTextAnimationSeq = null;

    public DiceGrid diceGrid = null; // 어디 DiceGrid에 있을 것인지

    public SpriteRenderer spriteRenderer = null;

    public Dice dice = null;
    public Vector2Int positionKey => dice != null ? dice.positionKey : Vector2Int.zero;

    public Action<Dice> OnDiceBinded = null;
    public Action<Dice> OnDiceUnbinded = null;

    protected virtual void Start()
    {
        curHP = maxHP;
        if(hpText != null)
        {
            hpText.SetText(curHP.ToString());
        }
    }

    public void SetDiceGrid(DiceGrid diceGrid, Dice _dice = null)
    {
        this.diceGrid = diceGrid;
        if(_dice != null) dice = _dice;
    }

    public bool ChangeMyDice(Vector2Int targetPositionKey)
    {
        if (diceGrid == null) return false;

        if(diceGrid.grid.TryGetValue(targetPositionKey, out Dice targetDice))
        {
            return ChangeMyDice(targetDice);
        }

        return false;
    }

    public bool ChangeMyDice(Dice targetDice)
    {
        if (diceGrid == null) return false;
        bool changable = diceGrid.diceUnitGrid.ContainsKey(targetDice.positionKey) == false 
            && targetDice.UnitBindable();
        if (changable == false) return false;

        bool alreadyBindedDice = dice != null && diceGrid.diceUnitGrid.ContainsKey(positionKey);
        if (alreadyBindedDice)
        {
            diceGrid.diceUnitGrid.Remove(positionKey);
        }

        OnDiceUnbinded?.Invoke(dice);
        dice = targetDice;
        diceGrid.diceUnitGrid[positionKey] = this;
        OnDiceBinded?.Invoke(targetDice);
        return true;
    }

    public void SetSpriteSortingOrder()
    {
        spriteRenderer.sortingOrder = positionKey.y;
    }

    public virtual void Damage(int damage)
    {
        int startHP = curHP;
        int destHP = startHP - damage;
        destHP = Mathf.Clamp(destHP, 0, maxHP);
        if(hpText != null)
        {
            hpTextAnimationSeq.Kill();
            hpTextAnimationSeq = DOTween.Sequence();
            hpTextAnimationSeq.Append(DOTween.To(() => startHP, x => { hpText.SetText(x.ToString()); }, destHP, hpTextAnimatingDuration))
                .SetEase(Ease.Linear);
        }
        PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
        popup.Popup(damage.ToString(), transform.position + Vector3.up * 0.3f);
        curHP = destHP;
    }
}
