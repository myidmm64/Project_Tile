using UnityEngine;

public class Tile : MonoBehaviour, IPoolable
{
    private ETileStatus _status = ETileStatus.None;

    public EPoolType PoolType { get => EPoolType.Tile; set { } }
    public GameObject POOLABLE_GAMEOBJECT { get; set; }

    public void Initailize()
    {
    }

    public void PopObject()
    {
    }

    public void PushObject()
    {
    }
}
