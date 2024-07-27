using System.Collections.Generic;
using UnityEngine;

public class DiceGrid : MonoBehaviour
{
    private Dictionary<Vector2Int, Dice> _grid = new Dictionary<Vector2Int, Dice>();

    [SerializeField]
    private int _column = 5;
    [SerializeField]
    private int _row = 5;
    [SerializeField]
    private Vector2 _padding = Vector2.one;
    [SerializeField]
    private Vector2 _startPosition = Vector2.zero;

    private void Awake()
    {
        CreateGrid(_column, _row, _padding, _startPosition);
    }

    public Dice GetDice(Vector2Int positionKey)
    {
        if(_grid.ContainsKey(positionKey) == false)
        {
            Debug.LogError($"PositionKey가 존재하지 않습니다. {positionKey}");
            return null;
        }
        return _grid[positionKey];
    }

    private void CreateGrid(int column, int row, Vector2 padding, Vector2 startPosition)
    {
        for (int i = 0; i < column; i++)
        {
            for (int j = 0; j < row; j++)
            {
                Vector2Int positionKey = new Vector2Int(i, j);
                Vector2 dicePosition = startPosition + (positionKey * padding);

                Dice dice = PoolManager.Inst.Pop(EPoolType.Dice) as Dice;
                dice.positionKey = positionKey;
                dice.transform.position = dicePosition;
                dice.ChangeDiceType(EDiceType.Normal); // Custom
                dice.Roll();
                dice.gameObject.name = positionKey.ToString();

                _grid.Add(positionKey, dice);
            }
        }
    }
}
