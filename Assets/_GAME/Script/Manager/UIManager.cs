using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private UICanvas[] canvasPrefabs;

    private readonly Dictionary<Type, UICanvas> prefabs = new Dictionary<Type, UICanvas>();
    private readonly Dictionary<Type, UICanvas> actives = new Dictionary<Type, UICanvas>();
    private bool isMapBuilt;

    protected override void Awake()
    {
        base.Awake();
        if (IsDuplicate) return;
        BuildPrefabMap();
    }

    public T OpenUI<T>() where T : UICanvas
    {
        T canvas = GetUI<T>();
        if (canvas == null) return null;
        canvas.Open();
        return canvas;
    }
    public void CloseUI<T>(float delay = 0f) where T : UICanvas
    {
        if (actives.TryGetValue(typeof(T), out UICanvas canvas) == false) return;
        if (canvas == null) return;
        canvas.Close(delay);
    }
    public bool IsOpened<T>() where T : UICanvas
    {
        if (actives.TryGetValue(typeof(T), out UICanvas canvas) == false) return false;
        return canvas != null && canvas.gameObject.activeSelf;
    }
    public T GetUI<T>() where T : UICanvas
    {
        Type type = typeof(T);
        if (actives.TryGetValue(type, out UICanvas active) && active != null) return active as T;

        T prefab = GetPrefab<T>();
        if (prefab == null) return null;

        T instance = Instantiate(prefab, transform);
        instance.Setup();
        actives[type] = instance;
        return instance;
    }
    public void CloseAll()
    {
        foreach (UICanvas canvas in actives.Values)
        {
            if (canvas != null) canvas.CloseDirectly();
        }
    }
    public void ClearAll()
    {
        actives.Clear();
    }

    private T GetPrefab<T>() where T : UICanvas
    {
        BuildPrefabMap();
        Type type = typeof(T);
        if (prefabs.TryGetValue(type, out UICanvas prefab)) return prefab as T;
#if UNITY_EDITOR
        Debug.LogError($"Chua keo prefab {type.Name} vao o Canvas Prefabs cua UIManager");
#endif
        return null;
    }
    private void BuildPrefabMap()
    {
        if (isMapBuilt) return;
        isMapBuilt = true;
        for (int i = 0; i < canvasPrefabs.Length; i++)
        {
            if (canvasPrefabs[i] == null) continue;
            prefabs[canvasPrefabs[i].GetType()] = canvasPrefabs[i];
        }
    }
}
