using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleTon<StageManager>
{
    [SerializeField]
    private FloorDataSO _FloorDataSO = null;
    private int _floorIndex = 0;
    private int _stageIndex = -1;

    private void Start()
    {
        StartNextStage();
    }

    public StageData GetCurrentStageData()
    {
        return _FloorDataSO.floors[_floorIndex].stageDatas[_stageIndex];
    }

    public void StartNextStage()
    {
        // 맵 초기화 함수

        _stageIndex++;
        if (_stageIndex >= _FloorDataSO.floors[_floorIndex].stageDatas.Count)
        {
            _floorIndex++;
            if (_floorIndex >= _FloorDataSO.floors.Count)
            {
                Debug.Log("스테이지 끝끝끝");
                return;
            }
            _stageIndex = 0;
        }

        List<DiceUnit> spawnedUnits;
        DiceGrid.Inst.GenerateMap(_FloorDataSO.floors[_floorIndex].stageDatas[_stageIndex].stageGenData
            , out spawnedUnits);
        foreach(var unit in  spawnedUnits)
        {
            unit.StartStage(_FloorDataSO.floors[_floorIndex], _FloorDataSO.floors[_floorIndex].stageDatas[_stageIndex]);
        }
    }
}