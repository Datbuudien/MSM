using UnityEngine;
public class WeaponHand : MonoBehaviour
{
    [SerializeField] private PoolType pt;
    [SerializeField] private GameObject model;
    public PoolType poolType =>pt;
    public void SetVisible(bool c) => model.SetActive(c);
}