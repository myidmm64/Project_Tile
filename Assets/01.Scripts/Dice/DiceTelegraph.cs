using System;
using System.Collections;
using UnityEngine;

public class DiceTelegraph : MonoBehaviour, IPoolable
{
    public EPoolType PoolType { get; set; }
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

    // 단순하게 n초 뒤 피해가 들어오도록
    public void StartTelepgraph(DiceGrid grid, Vector2Int positionKey, float waitTime, Action Callback)
    {
        transform.position = grid.dices[positionKey].transform.position;

        StartCoroutine(WaitAndCallback(waitTime, Callback));
    }

    private IEnumerator WaitAndCallback(float waitTime, Action Callback)
    {
        yield return new WaitForSeconds(waitTime);
        Callback?.Invoke();
        PoolManager.Inst.Push(this);
    }
}
