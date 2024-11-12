using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyPattern : MonoBehaviour
{
    public PatternDataSO data = null;
    protected Enemy _enemy = null;
    public bool isEnded = false;
    [SerializeField]
    private EnemyPattern _nextPattern = null;
    public EnemyPattern nextPattern => _nextPattern;

    public void BindEnemy(Enemy enemy)
    {
        _enemy = enemy;
    }

    public virtual void Initialize()
    {

    }

    public abstract bool Startable();
    public abstract void Enter(); // ���Ͽ� ���� ��
    public abstract void PatternUpdate(); // ���� �������� ��
    public abstract void Exit(); // ���Ͽ��� ������ ��
}