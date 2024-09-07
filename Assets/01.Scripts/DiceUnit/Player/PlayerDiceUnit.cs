using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerDiceUnit : DiceUnit, IDamagable
{
    // 추후 모듈 붙여서 모듈마다 이벤트 발급해주는 방식으로 ㄱㄱ
    public PlayerMoveModule moveModule { get; private set; }
    public PlayerAttackModule attackModule { get; private set; }
    public PlayerSkillModule skillModule { get; private set; }

    private int _curHP = 0;
    [SerializeField]
    private int _maxHP = 0;
    public int CurHP { get => _curHP; set => _curHP = value; }
    public int MaxHP { get => _maxHP; set => _maxHP = value; }

    protected override void Awake()
    {
        base.Awake();
        moveModule = GetComponent<PlayerMoveModule>();
        attackModule = GetComponent<PlayerAttackModule>();
        skillModule = GetComponent<PlayerSkillModule>();
    }

    private void Start()
    {
        TestInit();
    }

    protected override void Update()
    {
        base.Update();
        moveModule.Move();
        SetSpriteSortingOrder();
    }

    private void TestInit()
    {
        ChangeMyDice(new Vector2Int(1, 1));
        transform.position = dice.groundPos;

        _curHP = _maxHP;
        MainUI.Inst.GetUIElement<CharacterUI>().hpSlider.Initialize(_maxHP);
    }

    public EDirection GetDirection()
    {
        return spriteRenderer.flipX ? EDirection.Left : EDirection.Right;
    }

    public void Damage(int damage)
    {
        _curHP -= damage;
        _curHP = Mathf.Clamp(_curHP, 0, _maxHP);

        PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
        popup.Popup(damage.ToString(), transform.position + Vector3.up * 0.3f);

        SetHPBar();
    }

    private void SetHPBar()
    {
        MainUI.Inst.GetUIElement<CharacterUI>().hpSlider.SetValueWithAnimation(_curHP, 0.2f);
    }
}
