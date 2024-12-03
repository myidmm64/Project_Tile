using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/PoolData")]
public class PoolDataSO : ScriptableObject
{
    public int GENERATE_COUNT = 10;
    public List<PoolData> poolDatas = new List<PoolData>();
    [SerializeField]
    private string syncPath = "Poolable/";
    private HashSet<int> _hashcodes = new(); 

    [ContextMenu("PoolData ��ũ")]
    public void SyncPoolData()
    {
        var objs = Resources.LoadAll<GameObject>(syncPath);
        foreach (var obj in objs)
        {
            if (obj.GetComponent<IPoolable>() == null)
            {
                Debug.LogError($"Poolable ��ũ��Ʈ�� �������� ����.");
                continue;
            }

            int hashCode = obj.GetHashCode();
            if (_hashcodes.Contains(hashCode) == false) // �������� ���� ���� �߰�
            {
                PoolData poolData = new PoolData(EPoolType.None, obj, GENERATE_COUNT);
                poolData.name = obj.name;
                _hashcodes.Add(hashCode);
                poolDatas.Add(poolData);
            }
        }
    }

    [ContextMenu("�ؽü� �ʱ�ȭ")]
    public void ResetHashSet()
    {
        _hashcodes.Clear();
        poolDatas.Clear();
    }
}

[System.Serializable]
public class PoolData
{
    [HideInInspector]
    public string name = "";

    public EPoolType ePoolType = EPoolType.None;
    public GameObject obj = null;
    public int generateCount;

    public PoolData(EPoolType _ePoolType, GameObject _obj, int _generateCount)
    {
        ePoolType = _ePoolType;
        obj = _obj;
        generateCount = _generateCount;
    }
}
