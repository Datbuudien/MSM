using TMPro;
using UnityEngine;

public class CanvasVictory : UICanvas
{
    [SerializeField] private TextMeshProUGUI txtReward;
    [SerializeField] private TextMeshProUGUI txtLevel;

    protected override void OnOpen()
    {
        txtReward.text = "+" + LevelManager.Ins.LastReward;
        txtLevel.text = "LEVEL " + SaveManager.Ins.Data.Level;
    }
    public void OnClickContinue()  => GameManager.ChangeState(GameState.MainMenu);
    public override void BackKey() => OnClickContinue();
}
