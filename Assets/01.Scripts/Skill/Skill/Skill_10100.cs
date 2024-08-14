using UnityEngine;

public class Skill_10100 : Skill
{
    public override void UseSkill(SUseSkillData data)
    {
        PlayerDiceUnit player = data.owner as PlayerDiceUnit;
        if (player != null)
        {
            foreach (var attackTarget in player.attackModule.GetAttackTargets())
            {
                IDamagable damagable = attackTarget.GetComponent<IDamagable>();
                damagable.Damage(player.dice.dicePip);
                player.animator.Play("NormalAttack");

                data.specialActions["SuccessAttack"]?.Invoke();
                Destroy(gameObject);
                return; // 현재 리스트 0번째만 때리고 중단함, 나중에 타겟 설정 함수 나오면 교체 
            }
        }

        Destroy(gameObject);
    }
}
