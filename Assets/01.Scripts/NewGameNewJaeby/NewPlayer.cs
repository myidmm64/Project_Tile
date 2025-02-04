using System.Collections.Generic;
using UnityEngine;

public class NewPlayer : StageUnit, IDiceUnit, IMovable //, IDamagable
{
    private HashSet<PlayerModule> _playerModules = new HashSet<PlayerModule>();

    public Vector2Int PosKey { get; protected set; }
    public Dice CurDice { get; protected set; }

    // items Ãß°¡

    public override void StartStage()
    {
    }

    public override void EndStage()
    {
    }

    private void Awake()
    {
        var modules = GetComponents<PlayerModule>();
        foreach (var module in modules)
        {
            _playerModules.Add(module);
        }
    }

    public T GetModule<T>() where T : PlayerModule
    {
        foreach (var module in _playerModules)
        {
            if (module is T) return (T)module;
        }
        return null;
    }

    private void Update()
    {
        GetModule<PlayerMoveModule>().Move();
    }

    public bool ChangeDice(Dice targetDice)
    {
        return false;
    }

    public bool Knockback(EDirection dir, int amount)
    {
        return Move(PosKey + (Utility.EDirectionToVector(dir) * amount));
    }

    public bool Move(Vector2Int target)
    {
        if (_stage.Grid.ContainsKey(target))
        {
            transform.position = _stage.Grid[target].transform.position;
            return true;
        }
        return false;
    }

}
