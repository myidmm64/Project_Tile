using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceGrid : MonoSingleTon<DiceGrid>
{
    public Dictionary<Vector2Int, Dice> dices { get; private set; }
    public Dictionary<Vector2Int, DiceUnit> units { get; private set; }

    public void SetGrid(Dictionary<Vector2Int, Dice> diceGrid, Dictionary<Vector2Int, DiceUnit> unitGrid)
    {
        dices.Clear();
        units.Clear();

        dices = diceGrid;
        units = unitGrid;
    }

    public delegate bool BFSSearchEvent(Vector2Int posKey); // 찾는 거 성공하면 true 반환해주기

    // 가장 가까운 빈 Dice의 PositionKey 추출
    public Vector2Int FindClosestDice(Vector2Int start, EDirection firstSearchDir = EDirection.Right)
    {
        Vector2Int result = new Vector2Int(-1, -1);
        BFSSearchEvent closestSearch = (posKey) =>
        {
            if (dices.ContainsKey(posKey))
            {
                result = posKey;
                return true;
            }
            return false;
        };
        SearchWithBFS(start, closestSearch, firstSearchDir);
        return result;
    }

    // 가장 가까운 Team의 PositionKey 추출
    public Vector2Int FindClosestTeam(Vector2Int start, ETeam team, EDirection firstSearchDir = EDirection.Right)
    {
        Vector2Int result = new Vector2Int(-1, -1);
        BFSSearchEvent closestSearch = (posKey) =>
        {
            if (units.ContainsKey(posKey))
            {
                if (units[posKey].data.eTeam == team)
                {
                    result = posKey;
                    return true;
                }
            }
            return false;
        };
        SearchWithBFS(start, closestSearch, firstSearchDir);
        return result;
    }

    // 가장 가까운 DiceUnit의 PositionKey 추출
    public Vector2Int FindClosestUnit<T>(Vector2Int start, EDirection firstSearchDir = EDirection.Right) where T : DiceUnit
    {
        Vector2Int result = new Vector2Int(-1, -1);
        BFSSearchEvent closestSearch = (posKey) =>
        {
            if (units.ContainsKey(posKey))
            {
                if (units[posKey] is T)
                {
                    result = posKey;
                    return true;
                }
            }
            return false;
        };
        SearchWithBFS(start, closestSearch, firstSearchDir);
        return result;
    }


    public void SearchWithBFS(Vector2Int start, BFSSearchEvent action, EDirection firstSearchDir = EDirection.Right)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int[] directions = new Vector2Int[]
        {
            Utility.GetRotatedVector(new Vector2Int(1, 0), firstSearchDir),
            Utility.GetRotatedVector(new Vector2Int(0, -1), firstSearchDir),
            Utility.GetRotatedVector(new Vector2Int(-1, 0), firstSearchDir),
            Utility.GetRotatedVector(new Vector2Int(0, 1), firstSearchDir),
        };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            // 적이 있는 타일을 찾으면 반환
            if (current != start)
            {
                if(action.Invoke(current))
                {
                    return;
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
    }

    public List<Dice> GetDices(List<Vector2Int> positionKeies)
    {
        List<Dice> result = new List<Dice>();
        foreach (var positionKey in positionKeies)
        {
            if (dices.ContainsKey(positionKey)) result.Add(dices[positionKey]);
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
            if (units.ContainsKey(positionKey))
            {
                if (units[positionKey] is T)
                {
                    result.Add(units[positionKey]);
                }
            }
        }
        return result;
    }

    public IEnumerable<Dice> GetSamePipDices(int dicePip)
    {
        var query = from dice in dices.Values
                    where dice.dicePip == dicePip
                    select dice;
        return query;
    }

    public IEnumerable<Dice> GetDiceRow(int rowNum)
    {
        var query = from diceKeyValue in dices
                    where diceKeyValue.Key.y == rowNum
                    select diceKeyValue.Value;
        return query;
    }

    public IEnumerable<Dice> GetDiceColumn(int columnNum)
    {
        var query = from diceKeyValue in dices
                    where diceKeyValue.Key.x == columnNum
                    select diceKeyValue.Value;
        return query;
    }

    public List<Vector2Int> GetRangePosKeys(Vector2Int centerPos, RangeDataSO rangeData)
    {
        List<Vector2Int> rangePosKeys = new List<Vector2Int>();
        foreach (var rangeOption in rangeData.GetRangeOptions())
        {
            if (rangeOption.isSub)
                rangePosKeys.SubKeys(rangeOption.GetPosKeys(centerPos).ToArray());
            else
                rangePosKeys.AddRange(rangeOption.GetPosKeys(centerPos));
        }
        rangePosKeys.ExcludeReduplication();
        return rangePosKeys;
    }

    public List<DiceUnit> GetIncludedDiceUnits(RangeDataSO rangeData, DiceUnit owner) // skillRangeDataSO 내 정보를 통해 스킬 타겟 구하기
    {
        Vector2Int centerPos = GetCenterPos(rangeData, owner);
        List<Vector2Int> rangePosKeys = GetRangePosKeys(centerPos, rangeData);

        List<DiceUnit> targets = new List<DiceUnit>();
        foreach (var rangePosKey in rangePosKeys)
        {
            if (units.ContainsKey(rangePosKey))
            {
                DiceUnit target = units[rangePosKey];
                // 설정한 팀이거나, 본인 추가 설정을 했을 때
                if (rangeData.targetType == ETeam.None
                    || rangeData.targetType == target.data.eTeam
                    || rangeData.includeOwner && target.Equals(owner))
                {
                    targets.Add(target);
                }
            }
        }
        targets = GetSearchedTargets(rangeData, targets, centerPos);

        return targets;
    }

    private List<DiceUnit> GetSearchedTargets(RangeDataSO rangeData, List<DiceUnit> targets, Vector2Int centerPos)
    {
        switch (rangeData.searchType)
        {
            case ESearchType.None:
                return null;
            case ESearchType.Nearest:
                if (targets.Count == 0) return targets;
                DiceUnit nearestTarget = targets[0];
                foreach (var target in targets)
                {
                    float curNearestDist = (centerPos - nearestTarget.positionKey).sqrMagnitude;
                    float targetDist = (centerPos - target.positionKey).sqrMagnitude;

                    if (targetDist < curNearestDist)
                        nearestTarget = target;
                }
                targets.Clear();
                targets.Add(nearestTarget);
                return targets;
            case ESearchType.All:
                return targets;
            default:
                break;
        }
        return null;
    }

    private Vector2Int GetCenterPos(RangeDataSO rangeData, DiceUnit owner)
    {
        switch (rangeData.centerType)
        {
            case ECenterType.None:
                break;
            case ECenterType.Owner:
                return owner.positionKey;
            case ECenterType.Player:
                return Utility.player.positionKey;
            case ECenterType.MapCenter:
                return Vector2Int.zero; //StageManager.Inst.currentStage.genData.mapSize / 2;
            case ECenterType.PosKey:
                return rangeData.centerPosKey;
            default:
                break;
        }
        return Vector2Int.zero;
    }
}
