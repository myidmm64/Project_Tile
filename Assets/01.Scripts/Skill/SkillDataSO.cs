using UnityEngine;

[CreateAssetMenu(menuName = "SO/SkillData")]
public class SkillDataSO : ScriptableObject
{
    public SSkillData data;
    [SerializeField]
    private GameObject _skillPrefab;

    public Skill GetSkill()
    {
        GameObject obj = Instantiate(_skillPrefab);
        if(obj == null)
        {
            Debug.LogError("_skillPrefab �������� ������");
            return null;
        }
        Skill skillCompo = obj.GetComponent<Skill>();
        if (skillCompo == null)
        {
            Debug.LogError("skillCompo �������� ������");
            return null;
        }

        return skillCompo;
    }
}
