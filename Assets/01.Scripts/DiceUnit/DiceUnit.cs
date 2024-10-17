using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class DiceUnit : MonoBehaviour, IDamagable, IMovable
{
    public DiceGrid grid => DiceGrid.Inst;

    [SerializeField]
    private float _moveDuration = 0.12f;
    [SerializeField]
    private DiceUnitData _data = null;
    public DiceUnitData data => _data;
    [SerializeField]
    private DiceUnitSprite _sprite = null;
    public DiceUnitSprite sprite => _sprite;

    public Dice dice { get; private set; }
    public Vector2Int positionKey => dice != null ? dice.positionKey : new Vector2Int(-1, -1);
    public Action<Dice> OnDiceChanged = null;

    private Sequence _moveSeq = null;
    public Action<Vector2Int, Vector2Int> OnMoveStarted = null; // �����̱� ��, �� positionKey ����
    public Action<Vector2Int> OnMoveEnded = null; // ������ �� positionKey ����
    public bool moveable => _moveSeq == null || (_moveSeq != null && !_moveSeq.active);
    public bool isMoving => _moveSeq != null && _moveSeq.active;

    public int CurHP { get; set; }
    public int MaxHP { get; set; }

    public bool ChangeDice(Vector2Int targetPositionKey)
    {
        if (grid.dices.TryGetValue(targetPositionKey, out Dice targetDice))
        {
            return ChangeDice(targetDice);
        }

        return false;
    }

    public bool ChangeDice(Dice targetDice) // targetDice�� �����ϱ�
    {
        // grid �� dice�� �ְ�, unit�� �� �� �־�� ��.
        bool changable = grid.units.ContainsKey(targetDice.positionKey) == false;
        if (changable == false) return false;

        // ���� ��ġ�� DiceUnit ����� �̵�
        if (grid.units.ContainsKey(positionKey))
        {
            grid.units.Remove(positionKey);
        }
        dice = targetDice;

        grid.units[positionKey] = this;
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
