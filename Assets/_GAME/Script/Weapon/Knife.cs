using UnityEngine;
public class Knife : Bullet
{
    protected override void Move()
    {
        TF.position += (-TF.up)*(Speed*Time.deltaTime);
    }
}