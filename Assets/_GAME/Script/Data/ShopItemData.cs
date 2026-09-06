using System;
using UnityEngine;
[Serializable]
public abstract class ShopItemData<T> where T: Enum
{
    public T Type;
    public int Cost;
    public Sprite Icon;    
    public StatBonus[] Bonuses;
}