using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyPatternData")]
public class EnemyPatternDataSO : ScriptableObject
{
    [Header("우선도, 높을 수록 빨리")]
    public int priority = 0;
    [Header("쿨타임")]
    public float cooltime = 0f;
}
