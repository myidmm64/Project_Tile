using UnityEngine;

[CreateAssetMenu(menuName = "SO/EnemyPatternData")]
public class EnemyPatternDataSO : ScriptableObject
{
    [Header("�켱��, ���� ���� ����")]
    public int priority = 0;
    [Header("��Ÿ��")]
    public float cooltime = 0f;
}
