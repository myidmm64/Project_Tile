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

    public List<Sprite> _sprites = new List<Sprite>();
    public EPoolType PoolType { get; set; }
    public GameObject POOLABLE_GAMEOBJECT { get; set; }

    public Vector2Int positionKey = Vector2Int.zero;

    [SerializeField]
    private DiceAnimationDataSO _diceAnimationDataSO = null;
    //private SpriteRenderer _spriteRenderer = null;
    //private Animator _animator = null;
    private DiceAction _diceAction = null;

    private EDiceType _eDiceType = EDiceType.None;
    public int dicePip { get; private set; } // 1 ~ 6

    public void Initailize()
    {
        Transform spriteTrm = transform.Find("Sprite");
        //_spriteRenderer = spriteTrm.GetComponent<SpriteRenderer>();
        //_animator = spriteTrm.GetComponent<Animator>();
    }

    public void PopObject()
    {
    }

    public void PushObject()
    {
    }

    /// <summary>
    /// �ڽ� Ŭ�������� ���� ������� ���� ���ε� �Ұ����ϴٸ� �� �Լ��� �ٲ��ֱ�
    /// </summary>
    /// <returns></returns>
    public virtual bool UnitBindable()
    {
        return true;
    }

    public void SetSpriteOrder()
    {
        //if (_spriteRenderer == null) return;
        //_spriteRenderer.sortingOrder = 0 - positionKey.y;
    }

    // Dice ��ü
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
            Debug.LogWarning("Ŭ������ ã�� �� �����ϴ�.");
            return;
        }
        _diceAction = Activator.CreateInstance(type) as DiceAction;
    }

    public void Roll(int specificPip = 0)
    {
        int changePip = specificPip == 0 ? changePip = Random.Range(1, 7) : specificPip;
        dicePip = changePip;
        // �ӽ� �ڵ�
        //_spriteRenderer.sprite = _sprites[changePip];
        // roll Animation
        // _animator.SetInteger("dicePip", _dicePip); -> �ش� pip�� �������� �ִ� ���� �� ��������Ʈ�� ����
        // _animator.Play("Roll");
    }
}
