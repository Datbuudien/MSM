using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Player player;
    [SerializeField] private Level level;
    [SerializeField] private CameraFollow cameraFollow;

    [Header("Arena")]
    [SerializeField] private GameObject arena;
    [SerializeField] private Transform playerSpawnPoint;

    [Header("MenuStage")]
    [SerializeField] private GameObject menuStage;
    [SerializeField] private Transform menuStagePoint;

    private int lastReward;
    private int killCount;
    public int LastReward => lastReward;
    public int KillCount => killCount;

    public void OnResetLevel()
    {
        BotManager.Ins.CollectAll();
        menuStage.SetActive(true);
        cameraFollow.SetMenuView(true);
        player.OnInit();
        ApplyPlayerEquipment();
        player.SetRangeVisible(false);
        player.Teleport(menuStagePoint.position, Quaternion.identity);
        arena.SetActive(false);
    }
    public void OnStartGame()
    {
        BotManager.Ins.CollectAll();
        arena.SetActive(true);
        player.OnInit();
        ApplyPlayerEquipment();
        player.Teleport(playerSpawnPoint.position, Quaternion.identity);
        cameraFollow.SetMenuView(false);
        player.SetRangeVisible(true);
        menuStage.SetActive(false);
        killCount = 0;
        level.StartStage(0);
        SpawnUntilFull();
    }
    public void ApplyPlayerEquipment()
    {
        for(int c = 0; c < Constatnts.SHOP_CATEGORY_COUNT; c++)
        {
            ShopCategory category = (ShopCategory)c;
            PreviewEquip(category, SaveManager.Ins.GetEquipped(category));
        }
    }
    public void RotatePlayer(float degrees) => player.RotateBy(degrees);
    public void ResetPlayerRotation() => player.ResetRotation();
    public void SetShopView(bool isShop) => cameraFollow.SetShopView(isShop);
    public void PreviewEquip(ShopCategory category, int id)
    {
        switch(category)
        {
            case ShopCategory.Hat: player.ChangeHat((HatType)id); break;
            case ShopCategory.Pant: player.ChangePant((PantType)id); break;
            case ShopCategory.Accessory: player.ChangeAccessory((AccessoryType)id); break;
            default: player.ChangeWeapon((WeaponType)id); break;
        }
    }
    public void OnBotDeath(Character killer)
    {
        if (killer != null) killer.OnKill();
        if (killer == player) killCount++;
        SpawnUntilFull();
        if (level.IsStageCleared(BotManager.Ins.AliveCount) == false) return;
        OnStageCleared();
    }
    public void OnPlayerDeath()
    {
        lastReward = killCount * Constatnts.GOLD_PER_KILL;
        SaveManager.Ins.Data.Gold += lastReward;
        SaveManager.Ins.Save();
        GameManager.ChangeState(GameState.Lose);
    }

    public void SpawnUntilFull()
    {
        while (level.CanSpawnMore(BotManager.Ins.AliveCount))
        {
            if (BotManager.Ins.SpawnBot(player.SizeLevel) == false) return;
            level.OnBotSpawned();
        }
    }
    private void OnStageCleared()
    {
        if (level.HasNextStage == false)
        {
            OnWin();
            return;
        }
        level.StartStage(level.CurrentStage + 1);
        SpawnUntilFull();
    }
    private void OnWin()
    {
        PlayerData data = SaveManager.Ins.Data;
        lastReward = Constatnts.GOLD_WIN_BASE
                   + data.Level * Constatnts.GOLD_WIN_PER_LEVEL
                   + killCount * Constatnts.GOLD_PER_KILL;
        data.Gold += lastReward;
        data.Level++;
        SaveManager.Ins.Save();
        GameManager.ChangeState(GameState.Win);
    }
}
