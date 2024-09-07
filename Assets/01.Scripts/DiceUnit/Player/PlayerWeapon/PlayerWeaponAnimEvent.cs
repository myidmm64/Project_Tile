using UnityEngine;

public class PlayerWeaponAnimEvent : MonoBehaviour
{
    private PlayerWeapon _weapon = null;

    private void Awake()
    {
        _weapon = transform.GetComponentInParent<PlayerWeapon>();
    }

    public void Spawn()
    {
        _weapon.SpawnAttackObj();
    }

}
