using System.Collections;
using UnityEngine;

public class Skill_10101 : Skill
{
    public override void UseSkill(SUseSkillData data)
    {
        Vector2Int positionKey = data.owner.positionKey + Utility.EDirectionToVector(data.direction);
        SetPosition(data.grid, positionKey, Vector2.up * 0.43f);
        _spriteRenderer.flipX = data.direction == EDirection.Left;

        if(data.grid.units.ContainsKey(positionKey))
        {
            IDamagable damagable = data.grid.units[positionKey].GetComponent<IDamagable>();
            if(damagable == null)
            {
                Destroy(gameObject);
            }
            else
            {
                StartCoroutine(Test(damagable, data.grid.dices[positionKey].dicePip));
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Test(IDamagable target, int pip)
    {
        _animator.Play("UseSkill");
        for(int i = 0; i < 5; i++)
        {
            target.Damage(pip);
            yield return new WaitForSeconds(0.1f);
        }
        Destroy(gameObject);
    }
}
