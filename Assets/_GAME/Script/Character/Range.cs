using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AdaptivePerformance;
public class Range : MonoBehaviour
{
    [SerializeField]private Transform circle;
    public void SetRange(float range)=> circle.localScale= new Vector3(range*2f,range*2f,1f);
    public void SetVisible(bool check)=>circle.gameObject.SetActive(check);
}