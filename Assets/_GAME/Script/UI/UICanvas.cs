using UnityEngine;

public class UICanvas : MonoBehaviour
{
    private const float NOTCH_RATIO_THRESHOLD = 2.1f;
    private const float NOTCH_TOP_OFFSET = 100f;
    private const float WIDESCREEN_RATIO_THRESHOLD = 2.1f;
    private const float WIDESCREEN_DEFAULT_RATIO = 850f / 1920f;

    [SerializeField] private bool isDestroyOnClose;
    [SerializeField] private bool isHandlingRabbitEars;
    [SerializeField] private bool isWidescreenProcessing;
    [Header("Popup Child")]
    [SerializeField] private UICanvas[] popups;

    protected RectTransform rectTransform;
    private bool isInit;
    private bool isSetup;

    public UICanvas ParentsPopup { get; set; }

    void Start()
    {
        EnsureInit();
    }

    // UIManager goi MOI lan mo. Hai hook duoi tach theo tan suat, dung nham la bug im lang
    public void Setup()
    {
        EnsureInit();
        UIManager.Ins.AddBackUI(this);
        UIManager.Ins.PushBackAction(this, BackKey);
        if (isSetup == false)
        {
            isSetup = true;
            OnSetup();
        }
        OnOpen();
    }

    // MOT lan trong doi canvas: cache, AddListener, dung danh sach item
    protected virtual void OnSetup() { }
    // MOI lan mo: doc lai gold, refresh trang thai dang chon
    protected virtual void OnOpen() { }
    // back key cua Android
    public virtual void BackKey() { }

    public virtual void Open()
    {
        CancelInvoke(nameof(CloseDirectly));
        gameObject.SetActive(true);
    }
    public virtual void Close(float delayTime)
    {
        if (delayTime <= 0f)
        {
            CloseDirectly();
            return;
        }
        Invoke(nameof(CloseDirectly), delayTime);
    }
    public virtual void CloseDirectly()
    {
        UIManager.Ins.RemoveBackUI(this);
        gameObject.SetActive(false);
        if (isDestroyOnClose) Destroy(gameObject);
    }

    private void EnsureInit()
    {
        if (isInit) return;
        isInit = true;
        rectTransform = transform as RectTransform;

        HandleRabbitEars();
        HandleWidescreen();

        for (int i = 0; i < popups.Length; i++)
        {
            if (popups[i] != null) popups[i].ParentsPopup = this;
        }
    }
    private void HandleRabbitEars()
    {
        if (isHandlingRabbitEars == false || rectTransform == null) return;
        float ratio = (float)Screen.height / Screen.width;
        if (ratio <= NOTCH_RATIO_THRESHOLD) return;
        Vector2 leftBottom = rectTransform.offsetMin;
        Vector2 rightTop = rectTransform.offsetMax;
        rightTop.y = -NOTCH_TOP_OFFSET;
        leftBottom.y = 0f;
        rectTransform.offsetMax = rightTop;
        rectTransform.offsetMin = leftBottom;
    }
    private void HandleWidescreen()
    {
        if (isWidescreenProcessing == false || rectTransform == null) return;
        float ratio = (float)Screen.width / Screen.height;
        if (ratio >= WIDESCREEN_RATIO_THRESHOLD) return;
        float value = 1f - (ratio - WIDESCREEN_DEFAULT_RATIO);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectTransform.rect.width * value);
    }

    #region Popup

    public T GetPopup<T>() where T : UICanvas
    {
        for (int i = 0; i < popups.Length; i++)
        {
            if (popups[i] is T tmp) return tmp;
        }
#if UNITY_EDITOR
        Debug.LogError($"Chua keo popup {typeof(T).Name} vao o Popups cua {GetType().Name}");
#endif
        return null;
    }
    public T OpenPopup<T>() where T : UICanvas
    {
        T ui = GetPopup<T>();
        if (ui == null) return null;
        ui.Setup();
        ui.Open();
        return ui;
    }
    public bool IsOpenedPopup<T>() where T : UICanvas
    {
        T ui = GetPopup<T>();
        return ui != null && ui.gameObject.activeSelf;
    }
    public void ClosePopup<T>(float delayTime) where T : UICanvas
    {
        T ui = GetPopup<T>();
        if (ui != null) ui.Close(delayTime);
    }
    public void ClosePopupDirect<T>() where T : UICanvas
    {
        T ui = GetPopup<T>();
        if (ui != null) ui.CloseDirectly();
    }
    public void CloseAllPopup()
    {
        for (int i = 0; i < popups.Length; i++)
        {
            if (popups[i] != null) popups[i].CloseDirectly();
        }
    }

    #endregion
}
