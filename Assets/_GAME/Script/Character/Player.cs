using UnityEngine;
public class Player: Character
{
    [SerializeField] private Range range;
    public override void Move()
    {
        MoveByDirection(InputManager.Ins.MoveDirection);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        LevelManager.Ins.OnPlayerDeath();
    }
    protected override void OnAttackRangeCircleChange(float val)
    {
        range.SetRange(val);
    }
}