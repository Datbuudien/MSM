using UnityEngine;
public class Player: Character
{
    [SerializeField] private Range range;
    private Character marked;

    public override bool IsSoundOwner => true;

    protected override void OnUpdate()
    {
        Character target = GetNearestTarget();
        if (marked != target && marked != null) marked.SetTargeted(false);
        marked = target;
        if (marked != null) marked.SetTargeted(true);
    }
    public override void OnInit()
    {
        base.OnInit();
        marked = null;
    }
    public override void Move()
    {
        MoveByDirection(InputManager.Ins.MoveDirection);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        if (marked != null) marked.SetTargeted(false);
        marked = null;
        LevelManager.Ins.OnPlayerDeath();
    }
    public override void OnKill()
    {
        int before = SizeLevel;
        base.OnKill();
        if (SizeLevel > before) SoundManager.Ins.PlaySfx(SfxType.SizeUp);
    }
    protected override void OnAttackRangeCircleChange(float val)
    {
        range.SetRange(val);
    }
    public void SetRangeVisible(bool check) => range.SetVisible(check);
}