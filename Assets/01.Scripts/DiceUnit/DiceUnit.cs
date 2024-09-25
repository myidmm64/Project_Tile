using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class DiceUnit : MonoBehaviour, IDamagable, IMovable
{
    public DiceGrid diceGrid { get; private set; }

    [SerializeField]
    private TextMeshPro _dicePipText = null; // 현재 DiceUnit의 눈금 텍스트
    [SerializeField]
    public DiceUnitData data { get; private set; }

    public Dice dice { get; private set; }
    public Vector2Int positionKey => dice != null ? dice.positionKey : new Vector2Int(-1, -1);
    public Action<Dice> OnDiceChanged = null;

    public int CurHP { get; set; }
    public int MaxHP { get; set; }

    public void SetDiceGrid(DiceGrid diceGrid, Dice _dice = null)
    {
        this.diceGrid = diceGrid;
        if (_dice != null) dice = _dice;
    }

    public bool ChangeDice(Vector2Int targetPositionKey)
    {
        if (diceGrid == null) return false;

        if (diceGrid.grid.TryGetValue(targetPositionKey, out Dice targetDice))
        {
            return ChangeDice(targetDice);
        }

        return false;
    }

    public bool ChangeDice(Dice targetDice) // targetDice로 변경하기
    {
        // grid 내 dice가 있고, unit이 들어갈 수 있어야 함.
        bool changable = diceGrid.diceUnitGrid.ContainsKey(targetDice.positionKey) == false
            && targetDice.UnitBindable();
        if (changable == false) return false;

        // 본인 위치의 DiceUnit 지우고 이동
        if (diceGrid.diceUnitGrid.ContainsKey(positionKey))
        {
            diceGrid.diceUnitGrid.Remove(positionKey);
        }
        dice = targetDice;

        _dicePipText?.SetText(dice.dicePip.ToString());
        diceGrid.diceUnitGrid[positionKey] = this;
        OnDiceChanged?.Invoke(dice);
        return true;
    }

    public void SetSpriteSortingOrder(SpriteRenderer renderer)
    {
        renderer.sortingOrder = 0 - positionKey.y;
    }

    public abstract void Damage(int damage);
    public virtual bool Move(Vector2Int target)
    {
        return ChangeDice(target);
    }
    public virtual bool Knockback(EDirection dir, int amount)
    {
        Vector2Int target = positionKey + (Utility.EDirectionToVector(dir) * amount);
        return ChangeDice(target);
    }   
}
