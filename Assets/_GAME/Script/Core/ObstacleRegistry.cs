using System.Collections.Generic;
using UnityEngine;
public static class ObstacleRegistry
{
    private static readonly Dictionary<Collider, Obstacle> OBSTACLES = new Dictionary<Collider, Obstacle>();
    public static void Register(Collider c,Obstacle o) =>OBSTACLES[c]=o;
    public static bool TryGet(Collider c,out Obstacle o)=>OBSTACLES.TryGetValue(c,out o);

}