using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T inst = null;
    public static T Inst
    {
        get
        {
            if (inst == null)
            {
                inst = FindAnyObjectByType<T>();
                if (inst == null)
                {
                    Debug.LogWarning($"{typeof(T)} instance ¾øÀ½.");
                }
            }
            return inst;
        }
    }
}