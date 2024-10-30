using UnityEngine;

[CreateAssetMenu(menuName = "SO/WeaponData")]
public class PlayerWeaponDataSO : ScriptableObject
{
    public string weaponName;
    public string explainText;
    public EWeaponType weaponType = EWeaponType.None;
    public int skillID = 0;

    [SerializeField]
    private RangeDataSO _attackRange;
    public RangeDataSO attackRange => _attackRange;
    
    public int physicalDamage; // ������
    public int magicalDamage; // ������
    public float atkDelay = 0.4f;
    public int dpUp = 20;

    [SerializeField]
    private PlayerWeapon _weapon;

    public PlayerWeapon GetWeapon()
    {
        PlayerWeapon weaponCompo = Instantiate(_weapon);
        if (weaponCompo == null)
        {
            Debug.LogError("weaponCompo �������� ������");
            return null;
        }

        return weaponCompo;
    }
}
