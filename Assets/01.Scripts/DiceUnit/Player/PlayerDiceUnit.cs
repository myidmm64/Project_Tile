using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerDiceUnit : DiceUnit
{
    // ���� ��� �ٿ��� ��⸶�� �̺�Ʈ �߱����ִ� ������� ����
    public Animator animator { get; private set; }

    public PlayerMoveModule moveModule { get; private set; }
    public PlayerAttackModule attackModule { get; private set; }
    public PlayerSkillModule skillModule { get; private set; }

    public DiceUnit targetDiceUnit { get; private set; } // �� �����Ӹ��� ��� �÷��̾ �ٶ� Ÿ��

    // ���� PC��� �༮���� ������ ���������� ���� 


    private void Awake()
    {
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        animator = spriteRenderer.GetComponent<Animator>();
        moveModule = GetComponent<PlayerMoveModule>();
        attackModule = GetComponent<PlayerAttackModule>();
        skillModule = GetComponent<PlayerSkillModule>();
    }

    private void Start()
    {
        TestInit();
    }

    void Update()
    {
        CheckTarget();
        moveModule.Move();
        SetSpriteSortingOrder();
    }

    private void CheckTarget()
    {
        // ���� ������ ���� Ÿ���� ����
        // 1. ���� ���� �� ��
        // 2. �켱 ���� ��ġ
        // 3. ī���� ������ �Ϸ��� ��
        // 4. �Ÿ��� ����� ��
        // 5. �ִ� ü���� ���� ��
        DiceUnit nearestTarget = null;
        Vector2 min = Vector2.negativeInfinity;
        foreach(var diceUnitPair in diceGrid.diceUnitGrid)
        {
            if (diceUnitPair.Value.Equals(this)) continue; // ���� ���� ����� �Ŵϱ� �̰� ��ŵ

            Vector2 length = diceUnitPair.Key - positionKey;
            if(length.sqrMagnitude < min.sqrMagnitude)
            {
                min = length;
                nearestTarget = diceUnitPair.Value;
            }
        }

        if( nearestTarget != null )
        {
            Vector2 referenceDirection = Vector2.right;
            Vector2 toTarget = nearestTarget.positionKey - positionKey;
            toTarget.Normalize();

            float dotProduct = Vector2.Dot(referenceDirection, toTarget);

            // �¿� �Ǻ�
            if (dotProduct > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (dotProduct < 0)
            {
                spriteRenderer.flipX = true;
            }

        }

    }

    private void TestInit()
    {
        ChangeMyDice(new Vector2Int(1, 1));
        transform.position = dice.groundPos;
    }

    public EDirection GetDirection()
    {
        return spriteRenderer.flipX ? EDirection.Left : EDirection.Right;
    }
}
