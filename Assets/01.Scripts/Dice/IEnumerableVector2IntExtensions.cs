using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class IEnumerableVector2IntExtensions
{
    /// <summary>
    /// IEnumerable ���� �ߺ��Ǵ� �༮���� �ϳ��� ����� �����ݴϴ�.
    /// </summary>
    /// <param name="positionKeys"></param>
    /// <returns></returns>
    public static IEnumerable<Vector2Int> ExcludeReduplication(this IEnumerable<Vector2Int> positionKeys)
    {
        return positionKeys.Distinct();
    }

    /// <summary>
    /// �� Ienumerable�� �������� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="positionkeys"></param>
    /// <param name="exceptPositionkeys"></param>
    /// <returns></returns>
    public static IEnumerable<Vector2Int> ExceptKeys(this IEnumerable<Vector2Int> positionkeys, IEnumerable<Vector2Int> exceptPositionkeys)
    {
        return positionkeys.Except(exceptPositionkeys);
    }

    public static IEnumerable<Vector2Int> AddKeys(this IEnumerable<Vector2Int> positionKeys, params Vector2Int[] addPositions)
    {
        List<Vector2Int> result = positionKeys.ToList();
        result.AddRange(addPositions);
        return result;
    }

    public static IEnumerable<Vector2Int> SubKeys(this IEnumerable<Vector2Int> positionKeys, params Vector2Int[] subPositions)
    {
        List<Vector2Int> result = positionKeys.ToList();
        return (result.ExceptKeys(subPositions)).ExcludeReduplication();
    }
}
