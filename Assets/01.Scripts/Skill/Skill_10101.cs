using System.Collections;
using UnityEngine;

public class Skill_10101 : Skill
{
    public override void UseSkill(DiceUnit owner, DiceGrid grid, Vector2Int spawnPositionKey, EDirection direction)
    {
        SetPosition(grid, spawnPositionKey, Vector2.zero);
        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        _animator.Play("UseSkill");
        Debug.Log("스킬 시작");
        yield return new WaitForSeconds(1);
        Debug.Log("스킬 끝");
        Destroy(gameObject);
    }
}
