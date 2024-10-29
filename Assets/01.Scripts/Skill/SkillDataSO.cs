using UnityEngine;

[CreateAssetMenu(menuName = "SO/SkillData/SkillData")]
public class SkillDataSO : ScriptableObject
{
    [SerializeField]
    private Skill _skill;

    public T GetSkill<T>() where T : Skill
    {
        return GetSkill() as T;
    }

    public Skill GetSkill()
    {
        Skill skillCompo = Instantiate(_skill);
        if (skillCompo == null)
        {
            Debug.LogError("skillCompo �������� ������");
            return null;
        }

        return skillCompo;
    }
}
