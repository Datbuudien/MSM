using UnityEngine;

public abstract class Character : GameUnit
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator anim;
    [SerializeField]private float speed=.5f;
    [SerializeField]private float roateSpeed =.5f;
    private string currentAnim;
    private bool isMoving;
    private float tmp;
    public float Speed => speed;
    public bool IsMoving => isMoving;
    protected virtual void Awake()
    {
        tmp = Constatnts.MOVE_THRESHOLD*Constatnts.MOVE_THRESHOLD;
    }
    public abstract void Move();
    public abstract void OnInit();
    void FixedUpdate()
    {
        Move();
    }
    public void Attack()
    {
        if(isMoving) return;
        ChangeAnim(Constatnts.ANIM_ATTACK);
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
            ChangeAnim(Constatnts.ANIM_IDE);
            return;
        }
        Quaternion targetRotation = Quaternion.LookRotation(d);
        Quaternion newRotation = Quaternion.Slerp(rb.rotation,targetRotation,roateSpeed*Time.fixedDeltaTime);
        rb.MoveRotation(newRotation);
        rb.MovePosition(rb.position+d*Time.fixedDeltaTime*speed);
        ChangeAnim(Constatnts.ANIM_RUN);
    }
}