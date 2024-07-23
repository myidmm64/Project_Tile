using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/PoolData")]
public class PoolDataSO : ScriptableObject
{
    public int GENERATE_COUNT = 10;
    public List<PoolData> poolDatas = new List<PoolData>();

    [ContextMenu("PoolData ��ũ")]
    public void SyncPoolData()
    {
        var objs = Resources.LoadAll<GameObject>("Poolable");
        foreach (var obj in objs)
        {
            IPoolable tempPoolable = obj.GetComponent<IPoolable>();
            if (tempPoolable == null)
            {
                Debug.LogError($"Poolable ��ũ��Ʈ�� �������� ����.");
                continue;
            }
            int hashCode = tempPoolable.GetHashCode();

            PoolData poolData = poolDatas.Find(x => x.hashCode == hashCode);
            if(poolData != null)
            {
                Debug.Log($"�̹� �����ϴ� poolData�Դϴ�. name : {poolData.name}, poolType : {poolData.ePoolType}");
                continue;
            }
            else
            {
                poolData = new PoolData(EPoolType.None, obj, GENERATE_COUNT);
                poolData.name = obj.name;
                poolData.hashCode = hashCode;

                Debug.Log($"���� poolData�� �����մϴ�. name : {poolData.name}");
                poolDatas.Add(poolData);
            }
        }
    }
}

[System.Serializable]
public class PoolData
{
    [HideInInspector]
    public string name = "";
    [HideInInspector]
    public int hashCode = 0;

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
