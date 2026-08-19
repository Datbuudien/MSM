using UnityEngine;
public class WeaponHand : MonoBehaviour
{
    [SerializeField] private PoolType pt;
    public PoolType poolType =>pt;
}