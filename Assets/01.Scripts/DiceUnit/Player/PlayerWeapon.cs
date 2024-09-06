using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField]
    private Animator _animator = null;
    private int _attack = 0;

    public void PlayAni()
    {
        _animator.Play($"Attack{_attack}");
        _attack = (_attack + 1) % 2;
    }
}
