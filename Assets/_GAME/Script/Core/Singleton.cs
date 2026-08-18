using UnityEngine;
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T ins;
    protected bool IsDuplicate;
    public static T Ins
    {
        get
        {
            if (ins == null)
            {
                ins = FindFirstObjectByType<T>();
                if (ins == null)
                {
                    ins = new GameObject(typeof(T).Name).AddComponent<T>();
                }
            }
            return ins;
        }
    }
    protected virtual void Awake()
    {
        if(ins==null)
        {
            ins= this as T;
            return;
        }
        if(ins!=this) 
        {
            IsDuplicate = true;
            Destroy(gameObject);
        }
    }
}