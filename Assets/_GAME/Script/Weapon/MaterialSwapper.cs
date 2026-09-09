using UnityEngine;
// Doi material cho model nhieu slot (dao 2 slot, bua 2, boomerang 1...).
// Cache ban goc luc Awake de con duong ve "vu khi tran".
public class MaterialSwapper : MonoBehaviour
{
    [SerializeField] private Renderer[] renders;

    private Material[][] originals;
    private Material[][] slots;
    private Material current;
    public Material Current => current;

    void Awake()
    {
        originals = new Material[renders.Length][];
        slots = new Material[renders.Length][];
        for (int i = 0; i < renders.Length; i++)
        {
            originals[i] = renders[i].sharedMaterials;
            slots[i] = new Material[originals[i].Length];
        }
    }
    public void SetMaterial(Material mat)
    {
        if (current == mat) return;     // dan lien tiep cung skin: khong dung toi renderer
        current = mat;
        for (int i = 0; i < renders.Length; i++)
        {
            if (mat == null)
            {
                renders[i].sharedMaterials = originals[i];
                continue;
            }
            Material[] tmp = slots[i];
            for (int s = 0; s < tmp.Length; s++) tmp[s] = mat;
            renders[i].sharedMaterials = tmp;   // sharedMaterials: KHONG nhan ban instance (pitfall #1)
        }
    }
}
