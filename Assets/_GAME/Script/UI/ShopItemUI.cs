using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI txtCost;
    [SerializeField] private GameObject lockedMark;
    [SerializeField] private GameObject selectedMark;
    [SerializeField] private GameObject equippedMark;
    [SerializeField] private Button button;

    private ShopCategory category;
    private int id;
    private Action<ShopCategory, int> onClick;

    // MOT lan trong doi cell: cell duoc dung o OnSetup cua CanvasShop, khong bao gio dung lai (KI-14)
    public void OnSetup(ShopCategory category, int id, Sprite iconSprite, int cost, Action<ShopCategory, int> onClick)
    {
        this.category = category;
        this.id = id;
        this.onClick = onClick;
        icon.sprite = iconSprite;
        icon.enabled = iconSprite != null;      // chua co icon thi de trong, do hon la o vuong trang
        txtCost.text = cost.ToString();
        button.onClick.AddListener(HandleClick);
    }
    public void Refresh(bool owned, bool equipped, bool selected)
    {
        lockedMark.SetActive(owned == false);
        txtCost.gameObject.SetActive(owned == false);
        equippedMark.SetActive(equipped);
        selectedMark.SetActive(selected);
    }
    private void HandleClick() => onClick?.Invoke(category, id);
}
