using UnityEngine;

[CreateAssetMenu(menuName = "SO/SkillData/SkillData")]
public class SkillDataSO : ScriptableObject
{
    [SerializeField]
    private Skill _skill;
    [SerializeField]
    private SkillRangeDataSO _rangeData;
    public SkillRangeDataSO rangeData => _rangeData;

    public T GetSkill<T>() where T : Skill
    {
        return GetSkill() as T;
    }

    public Skill GetSkill()
    {
        Skill skillCompo = Instantiate(_skill);
        if (skillCompo == null)
        {
            Debug.LogError("skillCompo 가져오지 못했음");
            return null;
        }

        return skillCompo;
    }
}
