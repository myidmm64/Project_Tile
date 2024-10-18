using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/StageData")]
public class StageDataSO : ScriptableObject
{
    public List<StageData> stages = new List<StageData>();
}

[System.Serializable]
public struct StageData
{
    public string stageName;
    public List<MapData> mapDatas;
}

[System.Serializable]
public struct MapData
{
    public string mapName;
    public DiceGenerateDataSO mapData;
}