using UnityEngine;
public class Booster : MonoBehaviour
{
    [SerializeField] private BoosterEffect effect;
    void OnTriggerEnter(Collider other)
    {
        if(CharacterRegistry.TryGet(other,out Character c)==false) return;
        if(effect.TryApply(c)==false) return;
        gameObject.SetActive(false);
    }
}