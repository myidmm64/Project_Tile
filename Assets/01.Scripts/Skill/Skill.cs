using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField]
    protected RangeDataSO _rangeData;

    public bool IsUsable(DiceUnit owner)
    {
        bool result = ChildIsUsable(owner);
        if (result == false) Destroy(gameObject);
        return result;
    }
    protected abstract bool ChildIsUsable(DiceUnit owner);
    public abstract void UseSkill(DiceUnit owner);

    protected void PlayAndAction(Animator animator, string animationName, Action Callback)
    {
        animator.Play(animationName);
        animator.Update(0);
        StartCoroutine(PlayAndDestroyCoroutine(animator, Callback));
    }

    private IEnumerator PlayAndDestroyCoroutine(Animator animator, Action Callback)
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f);
        Callback?.Invoke();
    }
}
