using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDiceUnit : DiceUnit
{
    [SerializeField]
    private int _maxInputQueueCount = 5;
    [SerializeField]
    private float _moveDuration = 0.3f;
    private Queue<Vector2Int> _inputQueue = new Queue<Vector2Int>();
    private Sequence _moveSeq = null;
    private bool _moveable => _moveSeq == null || (_moveSeq != null && !_moveSeq.active);

    private void Start()
    {
        TestInit();
    }

    void Update()
    {
        Move();
    }

    private void TestInit()
    {
        dice = _diceGrid.GetDice(Vector2Int.zero);
        transform.position = dice.transform.position;
    }

    public void Move()
    {
        if(_inputQueue.Count > 0 && _moveable) // move중이 아닐 때만 이동
        {
            Vector2Int inputDir = _inputQueue.Dequeue();
            Vector2Int targetPositionKey = positionKey + inputDir;
            if(_diceGrid.TryGetDice(targetPositionKey, out Dice targetDice))
            {
                dice = targetDice;
                MoveAnimation(); // Seq, Animator 등을 이용한 시각적 이동
            }
        }
    }

    private void MoveAnimation()
    {
        _moveSeq = DOTween.Sequence();
        _moveSeq.Append(transform.DOMove(dice.transform.position, _moveDuration)).SetEase(Ease.Linear);
    }

    public void AddQueue(Vector2Int dir)
    {
        // max count 체크
        if(_inputQueue.Count > _maxInputQueueCount)
        {
            Debug.Log("Input Queue 꽉 참! clear 진행하겠음.");
            _inputQueue.Clear();
        }
        _inputQueue.Enqueue(dir);
    }

    public void UpMove(InputAction.CallbackContext context)
    {
        if(context.performed)
            AddQueue(Vector2Int.down); // positionKey는 상하 반대임
    }

    public void DownMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            AddQueue(Vector2Int.up); // positionKey는 상하 반대임
    }

    public void LeftMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            AddQueue(Vector2Int.left);
    }

    public void RightMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            AddQueue(Vector2Int.right);
    }
}
