using UnityEngine;
public interface IShopData
{
    int Count { get; }
    int GetId(int index);
    int GetCost(int index);
    Sprite GetIcon(int index);
    StatBonus[] GetBonuses(int index);
    int IndexOfId(int id);      
}
