using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TileExtensionMethod
{
    /*
    /// <summary>
    /// IEnumerable 내의 ITileUnit들을 반환합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tiles"></param>
    /// <returns></returns>
    public static IEnumerable<ITileEntity> GetITileUnits(this IEnumerable<Tile> tiles)
    {
        List<ITileEntity> result = new List<ITileEntity>();
        foreach (var tile in tiles)
        {
            if(tile.tileUnit != null)
            {
                result.Add(tile.tileUnit);
            }    
        }
        return result;
    }

    /// <summary>
    /// IEnumerable 내의 ITileUnit들을 반환합니다.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tiles"></param>
    /// <returns></returns>
    public static IEnumerable<T> GetITileUnits<T>(this IEnumerable<Tile> tiles) where T : ITileEntity
    {
        List<T> result = new List<T>();
        foreach (var tile in tiles)
        {
            if (tile.tileUnit != null)
            {
                if(tile.tileUnit is T)
                {
                    result.Add((T)tile.tileUnit);
                }
            }
        }
        return result;
    }
    */

    /// <summary>
    /// IEnumerable 내의 중복되는 Tile들을 하나만 남기고 없애줍니다.
    /// </summary>
    /// <param name="tiles"></param>
    /// <returns></returns>
    public static IEnumerable<Tile> ExcludeReduplication(this IEnumerable<Tile> tiles)
    {
        return tiles.Distinct();
    }

    /// <summary>
    /// 두 Ienumerable의 차집합을 반환합니다.
    /// </summary>
    /// <param name="tiles"></param>
    /// <param name="exceptTiles"></param>
    /// <returns></returns>
    public static IEnumerable<Tile> ExceptTiles(this IEnumerable<Tile> tiles, IEnumerable<Tile> exceptTiles)
    {
        return tiles.Except(exceptTiles);
    }

    public static IEnumerable<Tile> AddTiles(this IEnumerable<Tile> tiles, params Vector2Int[] addPositions)
    {
        List<Tile> result = new List<Tile>();
        result.AddRange(tiles);
        foreach (var addPosition in addPositions)
        {
            if(TileManager.Inst.TryGetTile(addPosition, out Tile tile))
            {
                result.Add(tile);
            }
        }
        return result;
    }

    public static IEnumerable<Tile> SubTiles(this IEnumerable<Tile> tiles, params Vector2Int[] subPositions)
    {
        List<Tile> result = new List<Tile>();
        foreach (var subPosition in subPositions)
        {
            if (TileManager.Inst.TryGetTile(subPosition, out Tile tile))
            {
                result.Add(tile);
            }
        }
        return (tiles.ExceptTiles(result)).ExcludeReduplication();
    }
}
