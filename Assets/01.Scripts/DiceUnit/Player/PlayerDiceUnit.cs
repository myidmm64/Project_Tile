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
    public PlayerAttackModule attackModule { get; private set; }

    public DiceUnit targetDiceUnit { get; private set; } // 매 프레임마다 계산 플레이어가 바라볼 타겟

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
        // 여러 로직을 통해 타겟을 설정
        // 1. 공격 범위 내 적
        // 2. 우선 순서 수치
        // 3. 거리가 가까운 적
        // 4. 최대 체력이 높은 적


    }

    private void TestInit()
    {
        ChangeMyDice(new Vector2Int(0, 0));
        transform.position = dice.groundPos;
    }
}
