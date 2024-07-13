using UnityEngine;

public abstract class Dice : MonoBehaviour, IPoolable
{
    public Vector2Int positionKey;
    public int pip = 0;

    public EPoolType PoolType { get => EPoolType.Dice_Normal; set { } }
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

