using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected Animator _animator = null;
    protected SpriteRenderer _spriteRenderer = null;

    protected virtual void Awake()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
        _spriteRenderer = _animator.GetComponent<SpriteRenderer>();
    }

    protected bool SetPosition(DiceGrid grid, Vector2Int positionKey, Vector3 addPosition)
    {
        if(grid.grid.ContainsKey(positionKey))
        {
            transform.position = grid.grid[positionKey].groundPos + addPosition;
            return true;
        }
        return false;
    }

    public abstract void UseSkill(DiceUnit owner, DiceGrid grid, EDirection direction);
}
