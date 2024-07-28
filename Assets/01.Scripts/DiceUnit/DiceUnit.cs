using System;
using UnityEngine;

public class DiceUnit : MonoBehaviour
{
    [SerializeField]
    protected DiceGrid _diceGrid = null; // 어디 DiceGrid에 있을 것인지

    public Dice dice = null;
    public Vector2Int positionKey => dice != null ? dice.positionKey : Vector2Int.zero;

    public Action<Dice> OnDiceBinded = null;
    public Action<Dice> OnDiceUnbinded = null;

    public void SetDiceGrid(DiceGrid diceGrid, Dice _dice = null)
    {
        _diceGrid = diceGrid;
        if(_dice != null) dice = _dice;
    }

    public bool ChangeMyDice(Vector2Int targetPositionKey)
    {
        if (_diceGrid == null) return false;

        if(_diceGrid.grid.TryGetValue(targetPositionKey, out Dice targetDice))
        {
            return ChangeMyDice(targetDice);
        }

        return false;
    }

    public bool ChangeMyDice(Dice targetDice)
    {
        if (_diceGrid == null) return false;
        bool changable = _diceGrid.diceUnitGrid.ContainsKey(targetDice.positionKey) == false 
            && targetDice.UnitBindable();
        if (changable == false) return false;

        bool alreadyBindedDice = dice != null && _diceGrid.diceUnitGrid.ContainsKey(positionKey);
        if (alreadyBindedDice)
        {
            _diceGrid.diceUnitGrid.Remove(positionKey);
        }

        OnDiceUnbinded?.Invoke(dice);
        dice = targetDice;
        _diceGrid.diceUnitGrid[positionKey] = this;
        OnDiceBinded?.Invoke(targetDice);
        return true;
    }
}
