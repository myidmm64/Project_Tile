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
        Debug.Log("��ų ����");
        yield return new WaitForSeconds(1);
        Debug.Log("��ų ��");
        Destroy(gameObject);
    }
}
