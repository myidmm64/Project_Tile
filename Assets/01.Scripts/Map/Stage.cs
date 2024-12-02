using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class DiceDictionary : SerializableDictionary<Vector2Int, Dice> { }
[Serializable]
public class UnitsDictionary : SerializableDictionary<Vector2Int, DiceUnit> { }

public class Stage : MonoBehaviour
{
    [SerializeField]
    protected UnityEvent OnStartStage = null;
    [SerializeField]
    protected UnityEvent OnEndStage = null;
    public DiceGenerateDataSO genData = null;

    [SerializeField]
    private Vector2Int _playerPos = Vector2Int.zero;
    public Vector2Int playerPos => _playerPos;

    // 커스텀 에디터로 이거 만들기
    [SerializeField]
    private DiceDictionary _editorDices = new();
    private Dictionary<Vector2Int, Dice> _dices;

    [SerializeField]
    private UnitsDictionary _editorUnits = new();
    private Dictionary<Vector2Int, DiceUnit> _units;

    public void StartStage()
    {
        // 에디터 상황은 좀 위험하니 인게임에서 할당해주기
        _dices = new Dictionary<Vector2Int, Dice>();
        foreach (var editorDice in _editorDices)
        {
            _dices.Add(editorDice.Key, editorDice.Value);
        }
        _units = new Dictionary<Vector2Int, DiceUnit>();
        foreach (var editorUnit in _editorUnits)
        {
            _units.Add(editorUnit.Key, editorUnit.Value);
        }
        DiceGrid.Inst.SetGrid(_dices, _units);

        OnStartStage?.Invoke();
    }

    public void EndStage()
    {
        OnEndStage?.Invoke();
    }

    public virtual bool IsStageEnded()
    {
        foreach(var unit in _units.Values)
        {
            if (unit.CurHP > 0) return true;
        }
        return false;
    }

    /*
    public void GenerateMap(DiceGenerateDataSO data, out List<DiceUnit> spawnedUnits)
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
                dice.Roll(); // 추후 숫자 통일 가능
                dice.SetSpriteOrder();
                dice.transform.SetParent(transform, false);

                dices.Add(positionKey, dice);
            }
        }

        spawnedUnits = new List<DiceUnit>();

        player.ChangeDice(data.playerPos);
        player.transform.position = player.dice.groundPos;
        spawnedUnits.Add(player);

        foreach (var spawnData in data.spawnDatas)
        {
            DiceUnit unit = Instantiate(spawnData.unit);
            if (unit.ChangeDice(spawnData.pos) == false)
            {
                Debug.LogError("?? 이상한 위치에 생성한 것 같아요");
            }
            unit.transform.position = unit.dice.groundPos;
            spawnedUnits.Add(unit);
            if (unit is Enemy)
            {
                _enemys.Add(unit as Enemy);
            }
        }
    }
     */
}