using UnityEngine;

public class UICanvas : MonoBehaviour
{
    // chay MOT lan luc UIManager tao instance
    public virtual void Setup() { }

    public virtual void Open()
    {
        CancelInvoke(nameof(CloseDirectly));
        gameObject.SetActive(true);
    }
    public virtual void Close(float delay)
    {
        if (delay <= 0f)
        {
            CloseDirectly();
            return;
        }
        Invoke(nameof(CloseDirectly), delay);
    }
    public virtual void CloseDirectly()
    {
        gameObject.SetActive(false);
    }
}
