using UnityEngine;

public abstract class Character : GameUnit
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField]private float speed=.5f;
    [SerializeField]private float roateSpeed =.5f;
    [SerializeField] private Transform throwPoint;
    [SerializeField]private WeaponHand weaponHand;
    [SerializeField]private float attackSpeed =1f;
    [SerializeField]private float attackRange =1f;
    private string currentAnim;
    private bool isMoving;
    private bool isAttacking;
    private float attackTimer;
    private float tmp;
    public float Speed => speed;
    public bool IsMoving => isMoving;
    public bool IsAttacking => isAttacking;
    protected virtual void Awake()
    {
        tmp = Constatnts.MOVE_THRESHOLD*Constatnts.MOVE_THRESHOLD;
        attackTimer = 1/attackSpeed;
    }
    public abstract void Move();
    public abstract void OnInit();
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
        attackTimer = 1f / attackSpeed;
        isAttacking = true;
        ChangeAnim(Constatnts.ANIM_ATTACK);
    }
    public void Throw()
    {
        if(isMoving) return;
        Bullet b = HBPools.Spawn<Bullet>(weaponHand.poolType,throwPoint.position,throwPoint.rotation);
        b.OnInit(throwPoint.position,attackRange,TF);
    }
    protected void ChangeAnim(string s)
    {
        if(currentAnim==s) return;
        if(currentAnim != "" && currentAnim != null) anim.SetBool(currentAnim,false);
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
        isAttacking =false;
        Quaternion targetRotation = Quaternion.LookRotation(d);
        Quaternion newRotation = Quaternion.Slerp(rb.rotation,targetRotation,roateSpeed*Time.fixedDeltaTime);
        rb.MoveRotation(newRotation);
        rb.MovePosition(rb.position+d*Time.fixedDeltaTime*speed);
        ChangeAnim(Constatnts.ANIM_RUN);
    }
    public void OnAttackEnd()=>isAttacking= false;
}