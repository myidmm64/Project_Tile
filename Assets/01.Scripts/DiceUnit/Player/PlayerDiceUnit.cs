using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDiceUnit : DiceUnit
{
    // 추후 모듈 붙여서 모듈마다 이벤트 발급해주는 방식으로 ㄱㄱ
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
        // 여러 로직을 통해 타겟을 설정
    }

    private void TestInit()
    {
        ChangeMyDice(new Vector2Int(0, 0));
        transform.position = dice.groundPos;
    }

    private void Attack()
    {
        // 사정거리 내 적이 있다면 자동공격
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
