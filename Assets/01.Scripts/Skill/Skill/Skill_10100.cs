using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Skill_10100 : Skill
{
    public override void UseSkill(SUseSkillData data)
    {
        FaceTarget(data.owner.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));

        _animator.Play("UseSkill");
        StartCoroutine(qweqwe());

        PlayerDiceUnit player = data.owner as PlayerDiceUnit;
        if (player != null)
        {
            foreach (var attackTarget in player.attackModule.GetAttackTargets())
            {
                IDamagable damagable = attackTarget.GetComponent<IDamagable>();
                damagable.Damage(player.dice.dicePip);
                player.animator.Play("NormalAttack");

                data.specialActions["SuccessAttack"]?.Invoke();
                // Destroy(gameObject);
                return; // 현재 리스트 0번째만 때리고 중단함, 나중에 타겟 설정 함수 나오면 교체 
            }
        }

        //  Destroy(gameObject);
    }

    void FaceTarget(Vector3 start, Vector3 target)
    {
        transform.position = start + Vector3.up * 0.8f + (target - start).normalized * 1.5f;

        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
    }

    private IEnumerator qweqwe()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f);
        Destroy(gameObject);
    }
}
