using UnityEngine;
public class DataManager: Singleton<DataManager>
{
    [SerializeField] private WeaponData wpData;
    [SerializeField] private HatData hData;
    [SerializeField] private PantData pData; 
    [SerializeField] private AccessoryData aData;
    public WeaponData WeaponData=>wpData;
    public HatData HatData =>hData;
    public PantData PantData=> pData;
    public AccessoryData AccessoryData => aData;

}