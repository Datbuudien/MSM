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
        menuStage.SetActive(true);          // bat san DICH truoc
        cameraFollow.SetMenuView(true);
        player.OnInit();
        ApplyPlayerEquipment();             // OnInit co SetWeapon(0), phai mac lai NGAY sau no
        player.SetRangeVisible(false);      // menu khong hien vong tam danh
        player.Teleport(menuStagePoint.position, Quaternion.identity);
        arena.SetActive(false);             // roi moi tat san CU
    }
    public void OnStartGame()
    {
        BotManager.Ins.CollectAll();
        arena.SetActive(true);              // bat san + NavMesh truoc khi spawn bot     
        player.OnInit();
        ApplyPlayerEquipment();             // GamePlay co 2 loi vao, ca hai deu phai mac do (KI-20)
        player.Teleport(playerSpawnPoint.position, Quaternion.identity);
        cameraFollow.SetMenuView(false);
        player.SetRangeVisible(true);
        menuStage.SetActive(false);
        killCount = 0;
        level.StartStage(0);
        SpawnUntilFull();
    }
    // Mac do that theo save. Goi sau OnInit() o MOI loi vao, va luc dong shop.
    public void ApplyPlayerEquipment()
    {
        for(int c = 0; c < Constatnts.SHOP_CATEGORY_COUNT; c++)
        {
            ShopCategory category = (ShopCategory)c;
            PreviewEquip(category, SaveManager.Ins.GetEquipped(category));
        }
    }
    // Mac thu trong shop. O menu khong ai doc chi so nen khong can tach "mac hinh" voi "mac that".
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
        if (killer != null) killer.OnKill();  // bot giet duoc cung to len
        if (killer == player) killCount++;   // bot giet nhau thi khong tinh cong nguoi choi
        SpawnUntilFull();
        if (level.IsStageCleared(BotManager.Ins.AliveCount) == false) return;
        OnStageCleared();
    }
    public void OnPlayerDeath()
    {
        // thua van an cong suc: chi tien theo so bot da ha, khong co base va khong len level
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
