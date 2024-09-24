using UnityEngine;

[CreateAssetMenu(menuName = "SO/DiceUnitData")]
public class DiceUnitData : ScriptableObject
{
    public string unitName;
    public int maxHP = 100;
}
