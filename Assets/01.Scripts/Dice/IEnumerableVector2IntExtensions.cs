using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IEnumerableVector2IntExtensions
{
    /// <summary>
    /// IEnumerable 내의 중복되는 녀석들을 하나만 남기고 없애줍니다.
    /// </summary>
    /// <param name="positionKeys"></param>
    /// <returns></returns>
    public static IEnumerable<Vector2Int> ExcludeReduplication(this IEnumerable<Vector2Int> positionKeys)
    {
        positionKeys = positionKeys.Distinct();
        return positionKeys;
    }

    /// <summary>
    /// 두 Ienumerable의 차집합을 반환합니다.
    /// </summary>
    /// <param name="positionkeys"></param>
    /// <param name="exceptPositionkeys"></param>
    /// <returns></returns>
    public static IEnumerable<Vector2Int> ExceptKeys(this IEnumerable<Vector2Int> positionkeys, IEnumerable<Vector2Int> exceptPositionkeys)
    {
        positionkeys = positionkeys.Except(exceptPositionkeys);
        return positionkeys;
    }

    public static IEnumerable<Vector2Int> AddKeys(this IEnumerable<Vector2Int> positionKeys, params Vector2Int[] addPositions)
    {
        List<Vector2Int> result = positionKeys.ToList();
        result.AddRange(addPositions);

        positionKeys = result;
        return result;
    }

    public static IEnumerable<Vector2Int> SubKeys(this IEnumerable<Vector2Int> positionKeys, params Vector2Int[] subPositions)
    {
        List<Vector2Int> result = positionKeys.ToList();
        positionKeys = (result.ExceptKeys(subPositions)).ExcludeReduplication();
        return positionKeys;
    }
}
