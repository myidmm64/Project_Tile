using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Stage : MonoBehaviour
{
    public DiceGenerateDataSO data = null; // 생성 데이터
    public string stageName;

    [SerializeField]
    protected UnityEvent OnStartStage = null;
    [SerializeField]
    protected UnityEvent OnEndStage = null;
    private Transform _gridTrm = null;

    public virtual void Awake()
    {
        _gridTrm = transform.Find("Grid");
        if(_gridTrm == null)
        {
            Debug.Log("GridTrm 생성");
            _gridTrm = new GameObject("Grid").transform;
            _gridTrm.SetParent(transform);
            _gridTrm.localPosition = Vector3.zero;
        }
    }

    public virtual void StartStage()
    {
        GenerateMap();
        OnStartStage?.Invoke();
    }

    public virtual void EndStage()
    {
        OnEndStage?.Invoke();
    }

    public virtual bool IsStageEnded()
    {
        foreach(var unit in DiceGrid.Inst.units.Values)
        {
            if (unit.CurHP > 0) return false;
        }
        return true;
    }

    protected virtual void ResetPreview()
    {
        Transform previewTrm = transform.Find("Preview");
        if (previewTrm != null)
        {
            Destroy(previewTrm);
        }
        previewTrm = new GameObject("Preview").transform;
        previewTrm.SetParent(transform);
        previewTrm.localPosition = Vector3.zero;
    }

    [ContextMenu("미리보기 생성")]
    public virtual void SetPreview()
    {
        ResetPreview();
        Transform previewTrm = transform.Find("Preview");
        Debug.Log("제작중");
    }

    public void GenerateMap()
    {
        /*
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
                    dice.SetSpriteOrder();
                    dice.transform.SetParent(transform, false);

                    _editorDices.Add(positionKey, dice);
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

            DiceGrid.Inst.SetGrid(_dices, _units);
        */
    }
}