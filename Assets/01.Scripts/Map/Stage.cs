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
    private UnityEvent OnStartStage = null;
    [SerializeField]
    private UnityEvent OnEndStage = null;

    [SerializeField]
    private Vector2Int _playerPos = Vector2Int.zero;
    public Vector2Int playerPos => _playerPos;

    // 커스텀 에디터로 이거 만들기
    [SerializeField]
    private DiceDictionary _dices = new();
    [SerializeField]
    private UnitsDictionary _units = new();

    public void StartStage()
    {
        OnStartStage?.Invoke();
    }

    public void EndStage()
    {
        OnEndStage?.Invoke();
    }

    public virtual bool IsStageEnded()
    {
        return false;
    }
}
