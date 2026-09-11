using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class IconRenderer
{
    private const int SIZE = 256;
    private const float PADDING = 1.12f;
    private const float FAR_AWAY = 5000f;
    private const string OUT_DIR = "Assets/_GAME/Sprites/Icons";
    private const string PREFAB_DIR = "Assets/_GAME/Prefab";
    private const string PANTS_PREFAB = "Assets/_GAME/Prefab/Character/Pants.prefab";

    private static readonly Vector3 VIEW_DIR = new Vector3(1f, .55f, -1.4f);

    private class Job
    {
        public GameObject Prefab;
        public Material Mat;
        public string Path;
        public System.Action<Sprite> Assign;
    }

    [MenuItem("Tools/Render Item Icons")]
    private static void RenderAll()
    {
        GameObject playerSource = SpawnPlayerSource();
        List<Job> jobs = new List<Job>();
        List<Job> done = new List<Job>();
        GameObject rig = null;
        try
        {
            CollectHats(jobs);
            CollectAccessories(jobs);
            CollectWeapons(jobs, playerSource);
            CollectPants(jobs);
            if (jobs.Count == 0)
            {
                Debug.LogWarning("Khong tim thay mon nao de render. Kiem tra HatData/WeaponData da dien Prefab chua.");
                return;
            }

            Directory.CreateDirectory(OUT_DIR);
            rig = BuildRig(out Camera cam);
            for (int i = 0; i < jobs.Count; i++)
            {
                EditorUtility.DisplayProgressBar("Render icon", jobs[i].Path, (float)i / jobs.Count);
                if (Shoot(cam, jobs[i])) done.Add(jobs[i]);
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            if (rig != null) Object.DestroyImmediate(rig);
            if (playerSource != null) Object.DestroyImmediate(playerSource);
        }

        AssetDatabase.Refresh();
        for (int i = 0; i < done.Count; i++) ApplyImportSettings(done[i].Path);
        AssetDatabase.Refresh();
        for (int i = 0; i < done.Count; i++)
        {
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(done[i].Path);
            if (sprite != null) done[i].Assign(sprite);
        }
        AssetDatabase.SaveAssets();
        Debug.Log($"Da render {done.Count}/{jobs.Count} icon vao {OUT_DIR} va dien vao ScriptableObject.");
    }


    private static void CollectHats(List<Job> jobs)
    {
        HatData data = LoadFirst<HatData>();
        if (data == null) return;
        foreach (HatItem item in data.Items)
        {
            if (item.Prefab == null) continue;
            HatItem captured = item;
            jobs.Add(new Job
            {
                Prefab = item.Prefab,
                Path = $"{OUT_DIR}/Ico_Hat_{item.Type}.png",
                Assign = s => { captured.Icon = s; EditorUtility.SetDirty(data); }
            });
        }
    }

    private static void CollectAccessories(List<Job> jobs)
    {
        AccessoryData data = LoadFirst<AccessoryData>();
        if (data == null) return;
        foreach (AccessoryItem item in data.Items)
        {
            if (item.Prefab == null) continue;
            AccessoryItem captured = item;
            jobs.Add(new Job
            {
                Prefab = item.Prefab,
                Path = $"{OUT_DIR}/Ico_Acce_{item.Type}.png",
                Assign = s => { captured.Icon = s; EditorUtility.SetDirty(data); }
            });
        }
    }

    private static void CollectPants(List<Job> jobs)
    {
        PantData data = LoadFirst<PantData>();
        if (data == null) return;
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(PANTS_PREFAB);
        if (prefab == null)
        {
            Debug.LogWarning($"Khong tim thay {PANTS_PREFAB} — bo qua icon quan.");
            return;
        }
        foreach (PantItem item in data.Items)
        {
            if (item.Mat == null) continue;
            PantItem captured = item;
            jobs.Add(new Job
            {
                Prefab = prefab,
                Mat = item.Mat,
                Path = $"{OUT_DIR}/Ico_Pant_{item.Type}.png",
                Assign = s => { captured.Icon = s; EditorUtility.SetDirty(data); }
            });
        }
    }

    private static void CollectWeapons(List<Job> jobs, GameObject playerSource)
    {
        WeaponData data = LoadFirst<WeaponData>();
        if (data == null || playerSource == null) return;
        Dictionary<PoolType, GameObject> hands = LoadHandWeapons(playerSource);
        foreach (WeaponItem item in data.Items)
        {
            if (hands.TryGetValue(item.BulletPool, out GameObject prefab) == false) continue;
            WeaponItem captured = item;
            jobs.Add(new Job
            {
                Prefab = prefab,
                Mat = item.Mat,
                Path = $"{OUT_DIR}/Ico_Weapon_{item.Type}.png",
                Assign = s => { captured.Icon = s; EditorUtility.SetDirty(data); }
            });
        }
    }

    private static Dictionary<PoolType, GameObject> LoadHandWeapons(GameObject playerSource)
    {
        Dictionary<PoolType, GameObject> map = new Dictionary<PoolType, GameObject>();
        WeaponHand[] hands = playerSource.GetComponentsInChildren<WeaponHand>(true);
        for (int i = 0; i < hands.Length; i++)
        {
            if (map.ContainsKey(hands[i].poolType)) continue;
            map[hands[i].poolType] = hands[i].gameObject;
        }
        return map;
    }

    private static GameObject SpawnPlayerSource()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { PREFAB_DIR });
        for (int i = 0; i < guids.Length; i++)
        {
            GameObject asset = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guids[i]));
            if (asset == null || asset.GetComponent<Player>() == null) continue;
            GameObject instance = Object.Instantiate(asset);
            instance.hideFlags = HideFlags.HideAndDontSave;
            instance.transform.position = Vector3.up * FAR_AWAY;
            instance.SetActive(false);
            return instance;
        }
        Debug.LogWarning($"Khong tim thay prefab co component Player trong {PREFAB_DIR} — bo qua icon vu khi.");
        return null;
    }


    private static GameObject BuildRig(out Camera cam)
    {
        GameObject root = new GameObject("__IconRig") { hideFlags = HideFlags.HideAndDontSave };
        root.transform.position = Vector3.up * FAR_AWAY;

        GameObject camGo = new GameObject("Cam");
        camGo.transform.SetParent(root.transform, false);
        cam = camGo.AddComponent<Camera>();
        cam.orthographic = true;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0f, 0f, 0f, 0f);
        cam.nearClipPlane = .01f;
        cam.enabled = false;

        AddLight(root.transform, Quaternion.Euler(35f, -35f, 0f), 1.1f);
        AddLight(root.transform, Quaternion.Euler(15f, 155f, 0f), .45f);
        return root;
    }

    private static void AddLight(Transform parent, Quaternion rotation, float intensity)
    {
        GameObject go = new GameObject("Light");
        go.transform.SetParent(parent, false);
        go.transform.rotation = rotation;
        Light light = go.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = intensity;
    }

    private static bool Shoot(Camera cam, Job job)
    {
        GameObject model = Object.Instantiate(job.Prefab);
        model.hideFlags = HideFlags.HideAndDontSave;
        model.transform.position = Vector3.up * FAR_AWAY;
        model.transform.localScale = Vector3.one;
        Transform[] all = model.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < all.Length; i++) all[i].gameObject.SetActive(true);
        if (job.Mat != null) SetMaterial(model, job.Mat);

        Renderer[] renders = model.GetComponentsInChildren<Renderer>(true);
        bool ok = renders.Length > 0;
        if (ok)
        {
            Frame(cam, renders);
            SaveShot(cam, job.Path);
        }
        Object.DestroyImmediate(model);
        return ok;
    }

    private static void SetMaterial(GameObject model, Material mat)
    {
        Renderer[] renders = model.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            Material[] slots = new Material[renders[i].sharedMaterials.Length];
            for (int s = 0; s < slots.Length; s++) slots[s] = mat;
            renders[i].sharedMaterials = slots;
        }
    }

    private static void Frame(Camera cam, Renderer[] renders)
    {
        Bounds b = renders[0].bounds;
        for (int i = 1; i < renders.Length; i++) b.Encapsulate(renders[i].bounds);

        Vector3 dir = VIEW_DIR.normalized;
        float back = b.extents.magnitude * 4f + 1f;
        cam.transform.rotation = Quaternion.LookRotation(-dir, Vector3.up);
        cam.transform.position = b.center + dir * back;
        cam.farClipPlane = back * 3f;

        float half = .0001f;
        for (int i = 0; i < 8; i++)
        {
            Vector3 corner = b.center + Vector3.Scale(b.extents,
                new Vector3((i & 1) == 0 ? -1f : 1f, (i & 2) == 0 ? -1f : 1f, (i & 4) == 0 ? -1f : 1f));
            Vector3 local = cam.transform.InverseTransformPoint(corner);
            half = Mathf.Max(half, Mathf.Abs(local.x), Mathf.Abs(local.y));
        }
        cam.orthographicSize = half * PADDING;
    }

    private static void SaveShot(Camera cam, string path)
    {
        RenderTexture rt = RenderTexture.GetTemporary(SIZE, SIZE, 24, RenderTextureFormat.ARGB32);
        RenderTexture previous = RenderTexture.active;
        cam.targetTexture = rt;

        RenderPipeline.StandardRequest request = new RenderPipeline.StandardRequest { destination = rt };
        if (RenderPipeline.SupportsRenderRequest(cam, request)) RenderPipeline.SubmitRenderRequest(cam, request);
        else cam.Render();

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(SIZE, SIZE, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0f, 0f, SIZE, SIZE), 0, 0);
        tex.Apply();
        File.WriteAllBytes(path, tex.EncodeToPNG());

        cam.targetTexture = null;
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);
        Object.DestroyImmediate(tex);
    }


    private static void ApplyImportSettings(string path)
    {
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null) return;
        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Single;
        importer.alphaIsTransparency = true;
        importer.mipmapEnabled = false;
        importer.SaveAndReimport();
    }

    private static T LoadFirst<T>() where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        if (guids.Length == 0)
        {
            Debug.LogWarning($"Khong tim thay asset {typeof(T).Name}");
            return null;
        }
        return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guids[0]));
    }
}
