public class CanvasSetting : UICanvas
{
    public void OnClickClose()     => Close(0f);
    public override void BackKey() => OnClickClose();
}
