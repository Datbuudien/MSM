using UnityEngine;
public class CanvasGameplay : UICanvas
{
    [SerializeField] private FloatingJoyStick joyStick;

    protected override void OnSetup() => InputManager.Ins.SetJoyStick(joyStick);

    public void OnClickPause() => OpenPopup<CanvasPause>();
}
