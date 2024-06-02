using UnityEngine;

public interface ITileEntity
{
    public Vector2Int PositionKey { get; set; }
    public void BindedObject(Tile tile);
    public void UnbindedObject(Tile tile);
}
