using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField]
    protected RangeDataSO _rangeData;
    protected List<DiceUnit> _targets = new List<DiceUnit>();
    public List<DiceUnit> targets => _targets;

    public bool UseSkill(DiceUnit owner)
    {
        if (IsUsable(owner))
        {
            SkillLogic(owner);
            return true;
        }
        return false;
    }

    public virtual bool IsUsable(DiceUnit owner)
    {
        _targets = DiceGrid.Inst.GetIncludedDiceUnits(_rangeData, owner);
        return _targets.Count > 0;
    }

    protected abstract void SkillLogic(DiceUnit owner);

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
