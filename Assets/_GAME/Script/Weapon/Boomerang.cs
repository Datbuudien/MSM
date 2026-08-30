using System;
using UnityEngine;
public class Boomerang: Bullet
{
    [SerializeField] private float returnDelay = 1f;
    private float timer;
    private bool isReturning;
    public override void OnInit(Vector3 pos,float range, Character owner)
    {
        base.OnInit(pos,range,owner);
        timer = 0f;
        isReturning = false;
    }
    protected override void Move()
    {
        timer += Time.deltaTime;
        if(timer>=returnDelay) isReturning = true;
        Vector3 d = isReturning? (owner.TF.position-TF.position).normalized : TF.forward;
        TF.position += d*Time.deltaTime*Speed;
    }
    protected override bool IsFinished()
    {
        return isReturning&&(owner.TF.position-TF.position).sqrMagnitude<.05f;
    }
    
}