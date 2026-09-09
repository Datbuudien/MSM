using TMPro;
using UnityEngine;

public class CanvasMainMenu : UICanvas
{
    [SerializeField] private TextMeshProUGUI txtGold;
    protected override void OnOpen() => RefreshGold();
    // shop nam DE LEN main menu chu khong dong no, nen mua xong phai bao rieng
    public void RefreshGold() => txtGold.text = SaveManager.Ins.Data.Gold.ToString();
    public void OnClickPlay()=> GameManager.ChangeState(GameState.GamePlay);
    public void OnClickShop()=> UIManager.Ins.OpenUI<CanvasShop>();
    public void OnClickWeaponShop()=> UIManager.Ins.OpenUI<CanvasWeaponShop>();
    public void OnClickSetting()=> UIManager.Ins.OpenUI<CanvasSetting>();
}
