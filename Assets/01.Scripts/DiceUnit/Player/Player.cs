using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : DiceUnit
{
    private HashSet<PlayerModule> _playerModules = new HashSet<PlayerModule>();
    public PlayerSprite playerSprite => base.sprite as PlayerSprite;
    public bool _isDungeon = true; // 현재 던전에 들어와있는지 체크

    private void Awake()
    {
        var modules = GetComponents<PlayerModule>();
        foreach (var module in modules)
        {
            _playerModules.Add(module);
        }
    }

    private void Start()
    {
        transform.position = dice.groundPos;
        MaxHP = data.maxHP;
        CurHP = data.maxHP;
        MainUI.Inst.GetUIElement<CharacterUI>().hpSlider.Initialize(data.maxHP);
    }

    private void Update()
    {
        sprite.LookAt(grid.FindClosestUnit<DiceUnit>(positionKey)); // 나중에 누굴 쫒아갈지 만들어두기
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

    public override void Damage(int damage)
    {
        CurHP -= damage;
        CurHP = Mathf.Clamp(CurHP, 0, MaxHP);

        PopupText popup = PoolManager.Inst.Pop(EPoolType.PopupText) as PopupText;
        popup.Popup(damage.ToString(), transform.position + Vector3.up * 0.3f);

        MainUI.Inst.GetUIElement<CharacterUI>().hpSlider.SetValueWithAnimation(CurHP, 0.2f);
    }
}
