using UnityEngine;

[CreateAssetMenu(menuName = "SO/SkillData/PlayerSkillData")]
public class PlayerSkillDataSO : SkillDataSO
{
    public EPlayerSkillType skillType;
    public string skillName;
    public string explainText;
    public Sprite skillImage;

    public int maxDP;
    public float cooltime;
}

public enum EPlayerSkillType
{
    None,
    Main,
    Special,
    Counter
}