using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public abstract class DiceUnit : MonoBehaviour
{
    [SerializeField]
    protected int _maxHP = 100;
    protected int _curHP = 0;

    public DiceGrid diceGrid = null; // ��� DiceGrid�� ���� ������

    public SpriteRenderer spriteRenderer = null;

    public Dice dice = null;
    public Vector2Int positionKey => dice != null ? dice.positionKey : Vector2Int.zero;

    public Action<Dice> OnDiceBinded = null;
    public Action<Dice> OnDiceUnbinded = null;

    protected virtual void Start()
    {
        SetMaxHP();
    }

    protected virtual void SetMaxHP()
    {
        _curHP = _maxHP;
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
        spriteRenderer.sortingOrder = 0 - positionKey.y;
    }

    public virtual void Damage(int damage)
    {
    }
}
