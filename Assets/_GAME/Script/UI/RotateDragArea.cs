using UnityEngine;
using UnityEngine.EventSystems;

public class RotateDragArea : MonoBehaviour, IDragHandler
{
    [SerializeField] private float degreePerPixel = .4f;

    public void OnDrag(PointerEventData eventData)
    {
        if (Mathf.Approximately(eventData.delta.x, 0f)) return;
        LevelManager.Ins.RotatePlayer(-eventData.delta.x * degreePerPixel);
    }
}
