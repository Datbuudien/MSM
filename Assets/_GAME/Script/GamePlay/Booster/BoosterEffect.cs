using UnityEngine;
public abstract class BoosterEffect: ScriptableObject
{
    public abstract bool TryApply(Character c);
}