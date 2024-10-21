using UnityEngine;

public class WeaponSword : PlayerWeapon
{
    public override void Attack()
    {
        _attackTargets[0].Damage(1);
        Debug.Log("어택택택");
    }

    protected override void UpdateAttackTargets()
    {
        foreach(var posKey in PosKeyUtil.StrPattern(_player.positionKey, _data.atkRange))
        {
            if(DiceGrid.Inst.units.TryGetValue(posKey, out var unit))
            {
                _attackTargets.Add(unit);
            }
        }
    }
}
