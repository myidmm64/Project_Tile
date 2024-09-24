using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDiceUnit : DiceUnit
{
    private HashSet<PlayerModule> _playerModules = new HashSet<PlayerModule>();
    public bool _isDungeon = true; // 현재 던전에 들어와있는지 체크

    public SpriteRenderer spriteRenderer { get; private set; }
    public Animator animator { get; private set; }

    private void Awake()
    {
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        animator = spriteRenderer.GetComponent<Animator>();

        var modules = GetComponents<PlayerModule>();
        foreach (var module in modules)
        {
            _playerModules.Add(module);
        }
    }

    private void Start()
    {
        TestInit();
    }

    private void Update()
    {
        GetModule<PlayerMoveModule>().Move();
    }

    public T GetModule<T>() where T : PlayerModule
    {
        foreach(var module in _playerModules)
        {
            if(module is T) return (T)module;
        }
        return null;
    }

    private void TestInit()
    {
        var grid = GameObject.FindAnyObjectByType<DiceGrid>();
        SetDiceGrid(grid);

        ChangeDice(new Vector2Int(1, 1));
        transform.position = dice.groundPos;

        MaxHP = data.maxHP;
        CurHP = data.maxHP;
        MainUI.Inst.GetUIElement<CharacterUI>().hpSlider.Initialize(data.maxHP);
    }

    public EDirection GetDirection()
    {
        return EDirection.Left;
        // return spriteRenderer.flipX ? EDirection.Left : EDirection.Right;
    }

    public override void Damage(int damage)
    {
        CurHP -= damage;
        CurHP = Mathf.Clamp(CurHP, 0, MaxHP);

        PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
        popup.Popup(damage.ToString(), transform.position + Vector3.up * 0.3f);

        MainUI.Inst.GetUIElement<CharacterUI>().hpSlider.SetValueWithAnimation(CurHP, 0.2f);
    }
}
