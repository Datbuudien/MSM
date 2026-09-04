using UnityEngine;

public class RotateComponent : MonoBehaviour
{
    [SerializeField] private Transform tf;
    [SerializeField] private float speedRotation = 1080f;
    private float z;
    void OnEnable()
    {
        z =0f;
    }
    void Update()
    {
        z = (z+speedRotation*Time.deltaTime)%360f;     
        tf.localRotation= Quaternion.Euler(270,0,z);
    }
}
