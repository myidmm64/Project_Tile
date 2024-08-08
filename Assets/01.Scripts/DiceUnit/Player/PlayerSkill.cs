using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    public bool IsMPSkill = false; // mp를 사용하는 스킬인지
    public int maxMP = 100; // 해당 스킬을 사용하기 위해 필요한 mp는 무엇인지
    private PlayerSkill _skill = null;

    public void UseSkill()
    {

    }
}
