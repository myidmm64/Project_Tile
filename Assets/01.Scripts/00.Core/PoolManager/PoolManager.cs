using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleTon<PoolManager>
{
    private Dictionary<EPoolType, Queue<IPoolable>> _pool = new();
    private Dictionary<EPoolType, GameObject> _poolableResorcesTable = new();
    public PoolDataSO poolData { get; private set;}

    private void Awake()
    {
        InitPool();
    }

    public void GeneratePoolObj(GameObject obj, EPoolType pooltype)
    {
        GameObject gameObj = Instantiate(obj);
        IPoolable poolable = gameObj.GetComponent<IPoolable>();
        poolable.POOLABLE_GAMEOBJECT = gameObj;
        poolable.PoolType = pooltype;

        if (_pool.ContainsKey(poolable.PoolType) == false)
        {
            _pool.Add(poolable.PoolType, new Queue<IPoolable>());
        }
        poolable.Initailize();
        gameObj.SetActive(false);
        gameObj.transform.SetParent(gameObject.transform);

        _pool[poolable.PoolType].Enqueue(poolable);
    }

    private void InitPool()
    {
        poolData = Resources.Load<PoolDataSO>("Poolable/Normal/PoolDataSO");
        if (poolData == null)
        {
            Debug.LogError("poolData 존재하지 않음.");
            return;
        }

        foreach (var poolData in poolData.poolDatas)
        {
            _poolableResorcesTable.Add(poolData.ePoolType, poolData.obj);

            for (int i = 0; i < poolData.generateCount; i++)
            {
                GeneratePoolObj(poolData.obj, poolData.ePoolType);
            }
        }
    }

    public T Pop<T>(EPoolType poolType) where T : IPoolable
    {
        if (_pool.ContainsKey(poolType) == false)
        {
            Debug.LogError($"{poolType} 이 _pool 내 존재하지 않음.");
            return default(T);
        }
        else if (_pool[poolType].Count == 0)
        {
            GeneratePoolObj(_poolableResorcesTable[poolType], poolType);
        }

        IPoolable result = _pool[poolType].Dequeue();
        result.POOLABLE_GAMEOBJECT.SetActive(true);
        result.POOLABLE_GAMEOBJECT.transform.SetParent(null);
        result.PopObject();
        return (T)result;
    }

    public IPoolable Pop(EPoolType poolType)
    {
        return Pop<IPoolable>(poolType);
    }

    public T PopWithComponent<T>(EPoolType poolType)
    {
        return Pop<IPoolable>(poolType).POOLABLE_GAMEOBJECT.GetComponent<T>();
    }

    public void Push(IPoolable poolable)
    {
        poolable.PushObject();
        poolable.POOLABLE_GAMEOBJECT.SetActive(false);
        poolable.POOLABLE_GAMEOBJECT.transform.SetParent(gameObject.transform);
        _pool[poolable.PoolType].Enqueue(poolable);
    }

    public void Dispose(EPoolType poolType)
    {
        if(_pool.ContainsKey(poolType))
        {
            foreach(var pool in _pool[poolType])
            {
                Destroy(pool.POOLABLE_GAMEOBJECT);
            }
            _pool.Remove(poolType);
        }
    }
}