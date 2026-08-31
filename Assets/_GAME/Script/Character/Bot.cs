using UnityEngine;
using UnityEngine.AI;
public class Bot:Character
{
    [SerializeField] private NavMeshAgent agent;
    private IBotState currentState;
    protected override void Awake()
    {
        base.Awake();
        agent.updatePosition=false;
        agent.updateRotation=false;
    }

    public override void Move()
    {
        if (agent.isOnNavMesh == false)
        {
            MoveByDirection(Vector3.zero);
            return;
        }
        agent.nextPosition=TF.position;
        MoveByDirection(agent.isStopped?Vector3.zero:agent.desiredVelocity.normalized);
    }
    public override void OnInit()
    {
        base.OnInit();
        agent.Warp(TF.position);
        ChangeState(new IdleState());
    }
    protected override void OnUpdate() => currentState?.OnExcute(this);
    public void ChangeState(IBotState state)
    {
        currentState?.OnExit(this);
        currentState=state;
        currentState?.OnEnter(this);
    }
    public bool MoveToRandomPoint()
    {
        if(agent.isOnNavMesh==false) return false;
        Vector3 randomPoint = TF.position+Random.insideUnitSphere*Constatnts.BOT_PATROL_RADIUS;
        if(NavMesh.SamplePosition(randomPoint,out NavMeshHit hit, Constatnts.BOT_PATROL_RADIUS,NavMesh.AllAreas)==false) return false;
        agent.isStopped =false;
        agent.SetDestination(hit.position);
        return true;
    }
    public void StopMoving()
    {
        if(agent.isOnNavMesh==false) return;
        agent.ResetPath();
        agent.isStopped=true;
    }
    public bool IsAtDestination()
    {
       if(agent.isOnNavMesh==false) return true;
       if(agent.pathPending) return false;
       if(agent.hasPath==false) return true;
        return agent.remainingDistance <=Constatnts.BOT_ARRIVE_DISTANCE;

    }
    protected override void OnDeath()
    {
        base.OnDeath();
        StopMoving();
        BotManager.Ins.OnDeath(this);
        Invoke(nameof(DespawnSelf),Constatnts.BOT_DESPAWN_DELAY);
    }
    private void DespawnSelf()=>HBPools.Despawn(this);
}