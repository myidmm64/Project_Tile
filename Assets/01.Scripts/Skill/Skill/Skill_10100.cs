using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill_10100 : Skill
{
    public override void UseSkill(SUseSkillData data)
    {
        transform.SetPositionAndRotation((Vector2)data.otherDatas["Position"], (Quaternion)data.otherDatas["Rotation"]);
        Utility.SetLocalScaleByDirection(transform, data.direction);

        _animator.Play($"Effect{(int)data.otherDatas["Index"]}");
        StartCoroutine(AnimEndDestroy());

        PlayerDiceUnit player = data.owner as PlayerDiceUnit;
        if (player != null)
        {
            foreach (var attackTarget in player.GetModule<PlayerAttackModule>().GetAttackTargets())
            {
                IDamagable damagable = attackTarget.GetComponent<IDamagable>();
                damagable.Damage(player.dice.dicePip);

                Action attackCallback = (Action)data.otherDatas["AttackCallback"];
                attackCallback?.Invoke();

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
