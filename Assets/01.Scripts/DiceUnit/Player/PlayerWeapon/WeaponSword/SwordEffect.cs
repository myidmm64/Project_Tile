using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEffect : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = transform.Find("Sprite").GetComponent<Animator>();
    }

    public void PlayEffect(int idx)
    {
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
