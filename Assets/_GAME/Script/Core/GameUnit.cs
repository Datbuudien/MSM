using UnityEngine;
public class GameUnit : MonoBehaviour
{
    [SerializeField] private PoolType type;
    public PoolType poolType=>type;
    private Transform tf;
    public Transform TF
    {
        get
        {
            if(tf==null)tf=transform;
            return tf;
        }
    }
    public virtual void OnDesPawn(){}
}