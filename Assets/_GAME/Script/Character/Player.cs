using UnityEngine;
public class Player: Character
{
    void Start()
    {
        OnInit();
    }
    public override void Move()
    {
        MoveByDirection(InputManager.Ins.MoveDirection);
    }
    public override void OnInit()
    {
        ChangeAnim(Constatnts.ANIM_IDE);
        weaponHand.SetVisible(true);
    }
}