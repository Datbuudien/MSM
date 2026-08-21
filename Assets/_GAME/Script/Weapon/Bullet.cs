using UnityEngine;

public abstract class Bullet : GameUnit
{
    [SerializeField]  private float speed = 6f;
    protected float Speed => speed;
    protected Vector3 StartPos{get;private set;}
    protected float RangeAttack{get;private set;}
    protected Transform owner{get;private set;}
    private float sqrRange;
    public virtual void OnInit(Vector3 pos,float range, Transform owner)
    {
        StartPos = pos;
        RangeAttack=range;
        this.owner =owner;
        sqrRange = range*range;
    }
    void Update()
    {
        Move();
        if (IsFinished())
        {
            HBPools.Despawn(this);
            return;
        }
    }
    protected abstract void Move();
    protected virtual bool IsFinished()=> (TF.position-StartPos).sqrMagnitude>sqrRange;
}