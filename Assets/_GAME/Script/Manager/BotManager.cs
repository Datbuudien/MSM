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
    public bool SpawnBot(int playerLevel)
    {
        if(TryGetSpawnPoint(out Vector3 point)==false) return false;
        Bot bot = HBPools.Spawn<Bot>(PoolType.Bot,point,Quaternion.identity);
        if(bot==null) return false;
        bot.OnInit();
        bot.SetSizeLevel(RollBotLevel(playerLevel));
        bots.Add(bot);
        return true;
    }
    private int RollBotLevel(int playerLevel)
    {
        int hi = Mathf.Min(playerLevel+2,Character.MAX_SIZE_LEVEL);
        int lo = Mathf.Max(hi-2,1);
        return Random.Range(lo,hi+1);
    }
    public void OnDeath(Bot bot, Character killer)
    {
        if(bots.Remove(bot)==false) return;
        LevelManager.Ins.OnBotDeath(killer);
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
