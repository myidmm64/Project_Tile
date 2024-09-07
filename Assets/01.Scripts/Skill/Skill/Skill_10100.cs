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

                return; // ���� ����Ʈ 0��°�� ������ �ߴ���, ���߿� Ÿ�� ���� �Լ� ������ ��ü 
            }
        }
    }

    private IEnumerator AnimEndDestroy()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f);
        Destroy(gameObject);
    }
}
