using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class ItemStatFiller
{
    private const float HAT_RANGE_MIN = .2f;
    private const float HAT_RANGE_MAX = 1f;

    private const float PERCENT_MIN = .03f;
    private const float PERCENT_MAX = .12f;

    private const float SIDEGRADE_UP = .15f;
    private const float SIDEGRADE_DOWN = -.12f;

    [MenuItem("Tools/Fill Item Stats")]
    private static void FillAll()
    {
        StringBuilder log = new StringBuilder("Dien chi so cho cac mon:\n");
        FillHats(log);
        FillPants(log);
        FillAccessories(log);
        FillWeapons(log);
        AssetDatabase.SaveAssets();
        Debug.Log(log.ToString());
    }


    private static void FillHats(StringBuilder log)
    {
        HatData data = LoadFirst<HatData>();
        if (data == null) return;
        MinMax cost = Range(data.Items, i => i.Cost);
        foreach (HatItem item in data.Items)
        {
            float value = Round1(Mathf.Lerp(HAT_RANGE_MIN, HAT_RANGE_MAX, cost.Normalize(item.Cost)));
            item.Bonuses = item.Cost <= 0 ? new StatBonus[0]
                                          : new[] { Flat(StatType.AttackRange, value) };
            Write(log, "Hat", item.Type.ToString(), item.Cost, item.Bonuses);
        }
        EditorUtility.SetDirty(data);
    }

    private static void FillPants(StringBuilder log)
    {
        PantData data = LoadFirst<PantData>();
        if (data == null) return;
        MinMax cost = Range(data.Items, i => i.Cost);
        foreach (PantItem item in data.Items)
        {
            float value = Round2(Mathf.Lerp(PERCENT_MIN, PERCENT_MAX, cost.Normalize(item.Cost)));
            item.Bonuses = item.Cost <= 0 ? new StatBonus[0]
                                          : new[] { Percent(StatType.MoveSpeed, value) };
            Write(log, "Pant", item.Type.ToString(), item.Cost, item.Bonuses);
        }
        EditorUtility.SetDirty(data);
    }

    private static void FillAccessories(StringBuilder log)
    {
        AccessoryData data = LoadFirst<AccessoryData>();
        if (data == null) return;
        MinMax cost = Range(data.Items, i => i.Cost);
        foreach (AccessoryItem item in data.Items)
        {
            float value = Round2(Mathf.Lerp(PERCENT_MIN, PERCENT_MAX, cost.Normalize(item.Cost)));
            item.Bonuses = item.Cost <= 0 ? new StatBonus[0]
                                          : new[] { Percent(StatType.AttackSpeed, value) };
            Write(log, "Acce", item.Type.ToString(), item.Cost, item.Bonuses);
        }
        EditorUtility.SetDirty(data);
    }

    private static void FillWeapons(StringBuilder log)
    {
        WeaponData data = LoadFirst<WeaponData>();
        if (data == null) return;
        foreach (WeaponItem item in data.Items)
        {
            item.Bonuses = item.Mat == null ? BareWeapon(item.BulletPool) : Skin((int)item.Type);
            Write(log, "Weapon", item.Type.ToString(), item.Cost, item.Bonuses);
        }
        EditorUtility.SetDirty(data);
    }

    private static StatBonus[] BareWeapon(PoolType pool)
    {
        switch (pool)
        {
            case PoolType.HammerBullet:
                return new[] { Percent(StatType.AttackRange, SIDEGRADE_UP),
                               Percent(StatType.AttackSpeed, SIDEGRADE_DOWN) };
            case PoolType.BoomerangBullet:
                return new[] { Percent(StatType.AttackSpeed, SIDEGRADE_UP),
                               Percent(StatType.AttackRange, SIDEGRADE_DOWN) };
            default:
                return new StatBonus[0];
        }
    }

    private static StatBonus[] Skin(int type)
    {
        switch (type % 3)
        {
            case 0:
                return new[] { Percent(StatType.AttackSpeed, SIDEGRADE_UP),
                               Percent(StatType.AttackRange, SIDEGRADE_DOWN) };
            case 1:
                return new[] { Percent(StatType.AttackRange, SIDEGRADE_UP),
                               Percent(StatType.MoveSpeed, SIDEGRADE_DOWN) };
            default:
                return new[] { Percent(StatType.MoveSpeed, SIDEGRADE_UP),
                               Percent(StatType.AttackSpeed, SIDEGRADE_DOWN) };
        }
    }


    private struct MinMax
    {
        public float Min;
        public float Max;
        public float Normalize(float value) => Mathf.Approximately(Max, Min) ? 1f
                                             : Mathf.InverseLerp(Min, Max, value);
    }

    private static MinMax Range<T>(IReadOnlyList<T> items, System.Func<T, int> cost)
    {
        MinMax res = new MinMax { Min = float.MaxValue, Max = float.MinValue };
        for (int i = 0; i < items.Count; i++)
        {
            int value = cost(items[i]);
            if (value <= 0) continue;
            res.Min = Mathf.Min(res.Min, value);
            res.Max = Mathf.Max(res.Max, value);
        }
        if (res.Min > res.Max) { res.Min = 0f; res.Max = 1f; }
        return res;
    }

    private static StatBonus Flat(StatType stat, float value)
        => new StatBonus { Stat = stat, FlatBonus = value, PercentBonus = 0f };
    private static StatBonus Percent(StatType stat, float value)
        => new StatBonus { Stat = stat, FlatBonus = 0f, PercentBonus = value };

    private static float Round1(float value) => Mathf.Round(value * 10f) / 10f;
    private static float Round2(float value) => Mathf.Round(value * 100f) / 100f;

    private static void Write(StringBuilder log, string kind, string name, int cost, StatBonus[] bonuses)
    {
        log.Append($"  {kind,-7} {name,-16} {cost,4}g  ");
        if (bonuses.Length == 0) { log.AppendLine("-"); return; }
        for (int i = 0; i < bonuses.Length; i++)
        {
            if (i > 0) log.Append(" · ");
            StatBonus b = bonuses[i];
            log.Append(Mathf.Approximately(b.FlatBonus, 0f)
                ? $"{b.Stat} {b.PercentBonus * 100f:+0;-0}%"
                : $"{b.Stat} {b.FlatBonus:+0.##;-0.##}");
        }
        log.AppendLine();
    }

    private static T LoadFirst<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        if (guids.Length == 0)
        {
            Debug.LogWarning($"Khong tim thay asset {typeof(T).Name}");
            return null;
        }
        return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[0]));
    }
}
