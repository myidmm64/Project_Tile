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
    [HideInInspector]
    public StatData stat;
    [SerializeField]
    private DiceUnitSprite _sprite = null;
    public DiceUnitSprite sprite => _sprite;

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

    protected virtual void Awake()
    {
        stat = new StatData(data.baseStat); // 초기 스탯
        MaxHP = data.baseStat.hp;
        CurHP = MaxHP;
    }

    public bool ChangeDice(Vector2Int targetPositionKey)
    {
        if (grid.dices.TryGetValue(targetPositionKey, out Dice targetDice))
        {
            return ChangeDice(targetDice);
        }

        return false;
    }

    public bool ChangeDice(Dice targetDice) // targetDice로 변경하기
    {
        // grid 내 dice가 있고, unit이 들어갈 수 있어야 함.
        bool changable = grid.units.ContainsKey(targetDice.positionKey) == false;
        if (changable == false) return false;

        // 본인 위치의 DiceUnit 지우고 이동
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

    protected virtual int CalculateAttackDamage(EAttackType attackType, float percentDamage, out bool isCritical)
    {
        isCritical = UnityEngine.Random.Range(0, 100) < stat.criticalChance;

        int baseDamage = 0;
        switch (attackType)
        {
            case EAttackType.None:
                break;
            case EAttackType.Physical:
                baseDamage = stat.physicalDamage;
                break;
            case EAttackType.Magical:
                baseDamage = stat.magicalDamage;
                break;
            default:
                break;
        }
        float damage = baseDamage 
            * (percentDamage / 100f)
            * (isCritical ? stat.criticalDamage / 100f : 1f)
            * (GetDamageMultiplierByDicePip() / 100f);
        return (int)damage;
    }

    protected virtual int CalculateTakeDamage(int damage, EAttackType attackType, bool isCritical, bool isTrueDamage = false)
    {
        if (isTrueDamage) return damage;

        float calculatedDamage = (damage * (stat.takeDamage / 100)) - stat.strength;
        return (int)calculatedDamage;
    }

    public virtual void Damage(int damage, EAttackType attackType, bool isCritical, bool isTrueDamage = false)
    {
        int calculatedDamage = CalculateTakeDamage(damage, attackType, isCritical, isTrueDamage);
        CurHP -= calculatedDamage;
        CurHP = Mathf.Clamp(CurHP, 0, MaxHP);

        PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
        popup.Popup(calculatedDamage.ToString(), transform.position + Vector3.up * 0.3f); 
        // 위 팝업에 attackType, damageType 넣어서 이쁘게 해보기
    }

    public virtual void Attack(DiceUnit target, EAttackType attackType, float percentDamage, out bool isCritical, bool isTrueDamage = false)
    {
        target.Damage(CalculateAttackDamage(attackType, percentDamage, out isCritical), attackType, isCritical, isTrueDamage);
    }

    public float GetDamageMultiplierByDicePip() // 주사위 값에 따른 최종 데미지 배율 계산
    {
        if (dice == null) return 100; // 없다면 그냥 100% 데미지
        switch(dice.dicePip)
        {
            case -1: return -30;
            case 0: return 0;
            case 1: return 30;
            case 2: return 45;
            case 3: return 65;
            case 4: return 90;
            case 5: return 120;
            case 6: return 150;
            case > 6: return 150 + ((dice.dicePip - 6) * 10);
                
            default: return 100;
        }
    }
}

public enum EAttackType
{
    None,
    Physical,
    Magical
}