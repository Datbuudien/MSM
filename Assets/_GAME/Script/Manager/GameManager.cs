using UnityEngine;

public enum GameState { MainMenu, GamePlay, Win, Lose }

public class GameManager : Singleton<GameManager>
{
    private static GameState gameState;
    private static bool isPaused;

    // cau hoi duy nhat moi thu dong duoc phep hoi
    public static bool CanPlay => gameState == GameState.GamePlay && isPaused == false;

    protected override void Awake()
    {
        base.Awake();
        if (IsDuplicate) return;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    void Start()
    {
        ChangeState(GameState.MainMenu);
    }

    public static bool IsState(GameState state) => gameState == state;

    public static void Pause()
    {
        if (isPaused) return;
        isPaused = true;
        CharacterRegistry.PauseAll();
    }
    public static void Resume()
    {
        if (isPaused == false) return;
        isPaused = false;
        CharacterRegistry.ResumeAll();
    }

    public static void ChangeState(GameState state)
    {
        Resume();               // roi trang thai la khong duoc de gi dong bang lai
        gameState = state;
        Ins.OnStateChanged(state);
    }

    private void OnStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                UIManager.Ins.CloseAll();
                UIManager.Ins.ClearBackKey();
                LevelManager.Ins.OnResetLevel();
                UIManager.Ins.OpenUI<CanvasMainMenu>();
                break;
            case GameState.GamePlay:
                UIManager.Ins.CloseAll();
                LevelManager.Ins.OnStartGame();
                UIManager.Ins.OpenUI<CanvasGameplay>();
                break;
            case GameState.Win:
                UIManager.Ins.CloseUI<CanvasGameplay>();
                UIManager.Ins.OpenUI<CanvasVictory>();
                break;
            case GameState.Lose:
                UIManager.Ins.CloseUI<CanvasGameplay>();
                UIManager.Ins.OpenUI<CanvasLose>();
                break;
        }
    }
}
