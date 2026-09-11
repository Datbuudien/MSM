using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasShop : UICanvas
{
    [SerializeField] private Transform content;
    [SerializeField] private ShopItemUI itemPrefab;
    [SerializeField] private TextMeshProUGUI txtGold;
    [SerializeField] private GameObject[] tabSelectedMarks;

    [Header("Nut mua / mac")]
    [SerializeField] private TextMeshProUGUI txtAction;
    [SerializeField] private Button btnAction;

    [Header("Lat trang")]
    [SerializeField] private Button btnPrevPage;
    [SerializeField] private Button btnNextPage;
    [SerializeField] private Image[] pageDots;
    [SerializeField] private Color dotOnColor = Color.white;
    [SerializeField] private Color dotOffColor = Color.gray;

    [Header("Chi so")]
    [SerializeField] private TextMeshProUGUI[] txtStats;
    [SerializeField] private GameObject infoPopup;
    [SerializeField] private Color statUpColor = Color.green;
    [SerializeField] private Color statDownColor = Color.red;
    [SerializeField] private Color statNoneColor = Color.gray;

    [Header("Xem ky / xoay nhan vat")]
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject inspectLayer;

    private bool isInspecting;
    private readonly List<ShopItemUI>[] cells = new List<ShopItemUI>[Constatnts.SHOP_CATEGORY_COUNT];
    private readonly int[] selectedIndex = new int[Constatnts.SHOP_CATEGORY_COUNT];
    private readonly int[] currentPage = new int[Constatnts.SHOP_CATEGORY_COUNT];
    private ShopCategory currentTab;

    protected override void OnSetup()
    {
        for (int c = 0; c < Constatnts.SHOP_CATEGORY_COUNT; c++)
        {
            ShopCategory category = (ShopCategory)c;
            IShopData data = DataManager.Ins.GetData(category);
            if (data == null)
            {
                cells[c] = new List<ShopItemUI>();
                continue;
            }
            List<ShopItemUI> list = new List<ShopItemUI>(data.Count);
            for (int i = 0; i < data.Count; i++)
            {
                ShopItemUI cell = Instantiate(itemPrefab, content);
                cell.OnSetup(category, data.GetId(i), data.GetIcon(i), data.GetCost(i), OnClickItem);
                list.Add(cell);
            }
            cells[c] = list;
        }
        btnAction.onClick.AddListener(OnClickAction);
        btnPrevPage.onClick.AddListener(OnClickPrevPage);
        btnNextPage.onClick.AddListener(OnClickNextPage);
    }
    protected override void OnOpen()
    {
        UIManager.Ins.CloseUI<CanvasMainMenu>();
        SetInspect(false);
        if (infoPopup != null) infoPopup.SetActive(false);
        for (int c = 0; c < Constatnts.SHOP_CATEGORY_COUNT; c++)
        {
            ShopCategory category = (ShopCategory)c;
            int index = DataManager.Ins.GetData(category).IndexOfId(SaveManager.Ins.GetEquipped(category));
            selectedIndex[c] = Mathf.Max(index, 0);
            currentPage[c] = selectedIndex[c] / Constatnts.SHOP_PAGE_SIZE;
        }
        OnClickTab((int)ShopCategory.Weapon);
    }

    public void OnClickTab(int index)
    {
        currentTab = (ShopCategory)index;
        for (int c = 0; c < Constatnts.SHOP_CATEGORY_COUNT; c++) tabSelectedMarks[c].SetActive(c == index);
        Refresh();
    }
    public void OnClickPrevPage()
    {
        int c = (int)currentTab;
        if (currentPage[c] <= 0) return;
        currentPage[c]--;
        Refresh();
    }
    public void OnClickNextPage()
    {
        int c = (int)currentTab;
        if (currentPage[c] >= PageCount(DataManager.Ins.GetData(currentTab)) - 1) return;
        currentPage[c]++;
        Refresh();
    }
    public void OnClickInspect() => SetInspect(isInspecting == false);

    private void SetInspect(bool on)
    {
        isInspecting = on;
        if (panel != null) panel.SetActive(on == false);
        if (inspectLayer != null) inspectLayer.SetActive(on);
        LevelManager.Ins.SetShopView(on == false);
    }

    public void OnClickInfo()
    {
        if (infoPopup == null) return;
        infoPopup.SetActive(infoPopup.activeSelf == false);
    }
    public void OnClickClose()
    {
        SetInspect(false);
        LevelManager.Ins.ApplyPlayerEquipment();
        LevelManager.Ins.ResetPlayerRotation();
        LevelManager.Ins.SetShopView(false);
        UIManager.Ins.OpenUI<CanvasMainMenu>();
        Close(0f);
    }
    public override void BackKey()
    {
        if (infoPopup != null && infoPopup.activeSelf)
        {
            infoPopup.SetActive(false);
            return;
        }
        if (isInspecting)
        {
            SetInspect(false);
            return;
        }
        OnClickClose();
    }

    private void OnClickItem(ShopCategory category, int id)
    {
        int index = DataManager.Ins.GetData(category).IndexOfId(id);
        if (index < 0) return;
        selectedIndex[(int)category] = index;
        LevelManager.Ins.PreviewEquip(category, id);
        Refresh();
    }
    private void OnClickAction()
    {
        IShopData data = DataManager.Ins.GetData(currentTab);
        int index = selectedIndex[(int)currentTab];
        int id = data.GetId(index);
        if (SaveManager.Ins.IsOwned(currentTab, id) == false
            && SaveManager.Ins.TryBuy(currentTab, id, data.GetCost(index)) == false) return;
        SaveManager.Ins.Equip(currentTab, id);
        SoundManager.Ins.PlaySfx(SfxType.Buy);
        Refresh();
    }

    private static int PageCount(IShopData data)
        => Mathf.Max(1, Mathf.CeilToInt((float)data.Count / Constatnts.SHOP_PAGE_SIZE));

    private void Refresh()
    {
        int c = (int)currentTab;
        IShopData data = DataManager.Ins.GetData(currentTab);
        int page = currentPage[c];
        int first = page * Constatnts.SHOP_PAGE_SIZE;
        int equippedId = SaveManager.Ins.GetEquipped(currentTab);
        int index = selectedIndex[c];

        for (int t = 0; t < Constatnts.SHOP_CATEGORY_COUNT; t++)
        {
            List<ShopItemUI> list = cells[t];
            bool isCurrentTab = t == c;
            for (int i = 0; i < list.Count; i++)
            {
                bool show = isCurrentTab && i >= first && i < first + Constatnts.SHOP_PAGE_SIZE;
                list[i].gameObject.SetActive(show);
                if (show == false) continue;
                int id = data.GetId(i);
                list[i].Refresh(SaveManager.Ins.IsOwned(currentTab, id), id == equippedId, i == index);
            }
        }
        txtGold.text = SaveManager.Ins.Data.Gold.ToString();
        RefreshPage(PageCount(data), page);
        RefreshStats(data, index);
        RefreshActionButton(data, index, equippedId);
    }
    private void RefreshPage(int pageCount, int page)
    {
        btnPrevPage.interactable = page > 0;
        btnNextPage.interactable = page < pageCount - 1;
        for (int i = 0; i < pageDots.Length; i++)
        {
            pageDots[i].gameObject.SetActive(i < pageCount);
            pageDots[i].color = i == page ? dotOnColor : dotOffColor;
        }
    }
    private void RefreshStats(IShopData data, int index)
    {
        for (int i = 0; i < txtStats.Length; i++)
        {
            txtStats[i].text = Constatnts.SHOP_STAT_NONE;
            txtStats[i].color = statNoneColor;
        }
        StatBonus[] bonuses = data.GetBonuses(index);
        if (bonuses == null) return;
        for (int i = 0; i < bonuses.Length; i++)
        {
            int stat = (int)bonuses[i].Stat;
            if (stat < 0 || stat >= txtStats.Length) continue;
            txtStats[stat].text = Describe(bonuses[i]);
            txtStats[stat].color = bonuses[i].FlatBonus + bonuses[i].PercentBonus < 0f ? statDownColor : statUpColor;
        }
    }
    private static string Describe(StatBonus bonus)
    {
        string res = "";
        if (Mathf.Approximately(bonus.FlatBonus, 0f) == false)
            res = Sign(bonus.FlatBonus) + bonus.FlatBonus.ToString("0.##");
        if (Mathf.Approximately(bonus.PercentBonus, 0f) == false)
        {
            if (res.Length > 0) res += " ";
            res += Sign(bonus.PercentBonus) + (bonus.PercentBonus * 100f).ToString("0") + "%";
        }
        return res.Length == 0 ? Constatnts.SHOP_STAT_NONE : res;
    }
    private static string Sign(float value) => value > 0f ? "+" : "";

    private void RefreshActionButton(IShopData data, int index, int equippedId)
    {
        int id = data.GetId(index);
        if (SaveManager.Ins.IsOwned(currentTab, id) == false)
        {
            int cost = data.GetCost(index);
            txtAction.text = Constatnts.SHOP_LABEL_BUY + " " + cost;
            btnAction.interactable = SaveManager.Ins.Data.Gold >= cost;
            return;
        }
        bool isEquipped = id == equippedId;
        txtAction.text = isEquipped ? Constatnts.SHOP_LABEL_EQUIPPED : Constatnts.SHOP_LABEL_EQUIP;
        btnAction.interactable = isEquipped == false;
    }
}
