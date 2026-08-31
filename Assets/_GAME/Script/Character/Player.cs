using UnityEngine;
public class Player: Character
{
    public override void Move()
    {
        MoveByDirection(InputManager.Ins.MoveDirection);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        LevelManager.Ins.OnPlayerDeath();
    }
}