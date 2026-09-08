using TMPro;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI txtGold;
    protected override void OnOpen() => txtGold.text = SaveManager.Ins.Data.Gold.ToString();
    public void OnClickPlay()=> GameManager.ChangeState(GameState.GamePlay);
    public void OnClickShop()=> UIManager.Ins.OpenUI<CanvasShop>();
    public void OnClickWeaponShop()=> UIManager.Ins.OpenUI<CanvasWeaponShop>();
    public void OnClickSetting()=> UIManager.Ins.OpenUI<CanvasSetting>();
}
