using UnityEngine;
public abstract class TimedBoosterEffect: BoosterEffect
{
    [SerializeField] private float duration = 25f;
    public override bool TryApply(Character c) => c.AddTimedEffect(this, duration);

    public abstract void OnBegin(Character c);
    public abstract void OnEnd(Character c);
}