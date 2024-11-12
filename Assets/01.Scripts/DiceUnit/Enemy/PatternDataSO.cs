using UnityEngine;

[CreateAssetMenu(menuName = "SO/PatternData/PatternData")]
public class PatternDataSO : ScriptableObject
{
    public int priority = 0;
    public float cooltime = 0f;
}

[CreateAssetMenu(menuName = "SO/PatternData/IdlePatternData")]
public class IdlePatternDataSO : PatternDataSO
{
    public float idleDuration = 0f;
}

[CreateAssetMenu(menuName = "SO/PatternData/AttackPatternData")]
public class AttackPatternDataSO : PatternDataSO
{
    public RangeDataSO attackRange = null;
    public bool excludeRange = false;

    public string animationName = string.Empty;
    public float attackTerm = 0f;
    public float patternDuration = 0f;
}

[CreateAssetMenu(menuName = "SO/PatternData/MovePatternData")]
public class MovePatternDataSO : PatternDataSO
{

    public Vector2Int poskey = Vector2Int.zero;


}