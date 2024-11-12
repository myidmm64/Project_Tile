using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : EnemyPattern
{
    private const int normalAttackID = 100001;
    private AttackPatternDataSO _aData => data as AttackPatternDataSO;

    private SkillDataSO _skillData = null;

    public override void Initialize()
    {
        base.Initialize();
        _skillData = Utility.GetSkillDataSO(normalAttackID);
    }

    public override void Enter()
    {
        // 애니메이션 시작. n 초 뒤 공격 스킬 사용.
        // 유지 시간 종료 시 다음 패턴
        StartCoroutine(PatternCoroutine());
    }

    private IEnumerator PatternCoroutine()
    {
        // Animator
        yield return new WaitForSeconds(_aData.attackTerm);
        Skill_100001 attackSkill = _skillData.GetSkill<Skill_100001>();
        attackSkill.Attack(_aData.attackRange);
        yield return new WaitForSeconds(_aData.patternDuration - _aData.attackTerm);
        isEnded = true;
    }

    public override void Exit()
    {
    }

    public override void PatternUpdate()
    {
    }

    public override bool Startable()
    {
        if (_aData.excludeRange)
        {
            List<DiceUnit> targets = DiceGrid.Inst.GetIncludedDiceUnits(_aData.attackRange, _enemy);
            return targets.Count > 0;
        }
        else
        {
            return true;
        }
    }
}
