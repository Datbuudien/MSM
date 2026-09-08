using UnityEngine;

public class CanvasPause : UICanvas
{
    protected override void OnOpen() => GameManager.Pause();

    public override void CloseDirectly()
    {
        GameManager.Resume();
        base.CloseDirectly();
    }
    public override void BackKey() => Close(0f);
    public void OnClickResume()    => Close(0f);
    public void OnClickHome()      => GameManager.ChangeState(GameState.MainMenu);
}
