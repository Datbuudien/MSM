using UnityEngine;
[CreateAssetMenu(fileName = "StatBuff", menuName = "Game/Booster/Stat Buff")]
public class StatBuffEffect:TimedBoosterEffect
{
    [SerializeField] private StatType stat;
    [SerializeField]private float multiplier =1.5f;
    public override void OnBegin(Character c)
    {
        c.SetStatMultiplier(stat,multiplier);
    }
    public override void OnEnd(Character c)
    {
        c.SetStatMultiplier(stat,1f);
    }
}