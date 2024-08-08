using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveModule : PlayerModule
{
    [SerializeField]
    private int _maxInputQueueCount = 3;
    [SerializeField]
    private float _moveDuration = 0.1f;
    private Queue<Vector2Int> _inputQueue = new Queue<Vector2Int>();
    private Sequence _moveSeq = null;

    public bool moveable => _moveSeq == null || (_moveSeq != null && !_moveSeq.active);
    public bool isMoving => _moveSeq != null && _moveSeq.active;

    public void Move()
    {
        if (_inputQueue.Count > 0 && moveable) // move���� �ƴ� ���� �̵�
        {
            Vector2Int inputDir = _inputQueue.Dequeue();
            Vector2Int targetPositionKey = _player.positionKey + inputDir;
            if (_player.ChangeMyDice(targetPositionKey))
            {
                _player.animator.SetFloat("Horizontal", inputDir.x);
                _player.animator.SetFloat("Vertical", inputDir.y);
                _player.animator.Play("Move");
                MoveAnimation(); // Seq, Animator ���� �̿��� �ð��� �̵�
            }
        }
    }

    private void MoveAnimation()
    {
        _moveSeq = DOTween.Sequence();
        _moveSeq.Append(transform.DOMove(_player.dice.groundPos, _moveDuration)).SetEase(Ease.Linear);
        _moveSeq.AppendCallback(() =>
        {
            _player.animator.Play("Idle");
        });
    }
    public void AddQueue(Vector2Int dir)
    {
        // max count üũ
        if (_inputQueue.Count > _maxInputQueueCount)
        {
            Debug.Log("Input Queue �� ��! clear �����ϰ���.");
            _inputQueue.Clear();
        }
        _inputQueue.Enqueue(dir);
    }

    public void UpMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            AddQueue(Vector2Int.up);
    }

    public void DownMove(InputAction.CallbackContext context)
    {
        if (context.performed)
            AddQueue(Vector2Int.down);
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
