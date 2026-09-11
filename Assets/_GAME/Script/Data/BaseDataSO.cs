using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseDataSO<TItem,T> : ScriptableObject, IShopData
where TItem : ShopItemData<T>
where T : Enum
{
    [SerializeField] private List<TItem> items =  new List<TItem>();
    public IReadOnlyList<TItem> Items=> items;
    public TItem GetItem(T type)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if(EqualityComparer<T>.Default.Equals(items[i].Type,type)) return items[i];
        }
        return null;
    }

    public int Count => items.Count;
    public int GetId(int index) => Convert.ToInt32(items[index].Type);
    public int GetCost(int index) => items[index].Cost;
    public Sprite GetIcon(int index) => items[index].Icon;
    public StatBonus[] GetBonuses(int index) => items[index].Bonuses;
    public int IndexOfId(int id)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if(Convert.ToInt32(items[i].Type) == id) return i;
        }
        return -1;
    }
}