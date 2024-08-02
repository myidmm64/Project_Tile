using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDiceUnit : DiceUnit
{
    // ���� ��� �ٿ��� ��⸶�� �̺�Ʈ �߱����ִ� ������� ����
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
        // �����Ÿ� �� ���� �ִٸ� �ڵ�����
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
        if(_inputQueue.Count > 0 && _moveable) // move���� �ƴ� ���� �̵�
        {
            Vector2Int inputDir = _inputQueue.Dequeue();
            Vector2Int targetPositionKey = positionKey + inputDir;
            if(ChangeMyDice(targetPositionKey))
            {
                _animator.SetFloat("Horizontal", inputDir.x);
                _animator.SetFloat("Vertical", inputDir.y);
                _animator.Play("Move");
                _diceGrid.grid[targetPositionKey].Roll();
                MoveAnimation(); // Seq, Animator ���� �̿��� �ð��� �̵�
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
        // max count üũ
        if(_inputQueue.Count > _maxInputQueueCount)
        {
            Debug.Log("Input Queue �� ��! clear �����ϰ���.");
            _inputQueue.Clear();
        }
        _inputQueue.Enqueue(dir);
    }

    public void UpMove(InputAction.CallbackContext context)
    {
        if(context.performed)
            AddQueue(Vector2Int.down); // positionKey�� ���� �ݴ���
    }

    public void DownMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            AddQueue(Vector2Int.up); // positionKey�� ���� �ݴ���
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
