using System;
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

    public Action<int> OnChangeFloor = null;
    public Action OnChangeStage = null;
    public Action OnGoLobby = null;

    public void GoLobby()
    {
        _floorIndex = 0;
        _stageIndex = -1;

        ChangeStage(_lobbyStage);
        OnChangeFloor?.Invoke(0); // �κ�� 0��
        OnChangeStage?.Invoke(); // �ʱ�ȭ��
        OnGoLobby?.Invoke();

    }

    public void StartDungeon()
    {
        _floorIndex = 0;
        _stageIndex = -1;
        StartNextStage();
        OnChangeFloor?.Invoke(_floorIndex + 1);
    }

    public void StartNextStage()
    {
        // �� �ʱ�ȭ �Լ�

        _stageIndex++;
        bool changeFloor = _stageIndex >= _FloorDataSO.floors[_floorIndex].stageDatas.Count;
        if (changeFloor)
        {
            _floorIndex++;
            if (_floorIndex >= _FloorDataSO.floors.Count)
            {
                Debug.Log("�������� ������");
                return;
            }
            _stageIndex = 0;
        }

        Stage newStage = Instantiate(_FloorDataSO.floors[_floorIndex].stageDatas[_stageIndex].stagePrefab);
        ChangeStage(newStage);

        if (changeFloor) OnChangeFloor?.Invoke(_floorIndex + 1); // �ε����� ������ +1
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
        OnChangeStage?.Invoke();
    }
}