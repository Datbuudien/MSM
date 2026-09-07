using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private UICanvas[] uiResources;
    [SerializeField] private Transform canvasParent;

    private readonly Dictionary<Type, UICanvas> uiCanvasPrefab = new Dictionary<Type, UICanvas>();
    private readonly Dictionary<Type, UICanvas> uiCanvas = new Dictionary<Type, UICanvas>();

    void LateUpdate()
    {
        UICanvas topUI = BackTopUI;
        if (topUI == null) return;
        // Input System moi: nut Back cung cua Android duoc map vao escapeKey
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;
        if (keyboard.escapeKey.wasPressedThisFrame == false) return;
        if (backActionEvents.TryGetValue(topUI, out UnityAction action)) action?.Invoke();
    }

    #region Canvas

    public T OpenUI<T>() where T : UICanvas
    {
        T canvas = GetUI<T>();
        if (canvas == null) return null;
        canvas.Setup();
        canvas.Open();
        return canvas;
    }
    public void CloseUI<T>() where T : UICanvas
    {
        if (IsOpened<T>()) uiCanvas[typeof(T)].CloseDirectly();
    }
    public void CloseUI<T>(float delayTime) where T : UICanvas
    {
        if (IsOpened<T>()) uiCanvas[typeof(T)].Close(delayTime);
    }
    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoaded<T>() && uiCanvas[typeof(T)].gameObject.activeInHierarchy;
    }
    public bool IsLoaded<T>() where T : UICanvas
    {
        Type type = typeof(T);
        return uiCanvas.ContainsKey(type) && uiCanvas[type] != null;
    }
    public T GetUI<T>() where T : UICanvas
    {
        Type type = typeof(T);
        if (IsLoaded<T>() == false)
        {
            T prefab = GetUIPrefab<T>();
            if (prefab == null) return null;
            uiCanvas[type] = Instantiate(prefab, canvasParent == null ? transform : canvasParent);
        }
        return uiCanvas[type] as T;
    }
    public void CloseAll()
    {
        foreach (KeyValuePair<Type, UICanvas> item in uiCanvas)
        {
            if (item.Value != null && item.Value.gameObject.activeInHierarchy) item.Value.CloseDirectly();
        }
    }

    private T GetUIPrefab<T>() where T : UICanvas
    {
        Type type = typeof(T);
        if (uiCanvasPrefab.TryGetValue(type, out UICanvas cached)) return cached as T;
        // uiResources null khi UIManager bi Singleton tu tao vi khong co san trong scene
        for (int i = 0; uiResources != null && i < uiResources.Length; i++)
        {
            if (uiResources[i] is T == false) continue;
            uiCanvasPrefab[type] = uiResources[i];
            return uiResources[i] as T;
        }
#if UNITY_EDITOR
        Debug.LogError($"Chua keo prefab {type.Name} vao o Ui Resources cua UIManager");
#endif
        return null;
    }

    #endregion

    #region Back Button

    private readonly Dictionary<UICanvas, UnityAction> backActionEvents = new Dictionary<UICanvas, UnityAction>();
    private readonly List<UICanvas> backCanvas = new List<UICanvas>();

    private UICanvas BackTopUI => backCanvas.Count > 0 ? backCanvas[backCanvas.Count - 1] : null;

    public void PushBackAction(UICanvas canvas, UnityAction action)
    {
        if (backActionEvents.ContainsKey(canvas)) return;
        backActionEvents.Add(canvas, action);
    }
    public void AddBackUI(UICanvas canvas)
    {
        if (backCanvas.Contains(canvas)) return;
        backCanvas.Add(canvas);
    }
    public void RemoveBackUI(UICanvas canvas)
    {
        backCanvas.Remove(canvas);
    }
    public void ClearBackKey()
    {
        backCanvas.Clear();
    }

    #endregion
}
