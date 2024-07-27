using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDiceUnit : DiceUnit
{
    [SerializeField]
    private int _maxInputQueueCount = 5;
    private Queue<Vector2> _inputQueue = new Queue<Vector2>();
    private bool _moveable = true;

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
        if(_moveable) // move중이 아닐 때만 이동
        {

        }
    }

    public void AddQueue(Vector2 dir)
    {
        // max count 체크
        if(_inputQueue.Count > _maxInputQueueCount)
        {
            _inputQueue.Clear();
        }
        _inputQueue.Enqueue(dir);
    }

    public void UpMove(InputAction.CallbackContext context)
    {
        AddQueue(Vector2.up);
    }

    public void DownMove(InputAction.CallbackContext context)
    {
        AddQueue(Vector2.down);
    }

    public void LeftMove(InputAction.CallbackContext context)
    {
        AddQueue(Vector2.left);
    }

    public void RightMove(InputAction.CallbackContext context)
    {
        AddQueue(Vector2.right);
    }
}
