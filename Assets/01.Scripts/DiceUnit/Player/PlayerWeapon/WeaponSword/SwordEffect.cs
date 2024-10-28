using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEffect : MonoBehaviour
{
    private Animator _animator;
    [SerializeField]
    private List<Vector2> _addPos = new List<Vector2>();
    [SerializeField]
    private List<float> _addRot = new List<float>();

    private void Awake()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
    }

    public void PlayEffect(Vector2 spawnPos, int idx)
    {
        transform.position = spawnPos + _addPos[idx];
        transform.rotation = Quaternion.Euler(0, 0, _addRot[idx]);

        _animator.Play($"Effect{idx}");
        _animator.Update(0);
        StartCoroutine(AnimEndDestroy());
    }

    private IEnumerator AnimEndDestroy()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f);
        Destroy(gameObject);
    }
}
