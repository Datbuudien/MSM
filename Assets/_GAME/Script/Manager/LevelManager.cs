using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Player player;
    [SerializeField] private Level level;
    [SerializeField] private GameObject arena;
    [SerializeField] private GameObject menuBackdrop;
    public void OnResetLevel()
    {
        BotManager.Ins.CollectAll();
        player.OnInit();
        player.SetKinematic(true);
        arena.SetActive(false);
        menuBackdrop.SetActive(true);
    }
    public void OnStartGame()
    {
        level.StartStage(0);
        SpawnUntilFull();
        player.SetKinematic(false);
        level.StartStage(0);
        SpawnUntilFull();
    }
    public void OnBotDeath()
    {
        SpawnUntilFull();
        if (level.IsStageCleared(BotManager.Ins.AliveCount) == false) return;
        OnStageCleared();
    }
    public void OnPlayerDeath() => GameManager.ChangeState(GameState.Lose);

    public void SpawnUntilFull()
    {
        while (level.CanSpawnMore(BotManager.Ins.AliveCount))
        {
            if (BotManager.Ins.SpawnBot() == false) return;
            level.OnBotSpawned();
        }
    }
    private void OnStageCleared()
    {
        if (level.HasNextStage == false)
        {
            GameManager.ChangeState(GameState.Win);
            return;
        }
        level.StartStage(level.CurrentStage + 1);
        SpawnUntilFull();
    }

}
