using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StageData")]
public class FloorDataSO : ScriptableObject
{
    public List<FloorData> floors = new List<FloorData>();
}

[System.Serializable]
public struct FloorData
{
    public string floorName;
    public List<StageData> stageDatas;
}

[System.Serializable]
public struct StageData
{
    public string stageName;
    public Stage stagePrefab;
}