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
}
