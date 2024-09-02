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
    public Vector2Int mapSize { get; private set; } // 맵에 dice가 삭제될 때 변경되어야할 가능 성 있음.
    public Vector2Int mapCenter => new Vector2Int(mapSize.x / 2 + 1, mapSize.y / 2 + 1); // 맵에 dice가 삭제될 때 변경되어야할 가능 성 있음.

    private void Awake()
    {
        grid = new Dictionary<Vector2Int, Dice>();
        diceUnitGrid = new Dictionary<Vector2Int, DiceUnit>();

        GenerateDices(out int maxRow, out int maxColumn);
        mapSize = new Vector2Int(maxRow, maxColumn);
    }

    [ContextMenu("그리드 재제작")]
    public void RestartGrid()
    {
        foreach(var dice in grid)
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

    // 가장 가까운 DiceUnit의 PositionKey 추출
    public Vector2Int FindClosestUnitTile(Vector2Int start)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),  // 위
            new Vector2Int(0, -1), // 아래
            new Vector2Int(1, 0),  // 오른쪽
            new Vector2Int(-1, 0)  // 왼쪽
        };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            // 적이 있는 타일을 찾으면 반환
            if (current != start && diceUnitGrid.ContainsKey(current))
            {
                return current;
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

    public bool TryGetDice(Vector2Int position, out Dice dice)
    {
        return grid.TryGetValue(position, out dice);
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

    public IEnumerable<Dice> GetDiceLine(Vector2Int startPos, EDirection direction, int count, bool plusReflect = false, EDirection rotateDirection = EDirection.Up)
    {
        if (count == -1)
        {
            count = Utility.GetMaxCountWithDirection(direction, mapSize);
        }
        List<Dice> result = new List<Dice>();
        Vector2Int dir = Utility.GetDirection(direction);
        Vector2Int reflectDir = Utility.GetDirection(Utility.GetReflectDirection(direction));
        for (int i = 0; i <= count; i++)
        {
            Vector2Int positionKey = startPos + dir * i;
            if (TryGetDice(GetRotatedPositionKey(positionKey, startPos, rotateDirection), out Dice dice))
            {
                result.Add(dice);
            }
            if (plusReflect)
            {
                Vector2Int reflectedPositionKey = startPos + reflectDir * i;
                if (TryGetDice(GetRotatedPositionKey(reflectedPositionKey, startPos, rotateDirection), out Dice reflectDice))
                {
                    result.Add(reflectDice);
                }
            }
        }
        return result.ExcludeReduplication();
    }

    public IEnumerable<Dice> GetCrossDices(Vector2Int startPos, int count)
    {
        List<Dice> result = new List<Dice>();
        result.AddRange(GetDiceLine(startPos, EDirection.Up, count, true));
        result.AddRange(GetDiceLine(startPos, EDirection.Right, count, true));
        return result.ExcludeReduplication();
    }

    public IEnumerable<Dice> GetXCrossDices(Vector2Int startPos, int count)
    {
        List<Dice> result = new List<Dice>();
        result.AddRange(GetDiceLine(startPos, EDirection.LeftUp, count, true));
        result.AddRange(GetDiceLine(startPos, EDirection.RightUp, count, true));
        return result.ExcludeReduplication();
    }

    public IEnumerable<Dice> GetDiceSquare(Vector2Int centerPos, int size, bool isBorder = false)
    {
        if (size % 2 == 0)
        {
            Debug.LogWarning("size가 짝수입니다. 홀수로 변환합니다.");
            size += 1;
        }
        return GetDiceRectangle(centerPos, size, size, isBorder);
    }

    public IEnumerable<Dice> GetDiceRotatedSquare(Vector2Int centerPos, int centerDistance, bool isBorder = false)
    {
        List<Dice> result = new List<Dice>();
        int curDistance = centerDistance;
        if (!isBorder)
        {
            if (TryGetDice(centerPos, out Dice dice))
            {
                result.Add(dice);
            }
        }
        while (curDistance > 0)
        {
            Vector2Int leftPos = new Vector2Int(centerPos.x - curDistance, centerPos.y);
            Vector2Int rightPos = new Vector2Int(centerPos.x + curDistance, centerPos.y);
            result.AddRange(GetDiceLine(leftPos, EDirection.RightUp, curDistance));
            result.AddRange(GetDiceLine(leftPos, EDirection.RightDown, curDistance));
            result.AddRange(GetDiceLine(rightPos, EDirection.LeftUp, curDistance));
            result.AddRange(GetDiceLine(rightPos, EDirection.LeftDown, curDistance));
            if (isBorder) break;
            curDistance--;
        }
        return result.ExcludeReduplication();
    }

    public IEnumerable<Dice> GetDiceRectangle(Vector2Int centerPos, int width, int height, bool isBorder = false, EDirection rotateDirection = EDirection.Up)
    {
        if (width == -1)
        {
            width = Utility.GetMaxCountWithDirection(EDirection.Right, mapSize);
        }
        else if (width % 2 == 0)
        {
            Debug.LogWarning("width가 짝수입니다. 홀수로 변환합니다.");
            width += 1;
        }
        if (height == -1)
        {
            height = Utility.GetMaxCountWithDirection(EDirection.Up, mapSize);
        }
        else if (height % 2 == 0)
        {
            Debug.LogWarning("height가 짝수입니다. 홀수로 변환합니다.");
            height += 1;
        }
        List<Dice> result = new List<Dice>();
        Vector2Int searchStartPos = new Vector2Int(-(width / 2), -(height / 2));
        Vector2Int searchEndPos = new Vector2Int(width / 2, height / 2);
        Vector2Int position = Vector2Int.zero;
        for (int x = searchStartPos.x; x <= searchEndPos.x; x++)
        {
            for (int y = searchStartPos.y; y <= searchEndPos.y; y++)
            {
                position.x = x;
                position.y = y;
                if (TryGetDice(GetRotatedPositionKey(centerPos + position, centerPos, rotateDirection), out Dice dice))
                {
                    result.Add(dice);
                }
            }
        }

        if (isBorder)
        {
            return result.ExceptDices(GetDiceRectangle(centerPos, width - 2, height - 2, false, rotateDirection));
        }
        return result;
    }

    public IEnumerable<Dice> GetDicesWithPattern(Vector2Int centerPos, string pattern, EDirection rotateDirection = EDirection.Up)
    {
        List<Dice> result = new List<Dice>();
        List<Vector2Int> positionKeys = GetStringToPositionKey(centerPos, pattern);
        foreach (var positionKey in positionKeys)
        {
            if (TryGetDice(GetRotatedPositionKey(positionKey, centerPos, rotateDirection), out Dice dice))
            {
                result.Add(dice);
            }
        }
        return result;
    }

    private Vector2Int GetRotatedPositionKey(Vector2Int targetKey, Vector2Int startkey, EDirection rotateDirection)
    {
        if (rotateDirection == EDirection.Up) return targetKey;
        Vector2 result = Quaternion.AngleAxis(Utility.GetZRotate(rotateDirection), Vector3.forward) * ((Vector2)(targetKey - startkey));
        return startkey + Vector2Int.RoundToInt(result);
    }

    /// <summary>
    /// string을 positionKey 집합으로 변환합니다
    /// </summary>
    /// <param name="targetString"></param>
    /// <returns></returns>
    public List<Vector2Int> GetStringToPositionKey(Vector2Int centerPos, string targetString)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        string[] rows = targetString.Split('\n');
        int maxColumn = rows.Length;
        int maxRow = rows[0].Length;
        Vector2Int startPos = centerPos + new Vector2Int(-(maxRow / 2), -(maxColumn / 2));

        for (int y = 1; y <= maxColumn; y++)
        {
            for (int x = 1; x <= maxRow; x++)
            {
                int number = rows[y - 1][x - 1] - '0';
                if (number == 0) continue;

                Vector2Int positionKey = startPos + new Vector2Int(x - 1, maxColumn - y);
                result.Add(positionKey);
            }
        }

        return result;
    }
}
