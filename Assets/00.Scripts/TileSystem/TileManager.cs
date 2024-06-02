using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Playables;
using UnityEngine;

public class TileManager : MonoSingleTon<TileManager>
{
    private Dictionary<Vector2Int, Tile> _tiles = new();
    [SerializeField, TextArea]
    private string _tileMapStr = string.Empty;
    [SerializeField]
    private SGenerateTileData _generateTileData;
    private TileGenerator _tileGenerator = null;

    private void Awake()
    {
        GenerateTileMap();
    }

    private void GenerateTileMap()
    {
        _tileGenerator = new TileGenerator(_generateTileData);
        _tileGenerator.GenerateTile(_tiles, _tileMapStr, Vector2Int.zero);
    }

    public bool TryGetTile(Vector2Int positionKey, out Tile tile)
    {
        return _tiles.TryGetValue(positionKey, out tile);
    }

    public IEnumerable<Tile> GetTileRow(int rowNum)
    {
        var query = from tileKeyValue in _tiles
                    where tileKeyValue.Key.y == rowNum
                    select tileKeyValue.Value;
        return query;
    }

    public IEnumerable<Tile> GetTileColumn(int columnNum)
    {
        var query = from tileKeyValue in _tiles
                    where tileKeyValue.Key.x == columnNum
                    select tileKeyValue.Value;
        return query;
    }

    public IEnumerable<Tile> GetTileLine(Vector2Int startPos, EDirection direction, int count, bool plusReflect = false, EDirection rotateDirection = EDirection.Up)
    {
        if (count == -1)
        {
            // count = Utility.GetMaxCountWithDirection(direction, _mapSize);
        }
        List<Tile> result = new List<Tile>();
        Vector2Int dir = Utility.GetDirection(direction);
        Vector2Int reflectDir = Utility.GetDirection(Utility.GetReflectDirection(direction));
        for (int i = 0; i <= count; i++)
        {
            Vector2Int tileKey = startPos + dir * i;
            if (TryGetTile(GetRotatedTileKey(tileKey, startPos, rotateDirection), out Tile tile))
            {
                result.Add(tile);
            }
            if (plusReflect)
            {
                Vector2Int reflectTileKey = startPos + reflectDir * i;
                if (TryGetTile(GetRotatedTileKey(reflectTileKey, startPos, rotateDirection), out Tile reflectTile))
                {
                    result.Add(reflectTile);
                }
            }
        }
        return result.ExcludeReduplication();
    }

    public IEnumerable<Tile> GetCrossTiles(Vector2Int startPos, int count)
    {
        List<Tile> result = new List<Tile>();
        result.AddRange(GetTileLine(startPos, EDirection.Up, count, true));
        result.AddRange(GetTileLine(startPos, EDirection.Right, count, true));
        return result.ExcludeReduplication();
    }

    public IEnumerable<Tile> GetXCrossTiles(Vector2Int startPos, int count)
    {
        List<Tile> result = new List<Tile>();
        result.AddRange(GetTileLine(startPos, EDirection.LeftUp, count, true));
        result.AddRange(GetTileLine(startPos, EDirection.RightUp, count, true));
        return result.ExcludeReduplication();
    }

    public IEnumerable<Tile> GetTileSquare(Vector2Int centerPos, int size, bool isBorder = false)
    {
        if (size % 2 == 0)
        {
            Debug.LogWarning("size가 짝수입니다. 홀수로 변환합니다.");
            size += 1;
        }
        return GetTileRectangle(centerPos, size, size, isBorder);
    }

    public IEnumerable<Tile> GetTileRotatedSquare(Vector2Int centerPos, int centerDistance, bool isBorder = false)
    {
        List<Tile> result = new List<Tile>();
        int curDistance = centerDistance;
        if (!isBorder)
        {
            if (TryGetTile(centerPos, out Tile tile))
            {
                result.Add(tile);
            }
        }
        while (curDistance > 0)
        {
            Vector2Int leftPos = new Vector2Int(centerPos.x - curDistance, centerPos.y);
            Vector2Int rightPos = new Vector2Int(centerPos.x + curDistance, centerPos.y);
            result.AddRange(GetTileLine(leftPos, EDirection.RightUp, curDistance));
            result.AddRange(GetTileLine(leftPos, EDirection.RightDown, curDistance));
            result.AddRange(GetTileLine(rightPos, EDirection.LeftUp, curDistance));
            result.AddRange(GetTileLine(rightPos, EDirection.LeftDown, curDistance));
            if (isBorder) break;
            curDistance--;
        }
        return result.ExcludeReduplication();
    }

    public IEnumerable<Tile> GetTileRectangle(Vector2Int centerPos, int width, int height, bool isBorder = false, EDirection rotateDirection = EDirection.Up)
    {
        if (width == -1)
        {
            // width = Utility.GetMaxCountWithDirection(EDirection.Right, _mapSize);
        }
        else if (width % 2 == 0)
        {
            Debug.LogWarning("width가 짝수입니다. 홀수로 변환합니다.");
            width += 1;
        }
        if (height == -1)
        {
            // height = Utility.GetMaxCountWithDirection(EDirection.Up, _mapSize);
        }
        else if (height % 2 == 0)
        {
            Debug.LogWarning("height가 짝수입니다. 홀수로 변환합니다.");
            height += 1;
        }
        List<Tile> result = new List<Tile>();
        Vector2Int searchStartPos = new Vector2Int(-(width / 2), -(height / 2));
        Vector2Int searchEndPos = new Vector2Int(width / 2, height / 2);
        Vector2Int position = Vector2Int.zero;
        for (int x = searchStartPos.x; x <= searchEndPos.x; x++)
        {
            for (int y = searchStartPos.y; y <= searchEndPos.y; y++)
            {
                position.x = x;
                position.y = y;
                if (TryGetTile(GetRotatedTileKey(centerPos + position, centerPos, rotateDirection), out Tile tile))
                {
                    result.Add(tile);
                }
            }
        }

        if (isBorder)
        {
            return result.ExceptTiles(GetTileRectangle(centerPos, width - 2, height - 2, false, rotateDirection));
        }
        return result;
    }

    public IEnumerable<Tile> GetTilesWithPattern(Vector2Int centerPos, string pattern, EDirection rotateDirection = EDirection.Up)
    {
        List<Tile> result = new List<Tile>();
        List<Vector2Int> tileKeys = GetStringToTileKeys(centerPos, pattern);
        foreach (var tileKey in tileKeys)
        {
            if (TryGetTile(GetRotatedTileKey(tileKey, centerPos, rotateDirection), out Tile tile))
            {
                result.Add(tile);
            }
        }
        return result;
    }

    private Vector2Int GetRotatedTileKey(Vector2Int targetKey, Vector2Int startkey, EDirection rotateDirection)
    {
        if (rotateDirection == EDirection.Up) return targetKey;
        Vector2 result = Quaternion.AngleAxis(Utility.GetZRotate(rotateDirection), Vector3.forward) * ((Vector2)(targetKey - startkey));
        return startkey + Vector2Int.RoundToInt(result);
    }

    /// <summary>
    /// string을 tileKey 집합으로 변환합니다
    /// </summary>
    /// <param name="targetString"></param>
    /// <returns></returns>
    public List<Vector2Int> GetStringToTileKeys(Vector2Int centerPos, string targetString)
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

                Vector2Int tileKey = startPos + new Vector2Int(x - 1, maxColumn - y);
                result.Add(tileKey);
            }
        }

        return result;
    }
}
