using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public bool IsUsable(DiceUnit owner)
    {
        bool result = ChildIsUsable(owner);
        if (result == false) Destroy(gameObject);
        return result;
    }
    protected abstract bool ChildIsUsable(DiceUnit owner);
    public abstract void UseSkill(DiceUnit owner);

    protected void PlayAndDestroy(Animator animator, string animationName)
    {
        animator.Play(animationName);
        animator.Update(0);
        StartCoroutine(PlayAndDestroyCoroutine(animator));
    }

    private IEnumerator PlayAndDestroyCoroutine(Animator animator)
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f);
        Destroy(gameObject);
    }
}
