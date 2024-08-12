using System.Collections;
using UnityEngine;

public class Skill_10101 : Skill
{
    public override void UseSkill(DiceUnit owner, DiceGrid grid, EDirection direction)
    {
        Vector2Int positionKey = owner.positionKey + Utility.GetDirection(direction);
        SetPosition(grid, positionKey, Vector2.up * 0.43f);
        _spriteRenderer.flipX = direction == EDirection.Left;

        if(grid.diceUnitGrid.ContainsKey(positionKey))
        {
            IDamagable damagable = grid.diceUnitGrid[positionKey].GetComponent<IDamagable>();
            if(damagable != null )
            {
                StartCoroutine(Test(damagable, grid.grid[positionKey].dicePip));
            }
        }
    }

    private IEnumerator Test(IDamagable target, int pip)
    {
        _animator.Play("UseSkill");
        Debug.Log("스킬 시작");
        for(int i = 0; i < 5; i++)
        {
            target.Damage(pip);
            yield return new WaitForSeconds(0.1f);
        }
        Debug.Log("스킬 끝");
        Destroy(gameObject);
    }
}
