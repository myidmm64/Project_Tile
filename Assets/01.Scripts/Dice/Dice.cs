using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour, IPoolable
{
    public EPoolType PoolType { get; set; }
    public GameObject POOLABLE_GAMEOBJECT { get; set; }

    [SerializeField]
    private DiceAnimationDataSO _diceAnimationDataSO = null;
    private SpriteRenderer _spriteRenderer = null;
    private Animator _animator = null;
    private DiceAction _diceAction = null;

    private EDiceType _eDiceType = EDiceType.None;
    private int _dicePip = 0; // 1 ~ 6

    public void Initailize()
    {
        Transform spriteTrm = transform.Find("Sprite");
        _spriteRenderer = spriteTrm.GetComponent<SpriteRenderer>();
        _animator = spriteTrm.GetComponent<Animator>();
    }

    public void PopObject()
    {
    }

    public void PushObject()
    {
    }

    // Dice 교체
    public void ChangeDiceType(EDiceType eDiceType)
    {
        _eDiceType = eDiceType;
        // _animator.runtimeAnimatorController = _diceAnimationDataSO.diceAnimationDatas.Find(x => x.eDiceType == _eDiceType).animator.runtimeAnimatorController;
        // SetDiceAction(_eDiceType);
    }

    private void SetDiceAction(EDiceType eDiceType)
    {
        string diceActionClassName = $"DiceAction{eDiceType.ToString()}";
        Type type = Type.GetType(diceActionClassName);
        if (type == null)
        {
            Debug.LogWarning("클래스를 찾을 수 없습니다.");
            return;
        }
        _diceAction = Activator.CreateInstance(type) as DiceAction;
    }

    public void Roll(int specificPip = 0)
    {
        int changePip = specificPip == 0 ? changePip = Random.Range(1, 7) : specificPip;
        _dicePip = changePip;
        // roll Animation
        // _animator.SetInteger("dicePip", _dicePip); -> 해당 pip를 기준으로 애니 끝난 후 스프라이트를 결정
        // _animator.Play("Roll");
    }
}
