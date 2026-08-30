using System;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Player player;
    [SerializeField] private Level level;
    void Start()
    {
        OnLoadLevel();
    }

    // Update is called once per frame
    public void OnLoadLevel()
    {
        BotManager.Ins.CollectAll();
        player.OnInit();
        level.StartStage(0);
        GameManager.ChangeState(GameState.GamePlay);
        SpawnUntilFull();
    }
    public void OnBotDeath()
    {
        SpawnUntilFull();
        if(level.IsStageCleared(BotManager.Ins.AliveCount)==false) return;
        OnStageCleared();
    }
    public void OnPlayerDeath()
    {
        
    }
    public void SpawnUntilFull()
    {
        
    }
    private void OnStageCleared()
    {
        
    }
    private void OnWin()
    {
        
    }
}
