using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceGrid : MonoBehaviour
{
    public Dictionary<Vector2Int, Dice> grid { get; private set; }
    public Dictionary<Vector2Int, DiceUnit> diceUnitGrid { get; private set; }

    [SerializeField]
    private DiceGenerateDataSO _diceGenerateData = null;
    public Vector2Int mapSize { get; private set; } // �ʿ� dice�� ������ �� ����Ǿ���� ���� �� ����.
    public Vector2Int mapCenter => new Vector2Int(mapSize.x / 2 + 1, mapSize.y / 2 + 1); // �ʿ� dice�� ������ �� ����Ǿ���� ���� �� ����.

    private void Awake()
    {
        grid = new Dictionary<Vector2Int, Dice>();
        diceUnitGrid = new Dictionary<Vector2Int, DiceUnit>();

        GenerateDices(out int maxRow, out int maxColumn);
        mapSize = new Vector2Int(maxRow, maxColumn);
    }

    [ContextMenu("�׸��� ������")]
    public void RestartGrid()
    {
        foreach (var dice in grid)
        {
            PoolManager.Inst.Push(dice.Value);
        }
        grid.Clear();
        GenerateDices(out int maxRow, out int maxColumn);
        mapSize = new Vector2Int(maxRow, maxColumn);
    }

    public void GenerateDices(out int maxRow, out int maxColumn)
    {
        string[] rows = _diceGenerateData.diceMapStr.Split('\n');
        maxColumn = rows.Length;
        maxRow = rows[0].Length;
        Vector2 startPos = _diceGenerateData.diceCenterPosition + GetPaddingPos(new Vector2(-(maxRow / 2), -(maxColumn / 2)), _diceGenerateData.dicePositionDistance);

        for (int y = 0; y < maxColumn; y++)
        {
            for (int x = 0; x < maxRow; x++)
            {
                int number = rows[y][x] - '0';
                if (number == 0) continue;
                Dice dice = PoolManager.Inst.Pop(EPoolType.Dice) as Dice; // PopDice((EDiceType)number);
                if (dice == null) continue;

                Vector2 dicePosition = startPos + GetPaddingPos(new Vector2(x, y), _diceGenerateData.dicePositionDistance);
                Vector2Int positionKey = new Vector2Int(x, maxColumn - y - 1);
                dice.positionKey = positionKey;
                dice.transform.position = dicePosition;
                dice.ChangeDiceType(EDiceType.Normal); // Custom
                dice.Roll();
                dice.SetSpriteOrder();
                dice.gameObject.name = positionKey.ToString();
                dice.transform.SetParent(transform, false);

                grid.Add(positionKey, dice);
            }
        }
    }

    public Vector2Int FindClosestUnitTile(Vector2Int start)
    {
        return FindClosestUnitTile<DiceUnit>(start);
    }

    // ���� ����� DiceUnit�� PositionKey ����
    public Vector2Int FindClosestUnitTile<T>(Vector2Int start) where T : DiceUnit
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),  // ��
            new Vector2Int(0, -1), // �Ʒ�
            new Vector2Int(1, 0),  // ������
            new Vector2Int(-1, 0)  // ����
        };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            // ���� �ִ� Ÿ���� ã���� ��ȯ
            if (current != start && diceUnitGrid.ContainsKey(current))
            {
                if (diceUnitGrid[current] is T)
                {
                    return current;
                }
            }

            foreach (Vector2Int dir in directions)
            {
                Vector2Int next = current + dir;

                if (!visited.Contains(next))
                {
                    queue.Enqueue(next);
                    visited.Add(next);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    private Vector2 GetPaddingPos(Vector2 pos, Vector2 padding)
    {
        return pos * padding;
    }

    public List<Dice> GetDices(List<Vector2Int> positionKeies)
    {
        List<Dice> result = new List<Dice>();
        foreach (var positionKey in positionKeies)
        {
            if (grid.ContainsKey(positionKey)) result.Add(grid[positionKey]);
        }
        return result;
    }

    public List<DiceUnit> GetDiceUnits(List<Vector2Int> positionKeies)
    {
        return GetDiceUnits<DiceUnit>(positionKeies);
    }

    public List<DiceUnit> GetDiceUnits<T>(List<Vector2Int> positionKeies) where T : DiceUnit
    {
        List<DiceUnit> result = new List<DiceUnit>();
        foreach (var positionKey in positionKeies)
        {
            if (diceUnitGrid.ContainsKey(positionKey))
            {
                if (diceUnitGrid[positionKey] is T)
                {
                    result.Add(diceUnitGrid[positionKey]);
                }
            }
        }
        return result;
    }

    public IEnumerable<Dice> GetSamePipDices(int dicePip)
    {
        var query = from dice in grid.Values
                    where dice.dicePip == dicePip
                    select dice;
        return query;
    }

    public IEnumerable<Dice> GetDiceRow(int rowNum)
    {
        var query = from diceKeyValue in grid
                    where diceKeyValue.Key.y == rowNum
                    select diceKeyValue.Value;
        return query;
    }

    public IEnumerable<Dice> GetDiceColumn(int columnNum)
    {
        var query = from diceKeyValue in grid
                    where diceKeyValue.Key.x == columnNum
                    select diceKeyValue.Value;
        return query;
    }
}
