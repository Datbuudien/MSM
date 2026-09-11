using UnityEngine;

public abstract class Bullet : GameUnit
{
    [SerializeField]  private float speed = 6f;
    [SerializeField] private MaterialSwapper swapper;
    protected float Speed => speed;
    protected Vector3 StartPos{get;private set;}
    protected float RangeAttack{get;private set;}
    protected Character owner{get;private set;}
    private float sqrRange;
    public virtual void OnInit(Vector3 pos,float range, Character owner)
    {
        StartPos = pos;
        RangeAttack=range;
        this.owner =owner;
        sqrRange = range*range;
    }
    void Update()
    {
        if(GameManager.CanPlay==false) return;
        Move();
        if (IsFinished())
        {
            HBPools.Despawn(this);
            return;
        }
    }
    public void SetMaterial(Material mat)
    {
        if(swapper==null) return;
        swapper.SetMaterial(mat);
    }
    protected abstract void Move();
    protected virtual bool IsFinished()=> (TF.position-StartPos).sqrMagnitude>sqrRange;
    void OnTriggerEnter(Collider other)
    {
        if(CharacterRegistry.TryGet(other,out Character victim) == false)
        {
            if(other.gameObject.layer != Constatnts.LAYER_OBSTACLE) return;
            if(owner!=null && owner.IsSoundOwner) SoundManager.Ins.PlaySfx(SfxType.WeaponHit);
            OnHitObstacle();
            return;
        }
        if(victim==owner) return;
        victim.OnHit(owner);
        HBPools.Despawn(this);
    }
    protected virtual void OnHitObstacle()=> HBPools.Despawn(this);
}