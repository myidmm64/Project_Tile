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
                return; // ���� ����Ʈ 0��°�� ������ �ߴ���, ���߿� Ÿ�� ���� �Լ� ������ ��ü 
            }
        }

        Destroy(gameObject);
    }
}
