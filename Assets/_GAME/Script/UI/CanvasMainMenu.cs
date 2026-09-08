public class CanvasMainMenu : UICanvas
{
    public void OnClickPlay()       => GameManager.ChangeState(GameState.GamePlay);
    public void OnClickShop()       => UIManager.Ins.OpenUI<CanvasShop>();
    public void OnClickWeaponShop() => UIManager.Ins.OpenUI<CanvasWeaponShop>();
    public void OnClickSetting()    => UIManager.Ins.OpenUI<CanvasSetting>();
    //protected override void OnOpen() => GameManager.ChangeState(GameState.GamePlay);
}
