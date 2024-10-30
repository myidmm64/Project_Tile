using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceGrid : MonoSingleTon<DiceGrid>
{
    public Dictionary<Vector2Int, Dice> dices { get; private set; }
    public Dictionary<Vector2Int, DiceUnit> units { get; private set; }
    // 위험지역 그리드도 만들기

    private Player _player = null;
    public Player player
    {
        get
        {
            if(_player == null)
                _player = FindFirstObjectByType<Player>();
            return _player;
        }
    }

    private void Awake()
    {
        dices = new Dictionary<Vector2Int, Dice>();
        units = new Dictionary<Vector2Int, DiceUnit>();
    }

    public void GenerateMap(DiceGenerateDataSO data)
    {
        // 초기화 함수 추가

        float totalWidth = data.padding.x * (data.mapSize.x - 1); // 간격의 수(격자 크기의 -1)만큼 곱해 전체 크기를 구함
        float totalHeight = data.padding.y * (data.mapSize.y - 1);

        Vector2 startPos = new Vector2(
            data.centerPos.x - (totalWidth / 2),
            data.centerPos.y - (totalHeight / 2));

        for (int y = 0; y < data.mapSize.y; y++)
        {
            for (int x = 0; x < data.mapSize.x; x++)
            {
                if (data.subPositions.Contains(new Vector2Int(x, y))) continue;
                Dice dice = PoolManager.Inst.Pop(EPoolType.Dice) as Dice; // PopDice((EDiceType)number);
                if (dice == null) continue;

                Vector2 dicePosition = startPos + new Vector2(x * data.padding.x, y * data.padding.y);
                Vector2Int positionKey = new Vector2Int(x, y);

                dice.transform.position = dicePosition;
                dice.positionKey = positionKey;
                dice.gameObject.name = $"dice : {positionKey.ToString()}";
                dice.Roll();
                dice.SetSpriteOrder();
                dice.transform.SetParent(transform, false);

                dices.Add(positionKey, dice);
            }
        }

        player.ChangeDice(data.playerPos);
    }

    // 가장 가까운 빈 Dice의 PositionKey 추출
    public Vector2Int FindClosestDice(Vector2Int start, EDirection firstSearchDir = EDirection.Right)
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
            if (current != start && dices.ContainsKey(current))
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

    // 가장 가까운 Team의 PositionKey 추출
    public Vector2Int FindClosestTeam(Vector2Int start, ETeam team, EDirection firstSearchDir = EDirection.Right)
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
            if (current != start && units.ContainsKey(current))
            {
                if (units[current].data.eTeam == team)
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

    // 가장 가까운 DiceUnit의 PositionKey 추출
    public Vector2Int FindClosestUnit<T>(Vector2Int start, EDirection firstSearchDir = EDirection.Right) where T : DiceUnit
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
            if (current != start && units.ContainsKey(current))
            {
                if (units[current] is T)
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

    public List<DiceUnit> GetIncludedDiceUnits(RangeDataSO rangeData, DiceUnit owner) // skillRangeDataSO 내 정보를 통해 스킬 타겟 구하기
    {
        Vector2Int centerPos = GetCenterPos(rangeData, owner);
        List<Vector2Int> rangePosKeys = new List<Vector2Int>();
        foreach (var rangeOption in rangeData.GetRangeOptions())
        {
            if(rangeOption.isSub)
                rangePosKeys.SubKeys(rangeOption.GetPosKeys(centerPos).ToArray());
            else
                rangePosKeys.AddRange(rangeOption.GetPosKeys(centerPos));
        }
        rangePosKeys.ExcludeReduplication();

        List<DiceUnit> targets = new List<DiceUnit>();
        foreach (var rangePosKey in rangePosKeys)
        {
            if (units.ContainsKey(rangePosKey))
            {
                DiceUnit target = units[rangePosKey];
                // 설정한 팀이거나, 본인 추가 설정을 했을 때
                if (rangeData.targetType == target.data.eTeam || rangeData.includeOwner && target.Equals(owner))
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
                return player.positionKey;
            case ECenterType.MapCenter:
                return MapManager.Inst.GetCurrentMapData().mapData.mapSize / 2;
            case ECenterType.PosKey:
                return rangeData.centerPosKey;
            default:
                break;
        }
        return Vector2Int.zero;
    }
}
