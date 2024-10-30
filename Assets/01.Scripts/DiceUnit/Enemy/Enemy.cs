using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Enemy : DiceUnit
{
    [SerializeField]
    protected float _hpAniDuration = 0.2f;

    protected List<EnemyPattern> _patterns = new List<EnemyPattern>();
    private HashSet<EnemyPattern> _cooldownPatterns = new HashSet<EnemyPattern>();
    protected EnemyPattern _currentPattern = null;

    protected abstract void BindPattern();

    protected override void Awake()
    {
        base.Awake();
        MaxHP = data.baseStat.hp;
        CurHP = MaxHP;
        BindPattern();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        PatternCycle();
    }

    public Player GetPlayer()
    {
        foreach (var diceUnit in grid.units.Values)
        {
            if (diceUnit is Player) return diceUnit as Player;
        }
        return null;
    }

    private void PatternCycle()
    {
        if (_currentPattern == null)
        {
            _currentPattern = GetNextPattern();
            if (_currentPattern != null)
            {
                // Debug.Log($"Pattern Enter : {_currentPattern.GetType()}");
                _currentPattern.Enter();
            }
        }
        if (_currentPattern == null)
        {
            // Debug.Log($"GetNextPattern으로 패턴을 찾지 못했음.");
            return;
        }

        _currentPattern.Update();
        if (_currentPattern.isEnded)
        {
            // Debug.Log($"Pattern Exit : {_currentPattern.GetType()}");
            _currentPattern.Exit();
            StartCoroutine(PatternCooldownCoroutine(_currentPattern));
            _currentPattern = null;
        }
    }

    private IEnumerator PatternCooldownCoroutine(EnemyPattern pattern)
    {
        _cooldownPatterns.Add(pattern);
        yield return new WaitForSeconds(pattern.GetCooltime());
        _cooldownPatterns.Remove(pattern);
    }

    private EnemyPattern GetNextPattern()
    {
        List<EnemyPattern> canStartPatterns = null;
        foreach (var pattern in _patterns)
        {
            // 쿨다운중이지 않고, 스타트할 수 있다면
            if (_cooldownPatterns.Contains(pattern) == false && pattern.CanStartPattern())
            {
                if (canStartPatterns == null) canStartPatterns = new List<EnemyPattern>();
                canStartPatterns.Add(pattern);
            }
        }

        // 가장 우선순위가 높은 것
        if (canStartPatterns != null && canStartPatterns.Count > 0)
        {
            int highestPriority = canStartPatterns.Max(p => p.GetPatternPriority());

            var highestPriorityPatterns = canStartPatterns
            .Where(p => p.GetPatternPriority() == highestPriority)
            .ToList();

            int randomIndex = Random.Range(0, highestPriorityPatterns.Count);
            return highestPriorityPatterns[randomIndex];
        }

        return null;
    }
}
