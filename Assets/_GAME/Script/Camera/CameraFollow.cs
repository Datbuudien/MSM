using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset= new Vector3(0f,0.5f,0f);
    [SerializeField] private Transform tf;
    [SerializeField]private float smoothTime =.2f;
    private Vector3 velocity;
    void LateUpdate()
    {
        Vector3 pos = tf.position+offset;
        transform.position = Vector3.SmoothDamp(transform.position,pos,ref velocity,smoothTime);
        transform.LookAt(tf);
    }

}