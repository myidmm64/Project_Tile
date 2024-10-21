using DG.Tweening;
using UnityEngine;

public class PlayerSprite : DiceUnitSprite
{
    private Sequence _aniSeq = null;

    public void AttackAnimation(int idx)
    {
        animator.Play($"Attack{idx}");
    }

    public void MoveAnimation(float horizontal, float vertical)
    {
        animator.SetFloat("Horizontal", horizontal * (direction == EDirection.Left ? -1f : 1f));
        animator.SetFloat("Vertical", vertical);
        animator.Play("Move");
        animator.Update(0);
    }

    public void IdleAnimation()
    {
        animator.Play("Idle");
        animator.Update(0);
    }

    public void AdvanceMove(Vector2 targetPos, float duration)
    {
        if (_aniSeq != null && _aniSeq.active)
        {
            transform.localPosition = Vector3.zero;
            _aniSeq.Kill();
        }

        _aniSeq = DOTween.Sequence();
        _aniSeq.Append(transform.DOLocalMove(targetPos, duration));
        _aniSeq.Append(transform.DOLocalMove(Vector3.zero, duration));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceMove(Vector2.right, 0.2f);
        }
    }
}
