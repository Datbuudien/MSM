using UnityEngine;
public class TargetDetector : MonoBehaviour
{
    [SerializeField] private Character owner;
    [SerializeField]private SphereCollider zone;
    public void SetRange(float range) =>zone.radius=range;
    void OnTriggerEnter(Collider other)
    {
        Character ch;
        if(!CharacterRegistry.TryGet(other,out ch)) return;
        owner.AddTarget(ch);
    }
    void OnTriggerExit(Collider other)
    {
        if(CharacterRegistry.TryGet(other,out Character ch)==false) return;
        
    }
}