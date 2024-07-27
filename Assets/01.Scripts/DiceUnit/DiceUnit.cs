using System;
using UnityEngine;

public class DiceUnit : MonoBehaviour
{
    [SerializeField]
    protected DiceGrid _diceGrid = null; // 어디 DiceGrid에 있을 것인지

    public Dice dice = null;
    public Vector2Int positionKey => dice != null ? dice.positionKey : Vector2Int.zero;

    public Action OnDiceBinded = null;
    public Action OnDiceUnbinded = null;

    public void SetDiceGrid(DiceGrid diceGrid, Dice _dice = null)
    {
        _diceGrid = diceGrid;
        if(_dice != null) dice = _dice;
    }


}
