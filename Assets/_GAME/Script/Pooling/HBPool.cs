using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class HBPools
{
    private class Pool
    {
        public GameUnit Prefab;
        public Transform Root;
        public readonly Queue<GameUnit> Inactive = new Queue<GameUnit>();
        public readonly List <GameUnit> Active = new List<GameUnit> ();
    }
    private static readonly Dictionary<PoolType,Pool> pools = new Dictionary<PoolType,Pool>();
    public static void PreLoad(GameUnit prefab, int n, Transform root)
    {
        Pool p = GetOrCreate(prefab,root);
        for(int i = 0; i< n; i++)
        {
            GameUnit u= Object.Instantiate(prefab,root);
            u.gameObject.SetActive(false);
            p.Inactive.Enqueue(u);
        }
    }
    public static T Spawn<T> (PoolType type,Vector3 pos,Quaternion rot) where T : GameUnit
    {
        Pool p;
        if(!pools.TryGetValue(type,out p)) return null;
        GameUnit unit = p.Inactive.Count >0? p.Inactive.Dequeue():Object.Instantiate(p.Prefab,p.Root);
        unit.TF.SetPositionAndRotation(pos,rot);
        unit.gameObject.SetActive(true);
        p.Active.Add(unit);
        return unit as T;
    }
    public static void Despawn(GameUnit unit)
    {
        Pool p;
        if(unit ==null || !pools.TryGetValue(unit.poolType,out p)) return;
        if(!p.Active.Remove(unit)) return;
        unit.OnDesPawn();
        unit.gameObject.SetActive(false);
        p.Inactive.Enqueue(unit);
    }
    public static void CollectAll()
    {
        foreach (Pool p in pools.Values)
        {
            int i = p.Active.Count-1;
            while (i >= 0)
            {
                Despawn(p.Active[i]);
                i--;
            }
        }
    }
    private static Pool GetOrCreate(GameUnit u,Transform t)
    {
        if(pools.TryGetValue(u.poolType, out Pool existing)) return existing;
        Pool p = new Pool{Prefab=u,Root=t};
        pools.Add(u.poolType,p);
        return p;
    }
}