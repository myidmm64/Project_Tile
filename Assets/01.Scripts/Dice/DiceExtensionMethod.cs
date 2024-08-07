using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DiceExtensionMethod
{
    /// <summary>
    /// IEnumerable 내의 중복되는 Dice들을 하나만 남기고 없애줍니다.
    /// </summary>
    /// <param name="dices"></param>
    /// <returns></returns>
    public static IEnumerable<Dice> ExcludeReduplication(this IEnumerable<Dice> dices)
    {
        return dices.Distinct();
    }

    /// <summary>
    /// 두 Ienumerable의 차집합을 반환합니다.
    /// </summary>
    /// <param name="dices"></param>
    /// <param name="exceptDices"></param>
    /// <returns></returns>
    public static IEnumerable<Dice> ExceptDices(this IEnumerable<Dice> dices, IEnumerable<Dice> exceptDices)
    {
        return dices.Except(exceptDices);
    }

    public static IEnumerable<Dice> AddDices(this IEnumerable<Dice> dices, DiceGrid grid, params Vector2Int[] addPositions)
    {
        List<Dice> result = new List<Dice>();
        result.AddRange(dices);
        foreach (var addPosition in addPositions)
        {
            if (grid.grid.ContainsKey(addPosition))
            {
                result.Add(grid.grid[addPosition]);
            }
        }
        return result;
    }

    public static IEnumerable<Dice> SubDices(this IEnumerable<Dice> dices, DiceGrid grid, params Vector2Int[] subPositions)
    {
        List<Dice> result = new List<Dice>();
        foreach (var subPosition in subPositions)
        {
            if (grid.grid.ContainsKey(subPosition))
            {
                result.Add(grid.grid[subPosition]);
            }
        }
        return (dices.ExceptDices(result)).ExcludeReduplication();
    }
}
