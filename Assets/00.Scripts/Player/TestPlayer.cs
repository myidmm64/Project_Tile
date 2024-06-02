using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour, ITileEntity
{
    public Vector2Int PositionKey { get; set; }

    public void BindedObject(Tile tile)
    {
    }

    public void UnbindedObject(Tile tile)
    {
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
        if(TileManager.Inst.TryGetTile(targetPositionKey, out var tile))
        {
            if(tile.HasStatus(ETileStatus.Moveable))
            {
                PositionKey = targetPositionKey;
                transform.position = tile.transform.position;
                tile.bindedEntity = this;
            }
        }
    }
}
