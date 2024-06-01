using System;
using UnityEngine;

public class GameObjectLoader : MonoBehaviour
{
    [SerializeField]
    private GameObjectLoaderDataSO _data = null;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if(_data == null)
        {
            Debug.LogError("Data가 존재하지 않습니다.");
            return;
        }

        GameObject obj = new GameObject(_data.GameObjectName);
        ColorHierarchy ch = obj.AddComponent<ColorHierarchy>();
        ch.backColor = _data.BackColor;
        ch.fontColor = _data.FontColor;
        ch.prefix = _data.Prefix;
        GameObject[] resources = Resources.LoadAll<GameObject>(_data.ResourcePath);
        foreach (var resource in resources)
        {
            GameObject inst = Instantiate(resource);
            inst.transform.SetParent(obj.transform);
        }
    }
}
