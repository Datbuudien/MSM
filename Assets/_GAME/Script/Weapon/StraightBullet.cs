using UnityEngine;
public class StraightBullet : Bullet
{
    protected override void Move()
    {
        TF.position += TF.forward * (Speed * Time.deltaTime);
    }
}