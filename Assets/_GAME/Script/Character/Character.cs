using System.Collections.Generic;
using UnityEngine;

public abstract class Character : GameUnit
{
    [SerializeField] private Rigidbody rb;
    [SerializeField]private Collider coll;
    [SerializeField] private Animator anim;
    [SerializeField]private float speed=.5f;
    [SerializeField]private float roateSpeed =.5f;
    [SerializeField] private Transform throwPoint;
    [SerializeField]protected WeaponHand weaponHand;
    [SerializeField]private float attackSpeed =1f;
    [SerializeField]private float attackRange =1f;
    [SerializeField]private TargetDetector detector;


    private string currentAnim=Constatnts.ANIM_IDE;
    private bool isMoving;
    private bool isAttacking;
    private bool hasShield;
    private float attackTimer;
    private float tmp;
    private bool isDead;
    public float Speed => speed;
    public bool IsMoving => isMoving;
    public bool IsAttacking => isAttacking;
    public bool HasTarget=> GetNearestTarget() !=null;
    public bool HasShield => hasShield;
    public bool IsDead=>isDead;


    private readonly List<Character> targets = new List<Character>();
    private readonly List<ActiveEffect> activeEffects= new List<ActiveEffect>();
    private readonly float [] statMultipliers = new float[(int)StatType.Count];
    private float CurrentSpeed=>speed*statMultipliers[(int)StatType.MoveSpeed];
    private float CurrentAttackSpeed=>attackSpeed*statMultipliers[(int)StatType.AttackSpeed];
    private float CurrentAttackRange=>attackRange*statMultipliers[(int)StatType.AttackRange];
    
    private struct ActiveEffect
    {
        public TimedBoosterEffect effect;
        public float remmaining;
    }
    protected virtual void Awake()
    {
        tmp = Constatnts.MOVE_THRESHOLD*Constatnts.MOVE_THRESHOLD;
        CharacterRegistry.Register(coll,this);
        //ClearEffects();
    }
    public abstract void Move();
    public virtual void OnInit()
    {
        isDead=false;
        rb.isKinematic=false;
        coll.enabled=true;
        CancelInvoke();
        targets.Clear();
        ClearEffects();
        attackTimer = 1/CurrentAttackSpeed;
        isMoving = false;
        isAttacking=false;
        ChangeAnim(Constatnts.ANIM_IDE);
        weaponHand.SetVisible(true);
        
    }
    protected virtual void OnUpdate(){}
    void Update()
    {
        if(isDead) return;
        UpdateEffects();
        OnUpdate();
        attackTimer -= Time.deltaTime;
        if(attackTimer<=0f) Attack();
    }
    void FixedUpdate()
    {
        if(isDead) return;
        Move();
    }
    public void Attack()
    {
        if(isMoving) return;
        Character target = GetNearestTarget();
        if(target==null) return;
        LookAtTarget(target);
        attackTimer = 1f / CurrentAttackSpeed;
        isAttacking = true;
        ChangeAnim(Constatnts.ANIM_ATTACK);
    }
    public void Throw()
    {
        if(isMoving) return;
        Bullet b = HBPools.Spawn<Bullet>(weaponHand.poolType,throwPoint.position,throwPoint.rotation);
        b.OnInit(throwPoint.position,CurrentAttackRange,this);
        weaponHand.SetVisible(false);
    }
    protected void ChangeAnim(string s)
    {
        if(currentAnim==s) return;
        anim.SetBool(currentAnim,false);
        currentAnim =s;
        anim.SetBool(currentAnim,true);
    }
    protected void MoveByDirection(Vector3 d)
    {
        isMoving = d.sqrMagnitude > tmp;
        if (isMoving == false)
        {
            if(!isAttacking) ChangeAnim(Constatnts.ANIM_IDE);
            return;
        }
        EndAttack();
        Quaternion targetRotation = Quaternion.LookRotation(d);
        Quaternion newRotation = Quaternion.Slerp(rb.rotation,targetRotation,roateSpeed*Time.fixedDeltaTime);
        rb.MoveRotation(newRotation);
        rb.MovePosition(rb.position+d*Time.fixedDeltaTime*CurrentSpeed);
        ChangeAnim(Constatnts.ANIM_RUN);
    }
    public void OnAttackEnd()=>EndAttack();
    private void EndAttack()
    {
        if(isAttacking==false) return;
        isAttacking= false;
        weaponHand.SetVisible(true);
    }
    private void RefreshAttackRange()
    {
        detector.SetRange(CurrentAttackRange);
        OnAttackRangeCircleChange(CurrentAttackRange);
    }
    protected virtual void OnAttackRangeCircleChange(float val){}
    public void AddTarget(Character target)
    {
        if(targets.Contains(target)) return;
        targets.Add(target);
    }
    public void RemoveTarget(Character target)
    {
        targets.Remove(target);
    }
    public Character GetNearestTarget()
    {
        Character res = null;
        float minDis = float.MaxValue;
        Vector3 currentPos = TF.position; 
        for(int i= targets.Count-1; i>=0; i--)
        {
            Character tmp = targets[i];
            if (tmp.IsDead)
            {
                targets.RemoveAt(i);
                continue;
            }
            float sqrDis = (tmp.TF.position-currentPos).sqrMagnitude;
            if (sqrDis <= minDis)
            {
                minDis=sqrDis;
                res=tmp;
            }
        }
        return res;
    }
    private void LookAtTarget(Character target)
    {
        Vector3 dir = target.TF.position-TF.position;
        dir.y=0f;
        if(dir.sqrMagnitude<tmp) return;
        rb.rotation = Quaternion.LookRotation(dir);
    }
    public void OnHit(Character c)
    {
        if(isDead) return;
        if (hasShield)
        {
            hasShield=false;
            return;
        }
        OnDeath();
    }
    protected virtual void OnDeath()
    {
        isDead=true;
        coll.enabled=false;
        rb.isKinematic=true;
        ChangeAnim(Constatnts.ANIM_DEAD);
    }
    public bool AddTimedEffect(TimedBoosterEffect effect,float duration)
    {
        if(isDead) return false;
        for(int i = 0; i < activeEffects.Count; i++)
        {
            if(activeEffects[i].effect !=effect) continue;
            activeEffects[i] = new ActiveEffect{effect=effect,remmaining=duration};
            return true;
        }
        activeEffects.Add(new ActiveEffect{effect=effect,remmaining=duration});
        effect.OnBegin(this);
        return true;
    }
    public void SetShield(bool check)=> hasShield=check;
    public void SetStatMultiplier(StatType type,float multiplier)
    {
        statMultipliers[(int)type]=multiplier;
        if(type==StatType.AttackRange) RefreshAttackRange();
    }
    private void UpdateEffects()
    {
        for(int i = activeEffects.Count - 1; i >= 0; i--)
        {
            ActiveEffect ae = activeEffects[i];
            ae.remmaining -= Time.deltaTime;
            if(ae.remmaining <= 0f)
            {
                activeEffects.RemoveAt(i);
                ae.effect.OnEnd(this);
            }
            else
            {
                activeEffects[i] = ae;
            }
        }
    }
    private void ClearEffects()
    {
        for(int i= activeEffects.Count-1;i>=0;i--) activeEffects[i].effect.OnEnd(this);
        activeEffects.Clear();
        hasShield = false;
        for(int i=0;i<statMultipliers.Length;i++) statMultipliers[i]=1f;
        RefreshAttackRange();
    }
}