using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class DiceUnit : MonoBehaviour, IDamagable, IMovable
{
    public DiceGrid diceGrid { get; private set; }

    [SerializeField]
    private float _moveDuration = 0.12f;
    [SerializeField]
    private TextMeshPro _dicePipText = null; // 현재 DiceUnit의 눈금 텍스트
    [SerializeField]
    public DiceUnitData data = null;

    public Dice dice { get; private set; }
    public Vector2Int positionKey => dice != null ? dice.positionKey : new Vector2Int(-1, -1);
    public Action<Dice> OnDiceChanged = null;

    private Sequence _moveSeq = null;
    public Action<Vector2Int, Vector2Int> OnMoveStarted = null; // 움직이기 전, 후 positionKey 전달
    public Action<Vector2Int> OnMoveEnded = null; // 움직인 후 positionKey 전달
    public bool moveable => _moveSeq == null || (_moveSeq != null && !_moveSeq.active);
    public bool isMoving => _moveSeq != null && _moveSeq.active;

    public int CurHP { get; set; }
    public int MaxHP { get; set; }

    public void SetDiceGrid(DiceGrid diceGrid, Dice _dice = null)
    {
        this.diceGrid = diceGrid;
        if (_dice != null) dice = _dice;
    }

    public bool ChangeDice(Vector2Int targetPositionKey)
    {
        if (diceGrid == null) return false;

        if (diceGrid.grid.TryGetValue(targetPositionKey, out Dice targetDice))
        {
            return ChangeDice(targetDice);
        }

        return false;
    }

    public bool ChangeDice(Dice targetDice) // targetDice로 변경하기
    {
        // grid 내 dice가 있고, unit이 들어갈 수 있어야 함.
        bool changable = diceGrid.diceUnitGrid.ContainsKey(targetDice.positionKey) == false
            && targetDice.UnitBindable();
        if (changable == false) return false;

        // 본인 위치의 DiceUnit 지우고 이동
        if (diceGrid.diceUnitGrid.ContainsKey(positionKey))
        {
            diceGrid.diceUnitGrid.Remove(positionKey);
        }
        dice = targetDice;

        _dicePipText?.SetText(dice.dicePip.ToString());
        diceGrid.diceUnitGrid[positionKey] = this;
        OnDiceChanged?.Invoke(dice);
        return true;
    }

    public void SetSpriteSortingOrder(SpriteRenderer renderer)
    {
        renderer.sortingOrder = 0 - positionKey.y;
    }

    public abstract void Damage(int damage);
    public virtual bool Move(Vector2Int target)
    {
        Vector2Int currentPos = positionKey;
        if(moveable && ChangeDice(target))
        {
            OnMoveStarted?.Invoke(currentPos, target);
            _moveSeq = DOTween.Sequence();
            _moveSeq.Append(transform.DOMove(dice.groundPos, _moveDuration)).SetEase(Ease.Linear);
            _moveSeq.AppendCallback(()=> OnMoveEnded?.Invoke(target));
            return true;
        }
        return false;
    }
    public virtual bool Knockback(EDirection dir, int amount)
    {
        Vector2Int target = positionKey + (Utility.EDirectionToVector(dir) * amount);
        return ChangeDice(target);
    }   
}
