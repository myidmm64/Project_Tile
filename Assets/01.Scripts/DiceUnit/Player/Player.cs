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
    [SerializeField]
    private float _hpAniDuration = 0.2f;

    public override void Initialize()
    {
        base.Initialize();
        var modules = GetComponents<PlayerModule>();
        foreach (var module in modules)
        {
            _playerModules.Add(module);
        }
    }

    public override void StartStage(FloorData floorData, StageData stageData)
    {
        base.StartStage(floorData, stageData);
        MainUI.Inst.GetUIElement<CharacterUI>().hpSlider.Initialize(MaxHP);
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

    public override void Damage(int damage, EAttackType attackType, bool isCritical, bool isTrueDamage = false)
    {
        base.Damage(damage, attackType, isCritical, isTrueDamage);
        MainUI.Inst.GetUIElement<CharacterUI>().hpSlider.SetValueWithAnimation(CurHP, _hpAniDuration);
    }
}
