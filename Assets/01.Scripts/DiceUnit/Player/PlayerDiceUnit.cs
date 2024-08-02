using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDiceUnit : DiceUnit
{
    // 추후 모듈 붙여서 모듈마다 이벤트 발급해주는 방식으로 ㄱㄱ
    [SerializeField]
    private int _maxInputQueueCount = 5;
    [SerializeField]
    private float _moveDuration = 0.3f;
    private Queue<Vector2Int> _inputQueue = new Queue<Vector2Int>();
    private Sequence _moveSeq = null;
    private Animator _animator = null;
    private bool _moveable => _moveSeq == null || (_moveSeq != null && !_moveSeq.active);
    private bool _isMoving => _moveSeq != null && _moveSeq.active;

    [SerializeField]
    private float _attackDelay = 1f;
    private float _attackTimer = 0f;

    private void Awake()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
    }

    private void Start()
    {
        TestInit();
    }

    void Update()
    {
        Attack();
        Move();
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

        if (_isMoving) return;
        if (_attackTimer >= _attackDelay)
        {
            if(_diceGrid.diceUnitGrid.ContainsKey(positionKey + Vector2Int.left))
            {
                Debug.Log("Left Attack");
                _attackTimer = 0f;
                _animator.Play("NormalAttack");
            }
            else if (_diceGrid.diceUnitGrid.ContainsKey(positionKey + Vector2Int.right))
            {
                Debug.Log("Right Attack");
                _attackTimer = 0f;
                _animator.Play("Move");
                _animator.Play("NormalAttack");
            }
        }
    }

    public void Move()
    {
        if(_inputQueue.Count > 0 && _moveable) // move중이 아닐 때만 이동
        {
            Vector2Int inputDir = _inputQueue.Dequeue();
            Vector2Int targetPositionKey = positionKey + inputDir;
            if(ChangeMyDice(targetPositionKey))
            {
                _animator.SetFloat("Horizontal", inputDir.x);
                _animator.SetFloat("Vertical", inputDir.y);
                _animator.Play("Move");
                _diceGrid.grid[targetPositionKey].Roll();
                MoveAnimation(); // Seq, Animator 등을 이용한 시각적 이동
            }
        }
    }

    private void MoveAnimation()
    {
        _moveSeq = DOTween.Sequence();
        _moveSeq.Append(transform.DOMove(dice.groundPos, _moveDuration)).SetEase(Ease.Linear);
        _moveSeq.AppendCallback(() =>
        {
            _animator.Play("Idle");
        });
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
