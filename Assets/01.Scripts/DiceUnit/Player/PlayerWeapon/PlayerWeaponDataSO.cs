using UnityEngine;

[CreateAssetMenu(menuName = "SO/WeaponData")]
public class PlayerWeaponDataSO : ScriptableObject
{
    public EWeaponType weaponType = EWeaponType.None;
    [TextArea]
    public string atkRange = string.Empty;
    public int skillID = 0;
}
