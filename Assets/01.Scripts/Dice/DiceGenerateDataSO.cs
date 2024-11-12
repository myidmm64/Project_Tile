using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DiceGenerateData")]
public class DiceGenerateDataSO : ScriptableObject
{
    public Vector2Int mapSize = Vector2Int.zero;
    public List<Vector2Int> subPositions = new List<Vector2Int>();
    public Vector2 centerPos = Vector2.zero;
    public Vector2 padding = Vector2.one;

    public Vector2Int playerPos = Vector2Int.zero;
    public List<SpawnData> spawnDatas = new List<SpawnData>();
}

[System.Serializable]
public struct SpawnData
{
    public DiceUnit unit;
    public Vector2Int pos;
}