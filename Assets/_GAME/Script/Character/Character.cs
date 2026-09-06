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
    [SerializeField]private float attackSpeed =1f;
    [SerializeField]private float attackRange =1f;
    [SerializeField]private TargetDetector detector;
    [SerializeField]private WeaponHand[] weaponHands;
    [SerializeField] private Transform hatPoint;
    [SerializeField] private Transform accessoryPoint;
    [SerializeField] private SkinnedMeshRenderer pantRender;

    protected WeaponHand weaponHand;
    private string currentAnim=Constatnts.ANIM_IDE;
    private bool isMoving;
    private bool isAttacking;
    private bool hasShield;
    private float attackTimer;
    private float tmp;
    private bool isDead;
    public bool IsMoving => isMoving;
    public bool IsAttacking => isAttacking;
    public bool HasTarget=> GetNearestTarget() !=null;
    public bool HasShield => hasShield;
    public bool IsDead=>isDead;

    private int currentWeaponIndex;
    private readonly List<Character> targets = new List<Character>();
    private readonly List<ActiveEffect> activeEffects= new List<ActiveEffect>();
    private readonly float [] statMultipliers = new float[(int)StatType.Count]; // boots
    private float CurrentSpeed => GetStat(StatType.MoveSpeed, speed);
    private float CurrentAttackSpeed => GetStat(StatType.AttackSpeed, attackSpeed);
    public float CurrentAttackRange => GetStat(StatType.AttackRange, attackRange);
    private readonly float [] equipAdd = new float [(int)StatType.Count];
    private readonly float [] equipMul = new float [(int)StatType.Count];
    private GameObject currentHat;
    private GameObject currentAccessory;
    private HatItem currentHatItem;
    private PantItem currentPantItem;
    private AccessoryItem currentAccessoryItem;
    private WeaponItem currentWeaponItem;
    private struct ActiveEffect
    {
        public TimedBoosterEffect effect;
        public float remaining;
    }
    protected virtual void Awake()
    {
        tmp = Constatnts.MOVE_THRESHOLD*Constatnts.MOVE_THRESHOLD;
        CharacterRegistry.Register(coll,this);
        for (int i = 0; i < (int)StatType.Count; i++)
        {
            statMultipliers[i] = 1f;
            equipMul[i] = 1f;
        }
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
        SetWeapon(0);
        
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
            activeEffects[i] = new ActiveEffect{effect=effect,remaining=duration};
            return true;
        }
        activeEffects.Add(new ActiveEffect{effect=effect,remaining=duration});
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
            ae.remaining -= Time.deltaTime;
            if(ae.remaining <= 0f)
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
    public bool ChangeRandomWeapon()
    {
        if(isDead) return false;
        if(weaponHands.Length <2) return false;
        int index = Random.Range(0,weaponHands.Length);
        if(index==currentWeaponIndex) index = (index+1)%weaponHands.Length;
        SetWeapon(index);
        return true;
    }
    private void SetWeapon(int index)
    {
        for(int i=0;i<weaponHands.Length;i++) weaponHands[i].gameObject.SetActive(i==index);
        currentWeaponIndex=index;
        weaponHand=weaponHands[index];
        weaponHand.SetVisible(true);
    }
    private void ApplyBonuses<T>(ShopItemData<T> item)  where T : System.Enum
    {
        if(item==null || item.Bonuses ==null) return;
        for(int i = 0; i < item.Bonuses.Length; i++)
        {
            StatBonus bonus = item.Bonuses[i];
            int index = (int) bonus.Stat;
            equipAdd[index] +=bonus.FlatBonus;
            equipMul[index] += bonus.PercentBonus;
        }
    }
    private void RecalculateEquipStats()
    {
        for(int i = 0; i < equipAdd.Length; i++)
        {
            equipAdd[i]=0f;
            equipMul[i]=1f;
        }
        ApplyBonuses(currentHatItem);
        ApplyBonuses(currentAccessoryItem);
        ApplyBonuses(currentPantItem);
        ApplyBonuses(currentWeaponItem);
        RefreshAttackRange();
    }
    public bool ChangeWeapon(WeaponType type)
    {
        WeaponItem item = DataManager.Ins.WeaponData.GetItem(type);
        if(item==null) return false;
        for(int i = 0; i < weaponHands.Length; i++)
        {
            if(weaponHands[i].poolType != item.BulletPool) continue;
            currentWeaponItem = item;
            SetWeapon(i);
            RecalculateEquipStats();
            return true;
        }
        return false;
    }
    public void ChangeHat(HatType type)
    {
        if(currentHat!=null) Destroy(currentHat);
        currentHat = null;
        currentHatItem = DataManager.Ins.HatData.GetItem(type);
        RecalculateEquipStats();
        if(currentHatItem==null || currentHatItem.Prefab==null) return;
        currentHat=Instantiate(currentHatItem.Prefab,hatPoint,false);
    }
    public void ChangeAccessory(AccessoryType type)
    {
        if(currentAccessory != null) Destroy(currentAccessory);
        currentAccessory = null;
        currentAccessoryItem = DataManager.Ins.AccessoryData.GetItem(type);
        RecalculateEquipStats();
        if(currentAccessoryItem==null || currentAccessoryItem.Prefab==null) return;
        currentAccessory=Instantiate(currentAccessoryItem.Prefab,accessoryPoint,false);
    }
    public void ChangePant(PantType type)
    {
        currentPantItem = DataManager.Ins.PantData.GetItem(type);
        RecalculateEquipStats();
        if(currentPantItem==null || currentPantItem.Mat==null) return;
        pantRender.sharedMaterial = currentPantItem.Mat;
    }
    private float GetStat(StatType stat,float baseValue)
    {
        int i= (int) stat;
        return (baseValue+equipAdd[i])*equipMul[i]*statMultipliers[i];    // chi so cong them duoc tinh theo ct x= (x+add)*multi*boost
    }
}