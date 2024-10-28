using UnityEngine;

[CreateAssetMenu(menuName = "SO/SkillData")]
public class SkillDataSO : ScriptableObject
{
    [SerializeField]
    private SSkillExplainData _data;
    public SSkillExplainData data => _data;
    [SerializeField]
    private GameObject _skillPrefab;

    public T GetSkill<T>() where T : Skill
    {
        return GetSkill() as T;
    }

    public Skill GetSkill()
    {
        GameObject obj = Instantiate(_skillPrefab);
        if(obj == null)
        {
            Debug.LogError("_skillPrefab 가져오지 못했음");
            return null;
        }
        Skill skillCompo = obj.GetComponent<Skill>();
        if (skillCompo == null)
        {
            Debug.LogError("skillCompo 가져오지 못했음");
            return null;
        }

        return skillCompo;
    }
}
