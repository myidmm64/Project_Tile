using UnityEngine;

public class WeaponSword : PlayerWeapon
{
    public override void Attack()
    {
    }

    protected override void UpdateAttackTargets()
    {
        foreach(var posKey in PosKeyUtil.StrPattern(_player.positionKey, _data.atkRange))
        {
            /*
            DiceUnit unit = DiceGrid.Inst.units.ContainsKey(posKey);
            if ()
            {

            }
            */
        }
    }
}
