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
    [SerializeField] private Range range;
    [SerializeField]private TargetDetector detector;

    private string currentAnim;
    private bool isMoving;
    private bool isAttacking;
    private float attackTimer;
    private float tmp;
    public float Speed => speed;
    public bool IsMoving => isMoving;
    public bool IsAttacking => isAttacking;
    public bool HasTarget=> GetNearestTarget() !=null;
    private readonly List<Character> targets = new List<Character>();
    protected virtual void Awake()
    {
        tmp = Constatnts.MOVE_THRESHOLD*Constatnts.MOVE_THRESHOLD;
        SetAttackRange(attackRange);
        CharacterRegistry.Register(coll,this);
    }
    public abstract void Move();
    public virtual void OnInit()
    {
        targets.Clear();
        attackTimer = 1/attackSpeed;
        isMoving = false;
        isAttacking=false;
        ChangeAnim(Constatnts.ANIM_IDE);
        weaponHand.SetVisible(true);
    }
    void Update()
    {
        attackTimer -= Time.deltaTime;
        if(attackTimer<=0f) Attack();
    }
    void FixedUpdate()
    {
        Move();
    }
    public void Attack()
    {
        if(isMoving) return;
        Character target = GetNearestTarget();
        if(target==null) return;
        LookAtTarget(target);
        attackTimer = 1f / attackSpeed;
        isAttacking = true;
        ChangeAnim(Constatnts.ANIM_ATTACK);
    }
    public void Throw()
    {
        if(isMoving) return;
        Bullet b = HBPools.Spawn<Bullet>(weaponHand.poolType,throwPoint.position,throwPoint.rotation);
        b.OnInit(throwPoint.position,attackRange,TF);
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
        rb.MovePosition(rb.position+d*Time.fixedDeltaTime*speed);
        ChangeAnim(Constatnts.ANIM_RUN);
    }
    public void OnAttackEnd()=>EndAttack();
    private void EndAttack()
    {
        if(isAttacking==false) return;
        isAttacking= false;
        weaponHand.SetVisible(true);
    }
    public void SetAttackRange(float val)
    {
        attackRange = val;
        range.SetRange(val);
    }
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
            if (tmp.gameObject.activeSelf == false)
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
}