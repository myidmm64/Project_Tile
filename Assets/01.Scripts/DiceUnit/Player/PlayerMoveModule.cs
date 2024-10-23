using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveModule : PlayerModule
{
    [SerializeField]
    private int _maxInputQueueCount = 3; // Input Queue�� �� �� �ִ� Input�� ����
    private Queue<Vector2Int> _inputQueue = new Queue<Vector2Int>();

    private void Start()
    {
        _player.OnMoveStarted += (currentPos, targetPos) =>
        {
            _player.sprite.lockFlip = true;
            Vector2Int inputDir = targetPos - currentPos;
            _player.playerSprite.MoveAnimation(inputDir.x, inputDir.y);
        };
        _player.OnMoveEnded += (targetPos) =>
        {
            _player.sprite.lockFlip = false;
            _player.playerSprite.IdleAnimation();
        };
    }

    public void Move()
    {
        if (_inputQueue.Count > 0 && _player.moveable) // move���� �ƴ� ���� �̵�
        {
            Vector2Int inputDir = _inputQueue.Dequeue();
            Vector2Int targetPositionKey = _player.positionKey + inputDir;
            _player.Move(targetPositionKey);
        }
    }

    public void ResetInputQueue()
    {
        _inputQueue.Clear();
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
