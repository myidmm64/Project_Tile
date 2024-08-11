using UnityEngine;

public static class TransformExtensions
{
    public static T FindComponentInChildren<T>(this Transform trm, string childPath) where T : Component
    {
        Transform child = trm.Find(childPath);
        if (child == null)
        {
            Debug.LogError($"{childPath} 에서 Transform 가져오지 못했음. 확인 부탁드립니다.");
            return null;
        }

        return child.GetComponent<T>();
    }
}
