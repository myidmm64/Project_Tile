using AllIn1SpriteShader;
using System;
using UnityEngine;

public class DiceUnitSprite : MonoBehaviour
{
    [SerializeField]
    protected DiceUnit _owner = null;
    public EDirection direction { get; private set; }

    public SpriteRenderer spriteRenderer { get; private set; }
    public Animator animator { get; private set; }
    public bool lockFlip = false;

    protected AllIn1Shader _shader = null;

    private void Awake()
    {
        if(_owner == null) _owner = transform.parent.GetComponent<DiceUnit>(); // DiceUnit 바로 아래 Sprite가 있을 거임
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        _shader = GetComponent<AllIn1Shader>();
    }

    public void SetLockFlip(int value)
    {
        lockFlip = Convert.ToBoolean(value);
    }

    public void LookAt(Vector2Int targetPositionKey)
    {
        LookAt(_owner.positionKey, targetPositionKey);
    }

    public void LookAt(Vector2 startPos, Vector2 targetPos)
    {
        if (lockFlip) return;
        float xCross = Vector3.Cross(Vector3.down, targetPos - startPos).z;
        if (xCross > 0)
        {
            if (spriteRenderer != null)
            {
                Vector3 localScale = transform.localScale;
                localScale.x = Mathf.Abs(localScale.x);
                transform.localScale = localScale;
            }// spriteRenderer.flipX = false;

            direction = EDirection.Right;
        }
        else if (xCross < 0)
        {
            if (spriteRenderer != null)
            {
                Vector3 localScale = transform.localScale;
                localScale.x = Mathf.Abs(localScale.x) * -1f;
                transform.localScale = localScale;
            }//spriteRenderer.flipX = true;

            direction = EDirection.Left;
        }
    }
}
