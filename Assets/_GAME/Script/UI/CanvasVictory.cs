public class CanvasVictory : UICanvas
{
    public void OnClickContinue()  => GameManager.ChangeState(GameState.MainMenu);
    public override void BackKey() => OnClickContinue();
}
