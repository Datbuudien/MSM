using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
public class BotManager : Singleton<BotManager>
{
    [SerializeField] private Transform player;
    [SerializeField] private float spawnRadius=25f;
    [SerializeField]private float minDistanceToPlayer=10f;
    private readonly List<Bot> bots=new List<Bot>();
    public int AliveCount=>bots.Count;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool SpawnBot()
    {
        if(TryGetSpawnPoint(out Vector3 point)==false) return false;
        Bot bot = HBPools.Spawn<Bot>(PoolType.Bot,point,Quaternion.identity);
        if(bot==null) return false;
        bot.OnInit();
        bots.Add(bot);
        return true;
    }
    public void OnDeath(Bot bot)
    {
        if(bots.Remove(bot)==false) return;
        LevelManager.Ins.OnBotDeath();
    }
    public void CollectAll()
    {
        for(int i=bots.Count-1;i>=0;i--)HBPools.Despawn(bots[i]);
        bots.Clear();
    }
    private bool TryGetSpawnPoint(out Vector3 point)
    {   
        float sqrMinDis=minDistanceToPlayer*minDistanceToPlayer;
        point = Vector3.zero;
        for(int i = 0; i < Constatnts.SPAWN_TRY_COUNT; i++)
        {
            Vector3 candidate = Random.insideUnitSphere*spawnRadius;
            candidate.y=0f;
            if(NavMesh.SamplePosition(candidate,out NavMeshHit hit,spawnRadius,NavMesh.AllAreas)==false) continue;
            if((hit.position-player.position).sqrMagnitude<sqrMinDis) continue;
            point = hit.position;
            return true;
        }
        return false;
    }
}
