using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    //[SerializeField]
    //protected RangeDataSO _rangeData;

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

    public List<DiceUnit> GetSkillTargets(RangeDataSO rangeData, DiceUnit owner) // skillRangeDataSO 내 정보를 통해 스킬 타겟 구하기
    {
        Vector2Int centerPos = GetCenterPos(rangeData, owner);
        List<Vector2Int> rangePosKeys = new List<Vector2Int>();
        foreach (var rangeOption in rangeData.GetRangeOptions())
        {
            rangePosKeys.AddRange(rangeOption.GetPosKeys(centerPos));
        }
        rangePosKeys.ExcludeReduplication();

        List<DiceUnit> targets = new List<DiceUnit>();
        foreach (var rangePosKey in rangePosKeys)
        {
            if (DiceGrid.Inst.units.ContainsKey(rangePosKey))
            {
                DiceUnit target = DiceGrid.Inst.units[rangePosKey];
                // 설정한 팀이거나, 본인 추가 설정을 했을 때
                if (rangeData.targetType == target.data.eTeam || rangeData.includeOwner && target.Equals(owner))
                {
                    targets.Add(target);
                }
            }
        }
        targets = GetSearchedTargets(rangeData, targets, centerPos);

        return targets;
    }

    private List<DiceUnit> GetSearchedTargets(RangeDataSO rangeData, List<DiceUnit> targets, Vector2Int centerPos)
    {
        switch (rangeData.searchType)
        {
            case ESearchType.None:
                return null;
            case ESearchType.Nearest:
                if (targets.Count == 0) return targets;
                DiceUnit nearestTarget = targets[0];
                foreach (var target in targets)
                {
                    float curNearestDist = (centerPos - nearestTarget.positionKey).sqrMagnitude;
                    float targetDist = (centerPos - target.positionKey).sqrMagnitude;

                    if (targetDist < curNearestDist)
                        nearestTarget = target;
                }
                targets.Clear();
                targets.Add(nearestTarget);
                return targets;
            case ESearchType.All:
                return targets;
            default:
                break;
        }
        return null;
    }

    private Vector2Int GetCenterPos(RangeDataSO rangeData, DiceUnit owner)
    {
        switch (rangeData.centerType)
        {
            case ECenterType.None:
                break;
            case ECenterType.Owner:
                return owner.positionKey;
            case ECenterType.Player:
                return DiceGrid.Inst.player.positionKey;
            case ECenterType.MapCenter:
                return MapManager.Inst.GetCurrentMapData().mapData.mapSize / 2;
            case ECenterType.PosKey:
                return rangeData.centerPosKey;
            default:
                break;
        }
        return Vector2Int.zero;
    }
}
