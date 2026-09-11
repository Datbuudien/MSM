using UnityEngine;
public class WeaponHand : MonoBehaviour
{
    [SerializeField] private PoolType pt;
    [SerializeField] private GameObject model;
    [SerializeField] private MaterialSwapper swapper;
    public PoolType poolType =>pt;
    public Material CurrentMaterial => swapper==null ? null : swapper.Current;
    public void SetVisible(bool c) => model.SetActive(c);
    public void SetMaterial(Material mat)
    {
        if(swapper==null) return;
        swapper.SetMaterial(mat);
    }
}