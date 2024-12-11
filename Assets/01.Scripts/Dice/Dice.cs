using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Pool;

public abstract class Dice : MonoBehaviour, IPoolable
{
    [SerializeField]
    private Transform _groundPosTrm = null; // DiceUnit이 올라갈 자리
    public Vector3 groundPos => _groundPosTrm.position;

    public EPoolType PoolType { get; set; }
    public GameObject POOLABLE_GAMEOBJECT { get; set; }

    public Vector2Int positionKey = Vector2Int.zero; // 현재 Dice가 어딨는지

    [SerializeField]
    private DiceSpriteDataSO _diceSpriteDataSO = null;
    [SerializeField]
    private SpriteRenderer _pipSpriteRenderer = null;
    private List<SpriteRenderer> _sprites = new List<SpriteRenderer>(); // Order 세팅용
    private Animator _animator = null;

    public int dicePip { get; private set; } // 1 ~ 6

    public virtual void Initailize()
    {
        _sprites.AddRange(transform.Find("Sprites").GetComponentsInChildren<SpriteRenderer>());
    }

    public virtual void PopObject()
    {
    }

    public virtual void PushObject()
    {
    }

    public void SetSpriteOrder()
    {
        if (_sprites.Count == 0) return;
        for(int i = 0; i < _sprites.Count; i++)
        {
            _sprites[i].sortingOrder = 0 - (positionKey.y * 5) + i;
        }
    }

    public virtual void EnterDice(DiceUnit unit)
    {

    }

    public virtual void ExitDice(DiceUnit unit)
    {

    }

    public virtual void AttackDice(DiceUnit unit)
    {

    }

    public virtual void SetPip(int pip, bool isAnimation = true)
    {
        dicePip = pip;
    }

    public virtual void ChangeType(DiceType type)
    {

    }
}

public enum DiceType
{
    Normal,

}
