public class CanvasLose : UICanvas
{
    public void OnClickHome()      => GameManager.ChangeState(GameState.MainMenu);
    public override void BackKey() => OnClickHome();
}
