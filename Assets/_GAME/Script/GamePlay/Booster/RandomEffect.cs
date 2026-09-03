using UnityEngine;

[CreateAssetMenu(fileName = "Random", menuName = "Game/Booster/Random")]
public class RandomEffect:BoosterEffect
{
    [SerializeField] private BoosterEffect[] effects;
    public override bool TryApply(Character c)
    {
        int index = Random.Range(0,effects.Length);
        return effects[index].TryApply(c);
    }
}