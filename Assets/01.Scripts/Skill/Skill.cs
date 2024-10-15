using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected Animator _animator = null;
    protected SpriteRenderer _spriteRenderer = null;
    protected SSkillData _skillData;

    protected virtual void Awake()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _spriteRenderer = _animator.GetComponent<SpriteRenderer>();
    }

    protected bool SetPosition(DiceGrid grid, Vector2Int positionKey, Vector3 addPosition)
    {
        if(grid.dices.ContainsKey(positionKey))
        {
            transform.position = grid.dices[positionKey].groundPos + addPosition;
            return true;
        }
        return false;
    }

    public void SetSkillData(SSkillData skillData)
    {
        _skillData = skillData;
    }
    public abstract void UseSkill(SUseSkillData data);
}
