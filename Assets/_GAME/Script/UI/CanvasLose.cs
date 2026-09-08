using TMPro;
using UnityEngine;

public class CanvasLose : UICanvas
{
    [SerializeField] private TextMeshProUGUI txtReward;
    [SerializeField] private TextMeshProUGUI txtLevel;

    protected override void OnOpen()
    {
        txtReward.text = "+" + LevelManager.Ins.LastReward;
        txtLevel.text = "KILL " + LevelManager.Ins.KillCount;
    }
    public void OnClickHome()      => GameManager.ChangeState(GameState.MainMenu);
    public void OnClickRetry()     => GameManager.ChangeState(GameState.GamePlay);
    public override void BackKey() => OnClickHome();
}
