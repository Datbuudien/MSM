using System.Collections.Generic;
using UnityEngine;
public static class CharacterRegistry
{
    private static readonly Dictionary<Collider,Character> CHARACTERS = new Dictionary<Collider, Character>();
    public static void Register(Collider c,Character ch)=>CHARACTERS[c]=ch;
    public static void Unregister(Collider c)=>CHARACTERS.Remove(c);
    public static bool TryGet(Collider c,out Character ch)=>CHARACTERS.TryGetValue(c,out ch);

    public static void PauseAll()
    {
        foreach (KeyValuePair<Collider,Character> pair in CHARACTERS)
            if (pair.Value != null) pair.Value.OnPause();
    }
    public static void ResumeAll()
    {
        foreach (KeyValuePair<Collider,Character> pair in CHARACTERS)
            if (pair.Value != null) pair.Value.OnResume();
    }
}
