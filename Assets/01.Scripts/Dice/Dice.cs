using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dice : MonoBehaviour, IPoolable
{
    [SerializeField]
    private Transform _groundPosTrm = null;
    public Vector3 groundPos => _groundPosTrm.position;

    public EPoolType PoolType { get; set; }
    public GameObject POOLABLE_GAMEOBJECT { get; set; }

    public Vector2Int positionKey = Vector2Int.zero;

    [SerializeField]
    private DiceAnimationDataSO _diceAnimationDataSO = null;
    [SerializeField]
    private DiceSpriteDataSO _diceSpriteDataSO = null;
    [SerializeField]
    private SpriteRenderer _pipSpriteRenderer = null;
    private List<SpriteRenderer> _sprites = new List<SpriteRenderer>(); // Order 세팅용
    private Animator _animator = null;
    private DiceAction _diceAction = null;

    private EDiceType _eDiceType = EDiceType.None;
    public int dicePip { get; private set; } // 1 ~ 6

    public void Initailize()
    {
        Transform spriteTrm = transform.Find("Sprite");
        _sprites.AddRange(transform.Find("Sprites").GetComponentsInChildren<SpriteRenderer>());
    }

    public void PopObject()
    {
    }

    public void PushObject()
    {
    }

    /// <summary>
    /// 자식 클래스에서 뭔가 기믹으로 인해 바인드 불가능하다면 이 함수를 바꿔주기
    /// </summary>
    /// <returns></returns>
    public virtual bool UnitBindable()
    {
        return true;
    }

    public void SetSpriteOrder()
    {
        if (_sprites.Count == 0) return;
        for(int i = 0; i < _sprites.Count; i++)
        {
            _sprites[i].sortingOrder = 0 - (positionKey.y * 5) + i;
        }
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
        // changePip = 0;
        dicePip = changePip;
        // 임시 코드
        _pipSpriteRenderer.sprite = _diceSpriteDataSO.diceSpriteDatas[changePip].sprite;
        _pipSpriteRenderer.color = _diceSpriteDataSO.diceSpriteDatas[changePip].accentColor;
        // roll Animation
        // _animator.SetInteger("dicePip", _dicePip); -> 해당 pip를 기준으로 애니 끝난 후 스프라이트를 결정
        // _animator.Play("Roll");
    }
}
