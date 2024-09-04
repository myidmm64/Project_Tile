using System.Collections.Generic;
using UnityEngine;

public static class PosKeyUtil
{
    public static IEnumerable<Vector2Int> Line(Vector2Int startPos, EDirection direction, int count, bool plusReflect = false, EDirection rotateDirection = EDirection.Up)
    {
        List<Vector2Int> result = new List<Vector2Int>();

        Vector2Int dir = Utility.EDirectionToVector(direction);
        Vector2Int reflectDir = Utility.EDirectionToVector(Utility.GetReflectedDirection(direction));
        for (int i = 0; i <= count; i++)
        {
            Vector2Int positionKey = startPos + dir * i;
            result.Add(RotatedPositionKey(positionKey, startPos, rotateDirection));
            if (plusReflect)
            {
                Vector2Int reflectedPositionKey = startPos + reflectDir * i;
                result.Add(RotatedPositionKey(reflectedPositionKey, startPos, rotateDirection));
            }
        }
        return result.ExcludeReduplication();
    }

    public static IEnumerable<Vector2Int> Cross(Vector2Int startPos, int count)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        result.AddRange(Line(startPos, EDirection.Up, count, true));
        result.AddRange(Line(startPos, EDirection.Right, count, true));
        return result.ExcludeReduplication();
    }

    public static IEnumerable<Vector2Int> XCross(Vector2Int startPos, int count)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        result.AddRange(Line(startPos, EDirection.LeftUp, count, true));
        result.AddRange(Line(startPos, EDirection.RightUp, count, true));
        return result.ExcludeReduplication();
    }

    public static IEnumerable<Vector2Int> Square(Vector2Int centerPos, int size, bool isBorder = false)
    {
        if (size % 2 == 0)
        {
            Debug.LogWarning("size가 짝수입니다. 홀수로 변환합니다.");
            size += 1;
        }
        return Rectangle(centerPos, size, size, isBorder);
    }

    public static IEnumerable<Vector2Int> RotatedSquare(Vector2Int centerPos, int centerDistance, bool isBorder = false)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        int curDistance = centerDistance;
        if (!isBorder)
        {
            result.Add(centerPos);
        }
        while (curDistance > 0)
        {
            Vector2Int leftPos = new Vector2Int(centerPos.x - curDistance, centerPos.y);
            Vector2Int rightPos = new Vector2Int(centerPos.x + curDistance, centerPos.y);
            result.AddRange(Line(leftPos, EDirection.RightUp, curDistance));
            result.AddRange(Line(leftPos, EDirection.RightDown, curDistance));
            result.AddRange(Line(rightPos, EDirection.LeftUp, curDistance));
            result.AddRange(Line(rightPos, EDirection.LeftDown, curDistance));
            if (isBorder) break;
            curDistance--;
        }
        return result.ExcludeReduplication();
    }

    public static IEnumerable<Vector2Int> Rectangle(Vector2Int centerPos, int width, int height, bool isBorder = false, EDirection rotateDirection = EDirection.Up)
    {
        if (width % 2 == 0)
        {
            Debug.LogWarning("width가 짝수입니다. 홀수로 변환합니다.");
            width += 1;
        }
        if (height % 2 == 0)
        {
            Debug.LogWarning("height가 짝수입니다. 홀수로 변환합니다.");
            height += 1;
        }
        List<Vector2Int> result = new List<Vector2Int>();
        Vector2Int searchStartPos = new Vector2Int(-(width / 2), -(height / 2));
        Vector2Int searchEndPos = new Vector2Int(width / 2, height / 2);
        Vector2Int position = Vector2Int.zero;
        for (int x = searchStartPos.x; x <= searchEndPos.x; x++)
        {
            for (int y = searchStartPos.y; y <= searchEndPos.y; y++)
            {
                position.x = x;
                position.y = y;
                result.Add(RotatedPositionKey(centerPos + position, centerPos, rotateDirection));
            }
        }

        if (isBorder)
        {
            return result.ExceptKeys(Rectangle(centerPos, width - 2, height - 2, false, rotateDirection));
        }
        return result;
    }

    public static IEnumerable<Vector2Int> StrPattern(Vector2Int centerPos, string pattern, EDirection rotateDirection = EDirection.Up)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        List<Vector2Int> positionKeys = StringToPositionKey(centerPos, pattern);
        foreach (var positionKey in positionKeys)
        {
            result.Add(RotatedPositionKey(positionKey, centerPos, rotateDirection));
        }
        return result;
    }

    private static Vector2Int RotatedPositionKey(Vector2Int targetKey, Vector2Int startkey, EDirection rotateDirection)
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
    private static List<Vector2Int> StringToPositionKey(Vector2Int centerPos, string targetString)
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
