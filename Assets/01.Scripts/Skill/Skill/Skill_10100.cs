using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill_10100 : Skill
{
    public override void UseSkill(SUseSkillData data)
    {
        _animator.Play($"Effect{(int)data.otherData}");
        StartCoroutine(AnimEndDestroy());

        PlayerDiceUnit player = data.owner as PlayerDiceUnit;
        if (player != null)
        {
            foreach (var attackTarget in player.attackModule.GetAttackTargets())
            {
                IDamagable damagable = attackTarget.GetComponent<IDamagable>();
                damagable.Damage(player.dice.dicePip);

                data.specialActions["SuccessAttack"]?.Invoke();

                return; // 현재 리스트 0번째만 때리고 중단함, 나중에 타겟 설정 함수 나오면 교체 
            }
        }
    }

    private IEnumerator AnimEndDestroy()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f);
        Destroy(gameObject);
    }
}
