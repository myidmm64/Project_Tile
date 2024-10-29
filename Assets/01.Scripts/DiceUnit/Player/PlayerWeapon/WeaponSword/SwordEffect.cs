using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEffect : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;

    public void PlayEffect(int idx)
    {
        _animator.gameObject.SetActive(true);
        _animator.Play($"Effect{idx}");
        _animator.Update(0);
        StartCoroutine(AnimEndDeactive());
    }

    private IEnumerator AnimEndDeactive()
    {
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f);
        _animator.gameObject.SetActive(false);
    }
}
