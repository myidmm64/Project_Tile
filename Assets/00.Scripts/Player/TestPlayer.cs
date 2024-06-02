using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestPlayer : MonoBehaviour, ITileEntity
{
    [SerializeField]
    private float _moveDuration = 0.2f;
    private Sequence _moveSeq = null;
    private bool _moveable = true;

    public Vector2Int PositionKey { get; set; }
    private Animator _animator = null;

    public void BindedObject(Tile tile)
    {
    }

    public void UnbindedObject(Tile tile)
    {
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector2Int positionKey = PositionKey;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            positionKey += Vector2Int.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            positionKey += Vector2Int.right;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            positionKey += Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            positionKey += Vector2Int.down;
        }
        Move(positionKey);
    }

    public void Move(Vector2Int targetPositionKey)
    {
        if (targetPositionKey == PositionKey) return;
        if (_moveable == false) return;
        if (TileManager.Inst.TryGetTile(targetPositionKey, out var tile))
        {
            if (tile.HasStatus(ETileStatus.Moveable))
            {
                MoveAnimation(tile, targetPositionKey - PositionKey);
                PositionKey = targetPositionKey;
                tile.bindedEntity = this;
            }
        }
    }

    private void MoveAnimation(Tile tile, Vector2Int dir)
    {
        if (dir.sqrMagnitude != 0)
        {
            if (dir.x < 0)
            {
                _animator.Play("MoveLeft");
            }
            else if (dir.x > 0)
            {
                _animator.Play("MoveRight");
            }
        }
        _moveable = false;
        _moveSeq = DOTween.Sequence();
        _moveSeq.Append(transform.DOMove(tile.transform.position, _moveDuration));
        _moveSeq.AppendCallback(() =>
        {
            _animator.Play("Idle");
            _moveable = true;
        });
    }
}
