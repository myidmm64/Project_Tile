using UnityEngine;

public interface IPoolable
{
    public EPoolType PoolType { get; set; }
    public GameObject POOLABLE_GAMEOBJECT { get; set; }

    public void Initailize();
    public void PopObject();
    public void PushObject();
}