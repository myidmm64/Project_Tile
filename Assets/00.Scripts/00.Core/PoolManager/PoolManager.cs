using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleTon<PoolManager>
{
    private Dictionary<EPoolType, Queue<IPoolable>> _pool = new();
    private Dictionary<EPoolType, GameObject> _poolableResorcesTable = new();

    [SerializeField]
    private int GENERATE_COUNT = 10;

    private void Awake()
    {
        InitPool();
    }

    private void GeneratePoolObj(GameObject obj, EPoolType pooltype)
    {
        GameObject gameObj = Instantiate(obj);
        IPoolable poolable = gameObj.GetComponent<IPoolable>();
        poolable.POOLABLE_GAMEOBJECT = gameObj;

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
        var objs = Resources.LoadAll<GameObject>("Poolable");
        foreach (var obj in objs)
        {
            IPoolable tempPoolable = obj.GetComponent<IPoolable>();
            if (tempPoolable == null)
            {
                Debug.LogError($"Poolable 스크립트가 존재하지 않음.");
                continue;
            }
            _poolableResorcesTable.Add(tempPoolable.PoolType, obj);

            for (int i = 0; i < GENERATE_COUNT; i++)
            {
                GeneratePoolObj(obj, tempPoolable.PoolType);
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
}