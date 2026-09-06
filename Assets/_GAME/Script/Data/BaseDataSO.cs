using System;
using System.Collections.Generic;
using UnityEngine;
public abstract class BaseDataSO<TItem,T> : ScriptableObject
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
}