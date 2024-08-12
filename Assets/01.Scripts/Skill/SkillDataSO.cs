using UnityEngine;

[CreateAssetMenu(menuName = "SO/SkillData")]
public class SkillDataSO : ScriptableObject
{
    [SerializeField]
    private int _maxMP;
    public int MaxMP => _maxMP;
    [SerializeField]
    private Sprite _skillImage;
    public Sprite SkillImage => _skillImage;
    [SerializeField]
    private string _skillName;
    public string SkillName => _skillName;

    [SerializeField]
    private GameObject _skillPrefab;

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
