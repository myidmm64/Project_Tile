using System.Collections.Generic;
using UnityEngine;

public class DiceGrid : MonoBehaviour
{
    public Dictionary<Vector2Int, Dice> grid { get; private set; }
    public Dictionary<Vector2Int, DiceUnit> diceUnitGrid { get; private set; }

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
        grid = new Dictionary<Vector2Int, Dice>();
        diceUnitGrid = new Dictionary<Vector2Int, DiceUnit>();

        CreateGrid(_column, _row, _padding, _startPosition);
    }

    [ContextMenu("그리드 재제작")]
    public void RestartGrid()
    {
        foreach(var dice in grid)
        {
            PoolManager.Inst.Push(dice.Value);
        }
        grid.Clear();
        CreateGrid(_column, _row, _padding, _startPosition);
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
                dice.SetSpriteOrder();
                dice.gameObject.name = positionKey.ToString();

                grid.Add(positionKey, dice);
            }
        }
    }
}
