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

    private void Awake()
    {
        animator = transform.Find("Sprite").GetComponent<Animator>();
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
        attackModule.Attack();
    }

    private void CheckTarget()
    {
        // ���� ������ ���� Ÿ���� ����
    }

    private void TestInit()
    {
        ChangeMyDice(new Vector2Int(0, 0));
        transform.position = dice.groundPos;
    }
}
