using UnityEngine;

[CreateAssetMenu(menuName = "SO/DiceUnitData")]
public class DiceUnitData : ScriptableObject
{
    public ETeam eTeam;
    public string unitName;
    public StatData baseStat;
}
