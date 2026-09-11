using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform tf;
    [SerializeField] private float smoothTime = .5f;
    [Header("GamePlay")]
    [SerializeField] private Vector3 offset = new Vector3(0f, 8f, -10f);
    [SerializeField] private float lookHeight = -2f;
    [Header("MainMenu")]
    [SerializeField] private Vector3 menuPosition = new Vector3(0f, 101.100128f, 5f);
    [Header("Shop")]
    [SerializeField] private Vector3 shopOffset = new Vector3(0f, 1.2f, 6.6f);
    [SerializeField] private float shopLookHeight = -1.4f;
    private readonly Quaternion menuRotation = new Quaternion(0f, .999950111f, .00999224838f, 0f);
    private Vector3 velocity;
    private bool isMenu;
    private bool isShop;
    void Awake() => SetMenuView(false);
    void LateUpdate()
    {
        if (isMenu == false)
        {
            FollowTarget(tf.position + offset, lookHeight);
            return;
        }
        if (isShop)
        {
            FollowTarget(tf.position + shopOffset, shopLookHeight);
            return;
        }
        transform.SetPositionAndRotation(menuPosition, menuRotation);
    }
    private void FollowTarget(Vector3 position, float height)
    {
        transform.position = Vector3.SmoothDamp(transform.position, position, ref velocity, smoothTime);
        Vector3 dir = tf.position + Vector3.up * height - transform.position;
        if (dir.sqrMagnitude < .0001f) return;
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void SetMenuView(bool isMenu)
    {
        this.isMenu = isMenu;
        isShop = false;
        velocity = Vector3.zero;
        if (isMenu)
        {
            transform.SetPositionAndRotation(menuPosition, menuRotation);
            return;
        }
        transform.position = tf.position + offset;
    }
    public void SetShopView(bool isShop)
    {
        if (isMenu == false) return;
        this.isShop = isShop;
        velocity = Vector3.zero;
    }
}
