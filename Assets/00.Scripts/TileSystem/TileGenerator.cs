using System;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator
{
    private SGenerateTileData _data;

    public TileGenerator(SGenerateTileData data)
    {
        _data = data;
    }

    public void GenerateTile(Dictionary<Vector2Int, Tile> tileMap, string pattern, Vector2Int centerPositionKey, bool isOverride = false)
    {
        // parse string
        // 0 : empty, 1 : tile ..

        string[] lines = pattern.Split('\n');
        int rowLimit = lines.Length / 2;
        int columnLimit = lines[0].Length / 2;

        for( int rn = 0; rn < lines.Length; rn++)
        {
            for (int cn = 0; cn < lines[rn].Length; cn++)
            {
                Vector2Int positionKey = centerPositionKey + new Vector2Int(cn - columnLimit, rowLimit - rn);
                ETileTypeChar tileType = (ETileTypeChar)((int)lines[rn][cn] - 48);
                Tile tile = null;
                switch (tileType)
                {
                    case ETileTypeChar.Empty:
                        break;
                    case ETileTypeChar.Tile:
                        tile = PoolManager.Inst.PopWithComponent<Tile>(EPoolType.Tile);
                        break;
                    default:
                        break;
                }

                if (tileMap.ContainsKey(positionKey) && tileMap[positionKey] != null && isOverride == false)
                {
                    Debug.LogWarning($"{positionKey} 위치에 이미 Tile이 존재합니다.");
                    continue;
                }
                if (tile != null)
                {
                    tile.name = positionKey.ToString();
                    tile.positionKey = positionKey;
                    tile.transform.position = _data.centerPos + (positionKey * _data.tilePadding);
                    tile.transform.SetParent(_data.generateParent);
                }
                tileMap[positionKey] = tile;
            }
        }
    }
}