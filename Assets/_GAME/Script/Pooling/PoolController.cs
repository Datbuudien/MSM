using UnityEngine;
using System.Collections.Generic;
public class PoolController : MonoBehaviour
{
    [System.Serializable]
    public class PoolAmount
    {
        public GameUnit prefab;
        public int amount;
        public Transform root;
    }
    [SerializeField] private List<PoolAmount> pools = new List<PoolAmount>();
    void Awake()
    {
        for(int i = 0; i < pools.Count; i++)
        {
            HBPools.PreLoad(pools[i].prefab,pools[i].amount,pools[i].root);
        }
    }
}