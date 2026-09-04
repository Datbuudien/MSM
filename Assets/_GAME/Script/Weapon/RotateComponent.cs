using UnityEngine;

public class RotateComponent : MonoBehaviour
{
    [SerializeField] private Transform tf;
    [SerializeField] private float speedRotation = 1080f;
    float z =0f;
    void Update()
    {
        z = (z+speedRotation*Time.deltaTime)%360f;
        Vector3 tmp = tf.localEulerAngles;
        tf.localRotation= Quaternion.Euler(tmp.x,tmp.y,z);
    }
}
