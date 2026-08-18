using UnityEngine;
public class FPS : MonoBehaviour
{
    [SerializeField] private float intervalTime =.5f;
    private int currentFPS =0;
    private float time =.0f;
    private float fps;
    #if UNITY_EDITOR
    void Update()
    {
        time+= Time.unscaledDeltaTime;
        currentFPS++;
        if(time<intervalTime) return;
        fps = (float) currentFPS / time;
        time =0f;
        currentFPS=0;
        Debug.Log($"Frame: {fps}");
    }
    #endif
}