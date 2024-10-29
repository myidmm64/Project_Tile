using UnityEngine;

[CreateAssetMenu(menuName = "SO/WeaponData")]
public class PlayerWeaponDataSO : ScriptableObject
{
    public string weaponName;
    public string explainText;
    public EWeaponType weaponType = EWeaponType.None;
    public int skillID = 0;

    [TextArea]
    public string atkRange = string.Empty;
    public int atk = 1;
    public float atkDelay = 0.4f;
    public int dpUp = 20;

    [SerializeField]
    private PlayerWeapon _weapon;

    public PlayerWeapon GetWeapon()
    {
        PlayerWeapon weaponCompo = Instantiate(_weapon);
        if (weaponCompo == null)
        {
            Debug.LogError("weaponCompo 가져오지 못했음");
            return null;
        }

        return weaponCompo;
    }
}
