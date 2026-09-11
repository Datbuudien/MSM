using System.Collections.Generic;
public class PlayerData
{
    public int Level;
    public int Gold;
    public int WeaponEquipped;
    public int HatEquipped;
    public int PantEquipped;
    public int AccessoryEquipped;
    public List<int> WeaponShopState = new List<int>();
    public List<int> HatShopState = new List<int>();
    public List<int> PantShopState = new List<int>();
    public List<int> AccessoryShopState = new List<int>();
    public bool HasSoundSettings;
    public float MusicVolume;
    public float SfxVolume;
}