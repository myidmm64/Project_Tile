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
    private DiceSpriteDataSO _diceSpriteDataSO = null;
    [SerializeField]
    private SpriteRenderer _pipSpriteRenderer = null;
    private List<SpriteRenderer> _sprites = new List<SpriteRenderer>(); // Order 세팅용
    private Animator _animator = null;
    private bool _isInitialized = false;

    public int dicePip { get; private set; } // 1 ~ 6

    private void Awake() // Pool에서 나오는 것과, 스테이지에 미리 생성되어있는 것의 차별
    {
        Initailize();
    }

    public void Initailize()
    {
        if (_isInitialized) return;
        _sprites.AddRange(transform.Find("Sprites").GetComponentsInChildren<SpriteRenderer>());
        _isInitialized = true;
    }

    public void PopObject()
    {
    }

    public void PushObject()
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

    public void Roll(int specificPip = 0)
    {
        int changePip = specificPip == 0 ? changePip = Random.Range(1, 7) : specificPip;
        dicePip = changePip;
        // 임시 코드
        _pipSpriteRenderer.sprite = _diceSpriteDataSO.diceSpriteDatas[changePip].sprite;
        _pipSpriteRenderer.color = _diceSpriteDataSO.diceSpriteDatas[changePip].accentColor;
    }
}
