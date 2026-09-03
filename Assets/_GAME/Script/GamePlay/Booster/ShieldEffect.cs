using UnityEngine;
[CreateAssetMenu(fileName = "Shield", menuName = "Game/Booster/Shield")]
public class ShieldEffect:TimedBoosterEffect
{
    public override void OnBegin(Character c)
    {
        c.SetShield(true);
    }
    public override void OnEnd(Character c)
    {
        c.SetShield(false);
    }
}