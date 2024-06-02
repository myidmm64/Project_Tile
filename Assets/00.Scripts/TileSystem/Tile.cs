using UnityEngine;

public class Tile : MonoBehaviour, IPoolable
{
    public int status = (int)ETileStatus.Moveable;

    public EPoolType PoolType { get => EPoolType.Tile; set { } }
    public GameObject POOLABLE_GAMEOBJECT { get; set; }
    public Vector2Int positionKey;

    public ITileEntity bindedEntity;

    public void Initailize()
    {
    }

    public void PopObject()
    {
    }

    public void PushObject()
    {
    }

    public bool HasStatus(ETileStatus status)
    {
        return (status & status) == status;
    }
}
