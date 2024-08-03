using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDiceUnit : DiceUnit
{
    // ���� ��� �ٿ��� ��⸶�� �̺�Ʈ �߱����ִ� ������� ����
    public Animator animator { get; private set; }

    public PlayerMoveModule moveModule { get; private set; }
    public PlayerAttackModule attackModule { get; private set; }

    public DiceUnit targetDiceUnit { get; private set; } // �� �����Ӹ��� ��� �÷��̾ �ٶ� Ÿ��

    private void Awake()
    {
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        animator = spriteRenderer.GetComponent<Animator>();
        moveModule = GetComponent<PlayerMoveModule>();
        attackModule = GetComponent<PlayerAttackModule>();
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
        attackModule.Attack();
    }

    private void CheckTarget()
    {
        // ���� ������ ���� Ÿ���� ����
        // 1. ���� ���� �� ��
        // 2. �켱 ���� ��ġ
        // 3. �Ÿ��� ����� ��
        // 4. �ִ� ü���� ���� ��


    }

    private void TestInit()
    {
        ChangeMyDice(new Vector2Int(0, 0));
        transform.position = dice.groundPos;
    }
}
