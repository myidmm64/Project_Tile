using UnityEngine;

public static class TransformExtensions
{
    public static T FindComponentInChildren<T>(this Transform trm, string childPath) where T : Component
    {
        Transform child = trm.Find(childPath);
        if (child == null)
        {
            Debug.LogError($"{childPath} ���� Transform �������� ������. Ȯ�� ��Ź�帳�ϴ�.");
            return null;
        }

        return child.GetComponent<T>();
    }
}
