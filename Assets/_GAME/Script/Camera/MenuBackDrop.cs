using UnityEngine;
public class MenuBackDrop: MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float distance = 12f;
    [SerializeField] private Vector2 imageSize = new Vector2(641f,1672f);
    void OnEnable()
    {
        transform.localPosition = new Vector3 (0f,0f,distance);
        transform.localRotation = Quaternion.identity;

        float h = 2f*distance*Mathf.Tan(cam.fieldOfView*0.5f*Mathf.Deg2Rad);
        float w = h* cam.aspect;
        float imageAspect = imageSize.x/imageSize.y;
        if(cam.aspect>imageAspect) h = w/imageAspect;
        else w=h*imageAspect;
        transform.localScale = new Vector3(w, h, 1f);
    }
}