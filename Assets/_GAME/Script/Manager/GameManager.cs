using UnityEngine;
public enum GameState{MainMenu,GamePlay,Finish,Revive,Setting}
;
public class GameManager: Singleton<GameManager>
{
    private static GameState gameState;
    protected override void Awake()
    {
        base.Awake();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    public static void ChangeState(GameState s) => gameState =s;
    public static bool IsState(GameState s)=> gameState ==s;

}