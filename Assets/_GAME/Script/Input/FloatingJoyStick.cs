using UnityEngine;
using UnityEngine.EventSystems;
public class FloatingJoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]private RectTransform bg;
    [SerializeField]private RectTransform handle;
    [SerializeField]private CanvasGroup canvasGroup;
    [SerializeField]private UnityEngine.UI.Image bgIMG;
    [SerializeField]private UnityEngine.UI.Image handleIMG;
    private const int CHECK = int.MinValue;
    private int pointerId = CHECK;
    private Vector2 inputVector;
    public float Horizontal => inputVector.x;
    public float Vertical => inputVector.y;
    void Start()
    {
        HideJoyStick();
    }
    private void HideJoyStick()
    {
        canvasGroup.alpha =0f;



    }
    private void ShowJoyStick()
    {
        canvasGroup.alpha =1f;


    }
    public void OnPointerDown(PointerEventData e)
    {
        if(GameManager.CanPlay==false) return;
        if(pointerId != CHECK) return;
        pointerId = e.pointerId;
        ShowJoyStick();
        bg.position = e.position;
        handle.anchoredPosition = Vector2.zero;
        OnDrag(e);
    }
    public void OnDrag(PointerEventData e)
    {
        if(e.pointerId != pointerId) return;
        if(GameManager.CanPlay==false) { HideJoyStick(); return; }
        Vector2 pos;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(bg,e.position, e.pressEventCamera, out pos))
        {
            pos.x = (pos.x/bg.sizeDelta.x);
            pos.y = (pos.y/bg.sizeDelta.y);
            inputVector = new Vector2(pos.x*2,pos.y*2);
            inputVector = (inputVector.sqrMagnitude>1.0f)?inputVector.normalized:inputVector;
            handle.anchoredPosition = new Vector2(inputVector.x*(bg.sizeDelta.x/2),inputVector.y*(bg.sizeDelta.y/2));
        }
    }
    public void OnPointerUp(PointerEventData e)
    {
        if(e.pointerId!=pointerId) return;
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
        pointerId = CHECK;
        HideJoyStick();
    }
    void OnDisable()
    {
        inputVector = Vector2.zero;
        pointerId=CHECK;
    }
}
