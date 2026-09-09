using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform tf;
    [SerializeField] private float smoothTime = .5f;

    [Header("GamePlay")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 8f, -10f);
    [SerializeField] private float lookHeight = -2f;

    // [Header("MainMenu")]
    // [SerializeField] private Vector3 menuOffset = new Vector3(0f, 1.1f, -5f);
    // [SerializeField] private float menuLookHeight = 1.1f;

    // private float lookHeight;
    private Vector3 velocity;

    void Awake() => SetMenuView(false);
    private bool isMenu;
    private readonly Vector3 menuPosition = new  Vector3(0f,101.100128f,5f);
    private readonly Quaternion menuRotation = new Quaternion(0,0.999950111f,0.00999224838f,0);

    void LateUpdate()
    {
        if(isMenu) return;
        transform.position = Vector3.SmoothDamp(transform.position, tf.position + offset, ref velocity, smoothTime);
        Vector3 lookPoint = tf.position + Vector3.up * lookHeight;
        Vector3 dir = lookPoint - transform.position;
        if (dir.sqrMagnitude < .0001f) return;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void SetMenuView(bool isMenu)
    {
        this.isMenu=isMenu;
        // offset = isMenu ? menuOffset : gameplayOffset;
        // lookHeight = isMenu ? menuLookHeight : gameplayLookHeight;
        transform.position=menuPosition;
        transform.rotation=menuRotation;
    }
    

}
