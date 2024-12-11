using System.Collections.Generic;
using UnityEngine;

public class DicePool : MonoSingleTon<DicePool>
{
    public PoolDataSO poolData { get; private set; }

    private List<EPoolType> _dicePoolTypes = new List<EPoolType>();

    private void Awake()
    {
        poolData = Resources.Load<PoolDataSO>("Poolable/Dice/DicePoolDataSO");
        if (poolData == null)
        {
            Debug.LogError("poolData 존재하지 않음.");
            return;
        }
    }

    private void Start()
    {
        StageManager.Inst.OnChangeFloor += DisposeFloorPool;
    }

    public void DisposeFloorPool(int floor)
    {
        foreach(var poolType in _dicePoolTypes)
        {
            PoolManager.Inst.Dispose(poolType);
        }

        int min = 100 + floor * 10; // 0층 Dice가 100부터 시작하므로
        int max = 100 + min + 99;
        var poolTypes = Utility.GetEnumValuesInRange<EPoolType>(min, max);

        foreach(var poolType in poolTypes)
        {
            foreach(var poolData in poolData.poolDatas)
            {
                if(poolData.ePoolType == poolType)
                {
                    for(int i = 0; i < poolData.generateCount; i++)
                    {
                        PoolManager.Inst.GeneratePoolObj(poolData.obj, poolType);
                    }
                }
            }
        }
    }


}
