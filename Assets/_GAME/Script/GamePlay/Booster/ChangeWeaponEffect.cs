using UnityEngine;

[CreateAssetMenu(fileName = "ChangeWeapon", menuName = "Game/Booster/Change Weapon")]
public class ChangeWeaponEffect:BoosterEffect
{
    public override bool TryApply(Character c)
    {
        return false;
    }
}