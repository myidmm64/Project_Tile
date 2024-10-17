using UnityEngine;

[CreateAssetMenu(menuName = "SO/WeaponData")]
public class PlayerWeaponDataSO : ScriptableObject
{
    [SerializeField]
    private GameObject _weaponPrefab;

    public EWeaponType weaponType = EWeaponType.None;
    [TextArea]
    public string atkRange = string.Empty;

    public int attackID = 0;
    public int skillID = 0;

    public PlayerWeapon GetWeapon()
    {
        GameObject obj = Instantiate(_weaponPrefab);
        if (obj == null)
        {
            Debug.LogError("_weaponPrefab �������� ������");
            return null;
        }
        PlayerWeapon weaponCompo = obj.GetComponent<PlayerWeapon>();
        if (weaponCompo == null)
        {
            Debug.LogError("weaponCompo �������� ������");
            return null;
        }

        return weaponCompo;
    }
}
