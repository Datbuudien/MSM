using UnityEngine;
public class DataManager: Singleton<DataManager>
{
    [SerializeField] private WeaponData weaponData;
    [SerializeField] private HatData hatData;
    [SerializeField] private PantData pantData;
    [SerializeField] private AccessoryData accessoryData;
    public WeaponData WeaponData => weaponData;
    public HatData HatData => hatData;
    public PantData PantData => pantData;
    public AccessoryData AccessoryData => accessoryData;

    public IShopData GetData(ShopCategory category)
    {
        switch(category)
        {
            case ShopCategory.Hat: return hatData;
            case ShopCategory.Pant: return pantData;
            case ShopCategory.Accessory: return accessoryData;
            default: return weaponData;
        }
    }
}
