using UnityEngine;
public class Obstacle : MonoBehaviour
{
    [SerializeField] private Collider coll;
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material opaqueMat;
    [SerializeField] private Material transparentMat;
    private bool isTransparent=false;
    void Awake()
    {
        ObstacleRegistry.Register(coll,this);
    }
    public void SetTransparent(bool check)
    {
        if(isTransparent==check) return;
        isTransparent=check;
        Material mat= check?transparentMat:opaqueMat;
        for(int i=0;i<renderers.Length;i++) renderers[i].sharedMaterial=mat;
    }
}