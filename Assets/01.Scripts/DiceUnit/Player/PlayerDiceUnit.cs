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

    [SerializeField]
    private float _attackDelay = 1f;
    private float _attackTimer = 0f;

    private void Awake()
    {
        animator = transform.Find("Sprite").GetComponent<Animator>();
        moveModule = GetComponent<PlayerMoveModule>();
    }

    private void Start()
    {
        TestInit();
    }

    void Update()
    {
        CheckTarget();
        moveModule.Move();
        Attack();
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

    private void Attack()
    {
        // �����Ÿ� �� ���� �ִٸ� �ڵ�����
        _attackTimer += Time.deltaTime;

        if (moveModule.isMoving) return;
        if (_attackTimer >= _attackDelay)
        {
            if(diceGrid.diceUnitGrid.ContainsKey(positionKey + Vector2Int.left))
            {
                Debug.Log("Left Attack");
                _attackTimer = 0f;
                animator.Play("NormalAttack");
            }
            else if (diceGrid.diceUnitGrid.ContainsKey(positionKey + Vector2Int.right))
            {
                Debug.Log("Right Attack");
                _attackTimer = 0f;
                animator.Play("Move");
                animator.Play("NormalAttack");
            }
        }
    }
}
