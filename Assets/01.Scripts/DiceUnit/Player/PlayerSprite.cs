using DG.Tweening;
using UnityEngine;

public class PlayerSprite : DiceUnitSprite
{
    private Sequence _aniSeq = null;
    private Vector2 _originPos = Vector2.zero;

    private void Start()
    {
        _originPos = transform.localPosition;
    }

    private void Update()
    {
        LookAttackTarget();
    }

    private void LookAttackTarget()
    {
        Player player = _owner as Player;
        var attackTargets = player.GetModule<PlayerAttackModule>().CurWeapon.attackTargets;
        if (attackTargets.Count > 0)
        {
            LookAt(attackTargets[0].positionKey);
        }
        else
        {
            LookAt(player.grid.FindClosestUnit<DiceUnit>(player.positionKey));
        }
    }

    public void AttackAnimation(int idx)
    {
        animator.Play($"Attack{idx}");
    }

    public void SkillAnimation(int idx)
    {
        animator.Play($"Skill{idx}");
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
            transform.localPosition = _originPos;
            _aniSeq.Kill();
        }

        _aniSeq = DOTween.Sequence();
        _aniSeq.Append(transform.DOLocalMove(_originPos + targetPos, duration)).SetEase(Ease.OutExpo);
        _aniSeq.Append(transform.DOLocalMove(_originPos, duration)).SetEase(Ease.Linear);
    }
}
