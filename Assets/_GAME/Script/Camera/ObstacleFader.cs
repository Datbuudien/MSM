using System.Collections.Generic;
using UnityEngine;
public class ObstacleFader : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField]private float targetHeight=1.2f;
    private readonly RaycastHit[] hits=new RaycastHit[8];
    private readonly List<Obstacle> blocking = new List<Obstacle>();
    private readonly List<Obstacle>previous = new List<Obstacle>();
    void LateUpdate()
    {
        blocking.Clear();
        Vector3 origin=target.position+Vector3.up*targetHeight;
        Vector3 dir = transform.position-origin;
        float dis=dir.magnitude;
        if(dis<.01f) return;
        int count = Physics.RaycastNonAlloc(origin,dir/dis,hits,dis,obstacleLayer);
        for(int i = 0; i < count; i++)
        {
            if(ObstacleRegistry.TryGet(hits[i].collider,out Obstacle o)==false) continue;
            blocking.Add(o);
            o.SetTransparent(true);
        }
        for(int i = 0; i < previous.Count; i++)
        {
            if(blocking.Contains(previous[i])) continue;
            previous[i].SetTransparent(false);
        }
        previous.Clear();
        previous.AddRange(blocking);
    }

}