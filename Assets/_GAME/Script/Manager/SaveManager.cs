using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    private const string FILE_NAME = "playerdata.json";

    private PlayerData data;

    public PlayerData Data
    {
        get
        {
            if (data == null) Load();
            return data;
        }
    }

    private string FilePath => Path.Combine(Application.persistentDataPath, FILE_NAME);

    protected override void Awake()
    {
        base.Awake();
        if (IsDuplicate) return;
        if (data == null) Load();
    }
    void OnApplicationPause(bool pause)
    {
        if (pause) Save();
    }

    public void Load()
    {
        data = null;
        if (File.Exists(FilePath))
        {
            try { data = JsonMapper.ToObject<PlayerData>(File.ReadAllText(FilePath)); }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogError($"Save hong, dung mac dinh: {e.Message}");
#endif
                data = null;
            }
        }
        if (data == null) data = CreateDefault();
        Validate();
    }
    public void Save()
    {
        if (data == null) return;
        // ghi ra file tam roi moi thay the: bi giet giua chung thi file that van nguyen ven
        string tempPath = FilePath + ".tmp";
        File.WriteAllText(tempPath, JsonMapper.ToJson(data));
        if (File.Exists(FilePath)) File.Delete(FilePath);
        File.Move(tempPath, FilePath);
    }

    public bool IsOwned(ShopCategory category, int id)
    {
        List<int> state = GetState(category);
        return id >= 0 && id < state.Count && state[id] == 1;
    }
    public bool TryBuy(ShopCategory category, int id, int cost)
    {
        if (IsOwned(category, id)) return false;
        if (Data.Gold < cost) return false;
        data.Gold -= cost;
        GetState(category)[id] = 1;
        Save();
        return true;
    }
    public void Equip(ShopCategory category, int id)
    {
        if (IsOwned(category, id) == false) return;
        switch (category)
        {
            case ShopCategory.Hat: data.HatEquipped = id; break;
            case ShopCategory.Pant: data.PantEquipped = id; break;
            case ShopCategory.Accessory: data.AccessoryEquipped = id; break;
            default: data.WeaponEquipped = id; break;
        }
        Save();
    }
    public int GetEquipped(ShopCategory category)
    {
        switch (category)
        {
            case ShopCategory.Hat: return Data.HatEquipped;
            case ShopCategory.Pant: return Data.PantEquipped;
            case ShopCategory.Accessory: return Data.AccessoryEquipped;
            default: return Data.WeaponEquipped;
        }
    }

    private List<int> GetState(ShopCategory category)
    {
        PlayerData tmp = Data;
        switch (category)
        {
            case ShopCategory.Hat: return tmp.HatShopState;
            case ShopCategory.Pant: return tmp.PantShopState;
            case ShopCategory.Accessory: return tmp.AccessoryShopState;
            default: return tmp.WeaponShopState;
        }
    }
    private void Validate()
    {
        EnsureSize(data.WeaponShopState, Enum.GetValues(typeof(WeaponType)).Length);
        EnsureSize(data.HatShopState, Enum.GetValues(typeof(HatType)).Length);
        EnsureSize(data.PantShopState, Enum.GetValues(typeof(PantType)).Length);
        EnsureSize(data.AccessoryShopState, Enum.GetValues(typeof(AccessoryType)).Length);

        data.Gold = Mathf.Max(0, data.Gold);
        data.Level = Mathf.Max(0, data.Level);
        data.WeaponEquipped = Clamp(data.WeaponEquipped, data.WeaponShopState.Count);
        data.HatEquipped = Clamp(data.HatEquipped, data.HatShopState.Count);
        data.PantEquipped = Clamp(data.PantEquipped, data.PantShopState.Count);
        data.AccessoryEquipped = Clamp(data.AccessoryEquipped, data.AccessoryShopState.Count);
    }
    private PlayerData CreateDefault()
    {
        PlayerData tmp = new PlayerData();
        // phai noi list truoc khi index vao no: PlayerData khoi tao list RONG
        EnsureSize(tmp.WeaponShopState, Enum.GetValues(typeof(WeaponType)).Length);
        tmp.WeaponShopState[(int)WeaponType.Knife] = 1;   // dao la vu khi cho khong
        tmp.WeaponEquipped = (int)WeaponType.Knife;
        return tmp;
    }
    private static void EnsureSize(List<int> list, int count)
    {
        while (list.Count < count) list.Add(0);
    }
    private static int Clamp(int value, int count) => (value < 0 || value >= count) ? 0 : value;
}
