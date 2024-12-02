using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleTon<StageManager>
{
    [SerializeField]
    private Stage _lobbyStage = null;
    [SerializeField]
    private FloorDataSO _FloorDataSO = null;

    public Stage currentStage { get; private set; }

    private int _floorIndex = 0;
    private int _stageIndex = -1;

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

        Stage newStage = Instantiate(_FloorDataSO.floors[_floorIndex].stageDatas[_stageIndex].stagePrefab);
        ChangeStage(newStage);
    }

    public void GoLobby()
    {
        ChangeStage(_lobbyStage);
    }

    private void ChangeStage(Stage stage)
    {
        if (currentStage != null)
        {
            currentStage.EndStage();
            Destroy(currentStage);
        }

        currentStage = stage;
        currentStage.StartStage();
    }
}