using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoSingleTon<MapManager>
{
    [SerializeField]
    private StageDataSO _stageDataSO = null;
    private int _stageIndex = 0;
    private int _mapIndex = -1;

    private void Start()
    {
        StartNextMap();
    }

    public void StartNextMap()
    {
        // �� �ʱ�ȭ �Լ�

        _mapIndex++;
        if (_mapIndex >= _stageDataSO.stages[_stageIndex].mapDatas.Count)
        {
            _stageIndex++;
            if (_stageIndex >= _stageDataSO.stages.Count)
            {
                Debug.Log("�������� ������");
                return;
            }
            _mapIndex = 0;
            ChangeStage();
        }
        
        DiceGrid.Inst.GenerateMap(_stageDataSO.stages[_stageIndex].mapDatas[_mapIndex].mapData);
    }

    public void ChangeStage()
    {

    }
}