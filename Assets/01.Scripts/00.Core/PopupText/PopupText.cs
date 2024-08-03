using System.Collections;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour, IPoolable
{
    private TextMeshPro _text = null;
    public EPoolType PoolType { get; set; }
    public GameObject POOLABLE_GAMEOBJECT { get; set; }

    public void Initailize()
    {
        _text = GetComponent<TextMeshPro>();
    }

    public void PopObject()
    {
    }

    public void PushObject()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        transform.Translate(Vector3.up * 1f * Time.deltaTime);
    }

    public void Popup(string text, Vector3 position)
    {
        _text.SetText(text);
        transform.position = position;
        StartCoroutine(PopupCoroutine());
    }

    private IEnumerator PopupCoroutine()
    {
        yield return new WaitForSeconds(1f);
        PoolManager.Inst.Push(this);
    }
}
