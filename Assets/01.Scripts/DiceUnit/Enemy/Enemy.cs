using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class Enemy : DiceUnit
{
    public bool isboss = false;

    [SerializeField]
    private TextMeshPro _hpText = null; // 보스 아니라면 체력 여기에
    [SerializeField]
    protected float _hpAniDuration = 0.2f;

    private List<EnemyPattern> _patterns = new List<EnemyPattern>();
    private HashSet<EnemyPattern> _cooldownPatterns = new HashSet<EnemyPattern>();
    protected EnemyPattern _currentPattern = null;

    public override void Initialize()
    {
        base.Initialize();
        _patterns.AddRange(transform.Find("Patterns").GetComponents<EnemyPattern>());
        _patterns.AddRange(transform.Find("Patterns").GetComponentsInChildren<EnemyPattern>());
        foreach(var pattern in _patterns)
        {
            pattern.BindEnemy(this);
            pattern.Initialize();
        }
    }

    protected virtual void Update()
    {
        if (_patterns.Count == 0) return;
        PatternCycle();
    }

    private void PatternCycle()
    {
        if (_currentPattern == null)
        {
            _currentPattern = GetNextPattern();
            if (_currentPattern != null)
            {
                Debug.Log($"Pattern Enter : {_currentPattern.GetType()}");
                _currentPattern.isEnded = false;
                _currentPattern.Enter();
            }
        }
        if (_currentPattern == null)
        {
            // Debug.Log($"GetNextPattern으로 패턴을 찾지 못했음.");
            return;
        }

        _currentPattern.PatternUpdate();
        if (_currentPattern.isEnded)
        {
            Debug.Log($"Pattern Exit : {_currentPattern.GetType()}");
            EnemyPattern nextPattern = _currentPattern.nextPattern; // currentPattern 지우기 전 저장

            _currentPattern.Exit();
            StartCoroutine(PatternCooldownCoroutine(_currentPattern));
            _currentPattern = null;

            if (nextPattern != null)
            {
                // nextPattern이 있다면 강제 시작
                Debug.Log($"Next Pattern Enter : {_currentPattern.GetType()}");
                _currentPattern = nextPattern;
                _currentPattern.isEnded = false;
                _currentPattern.Enter();
            }
        }
    }

    private IEnumerator PatternCooldownCoroutine(EnemyPattern pattern)
    {
        _cooldownPatterns.Add(pattern);
        yield return new WaitForSeconds(pattern.data.cooltime);
        _cooldownPatterns.Remove(pattern);
    }

    private EnemyPattern GetNextPattern()
    {
        List<EnemyPattern> canStartPatterns = null;
        foreach (var pattern in _patterns)
        {
            // 쿨다운중이지 않고, 스타트할 수 있다면
            if (_cooldownPatterns.Contains(pattern) == false && pattern.Startable())
            {
                if (canStartPatterns == null) canStartPatterns = new List<EnemyPattern>();
                canStartPatterns.Add(pattern);
            }
        }

        // 가장 우선순위가 높은 것
        if (canStartPatterns != null && canStartPatterns.Count > 0)
        {
            int highestPriority = canStartPatterns.Max(p => p.data.priority);

            var highestPriorityPatterns = canStartPatterns
            .Where(p => p.data.priority == highestPriority)
            .ToList();

            int randomIndex = Random.Range(0, highestPriorityPatterns.Count);
            return highestPriorityPatterns[randomIndex];
        }

        return null;
    }
}
