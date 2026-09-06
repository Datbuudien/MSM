using System;

[Serializable]
public class StatBonus
{
    public StatType Stat;
    public float FlatBonus;      // +
    public float PercentBonus;   // 0.2 = +20%
}