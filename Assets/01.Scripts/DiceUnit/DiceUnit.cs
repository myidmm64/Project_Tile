using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class DiceUnit : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _dicePipText = null;

    public DiceGrid diceGrid = null; // 어디 DiceGrid에 있을 것인지

    public SpriteRenderer spriteRenderer { get; private set; }
    public Animator animator { get; private set; }

    public bool autoFlip = true;

    public Dice dice = null;
    public Vector2Int positionKey => dice != null ? dice.positionKey : Vector2Int.zero;

    public Action<Dice> OnDiceBinded = null;
    public Action<Dice> OnDiceUnbinded = null;

    protected virtual void Awake()
    {
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        animator = spriteRenderer.GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        if (autoFlip) LookClosestUnit();
    }

    public void SetDiceGrid(DiceGrid diceGrid, Dice _dice = null)
    {
        this.diceGrid = diceGrid;
        if(_dice != null) dice = _dice;
    }

    public bool ChangeMyDice(Vector2Int targetPositionKey, bool setSortingOrder = true)
    {
        if (diceGrid == null) return false;

        if(diceGrid.grid.TryGetValue(targetPositionKey, out Dice targetDice))
        {
            return ChangeMyDice(targetDice, setSortingOrder);
        }

        return false;
    }

    public bool ChangeMyDice(Dice targetDice, bool setSortingOrder = true)
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
        _dicePipText?.SetText(dice.dicePip.ToString());
        diceGrid.diceUnitGrid[positionKey] = this;
        OnDiceBinded?.Invoke(targetDice);
        if (setSortingOrder) SetSpriteSortingOrder();
        return true;
    }

    public void LookClosestUnit()
    {
        Vector2Int closest = diceGrid.FindClosestUnitTile(positionKey);
        if (closest.x < positionKey.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (closest.x > positionKey.x)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void SetSpriteSortingOrder()
    {
        spriteRenderer.sortingOrder = 0 - positionKey.y;
    }
}
