#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public static class SamplePrefabGenerator
{
    private const string PrefabFolder = "Assets/Prefabs/Sample";

    public static void GenerateAll()
    {
        EnsureFolder();

        CreatePlayerPrefab();
        CreateEnemyPrefab("AggressiveEnemy", new Color(0.9f, 0.25f, 0.2f), "AggressiveEnemy");
        CreateEnemyPrefab("RangedEnemy", new Color(0.3f, 0.5f, 1f), "RangedEnemy");
        CreateEnemyPrefab("LoyaltyEnemy", new Color(0.6f, 0.3f, 0.9f), "LoyaltyEnemy");
        CreateProjectilePrefab();
        CreateBossPrefab();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Sample prefabs created in " + PrefabFolder);
    }

    private static void EnsureFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Prefabs"))
            AssetDatabase.CreateFolder("Assets", "Prefabs");

        if (!AssetDatabase.IsValidFolder(PrefabFolder))
            AssetDatabase.CreateFolder("Assets/Prefabs", "Sample");
    }

    private static void CreatePlayerPrefab()
    {
        var go = new GameObject("Player");
        go.tag = "Player";

        var sr = go.AddComponent<SpriteRenderer>();
        sr.color = new Color(1f, 0.85f, 0.3f);
        sr.sprite = CreateDefaultSprite();

        var rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        go.AddComponent<CapsuleCollider2D>();

        AddScript(go, "PlayerController");
        AddScript(go, "PlayerHealth");

        SavePrefab(go, "Player");
    }

    private static void CreateEnemyPrefab(string name, Color color, string scriptName)
    {
        var go = new GameObject(name);
        go.tag = "Enemy";

        var sr = go.AddComponent<SpriteRenderer>();
        sr.color = color;
        sr.sprite = CreateDefaultSprite();

        var rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        go.AddComponent<CircleCollider2D>();

        AddScript(go, scriptName);
        AddScript(go, "EnemyDeathHandler");

        SavePrefab(go, name);
    }

    private static void CreateProjectilePrefab()
    {
        var go = new GameObject("Projectile");

        var sr = go.AddComponent<SpriteRenderer>();
        sr.color = Color.cyan;
        sr.sprite = CreateDefaultSprite();
        go.transform.localScale = Vector3.one * 0.4f;

        var rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        var col = go.AddComponent<CircleCollider2D>();
        col.isTrigger = true;

        AddScript(go, "Projectile");

        SavePrefab(go, "Projectile");
    }

    private static void CreateBossPrefab()
    {
        var go = new GameObject("RealityStormBoss");
        go.tag = "Enemy";

        var sr = go.AddComponent<SpriteRenderer>();
        sr.color = new Color(0.8f, 0.2f, 1f);
        sr.sprite = CreateDefaultSprite();
        go.transform.localScale = Vector3.one * 2.2f;

        var rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        go.AddComponent<CircleCollider2D>();

        AddScript(go, "RealityStormBoss");
        AddScript(go, "EnemyDeathHandler");

        SavePrefab(go, "RealityStormBoss");
    }

    private static void AddScript(GameObject target, string className)
    {
        string[] guids = AssetDatabase.FindAssets(className + " t:MonoScript");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(path);
            if (script != null && script.GetClass() != null && script.GetClass().Name == className)
            {
                target.AddComponent(script.GetClass());
                return;
            }
        }
    }

    private static void SavePrefab(GameObject go, string name)
    {
        string path = PrefabFolder + "/" + name + ".prefab";
        PrefabUtility.SaveAsPrefabAsset(go, path);
        Object.DestroyImmediate(go);
    }

    private static Sprite CreateDefaultSprite()
    {
        // Create a simple white texture as placeholder
        Texture2D tex = new Texture2D(32, 32);
        Color[] pixels = new Color[32 * 32];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.white;
        tex.SetPixels(pixels);
        tex.Apply();

        return Sprite.Create(tex, new Rect(0, 0, 32, 32), new Vector2(0.5f, 0.5f), 32);
    }
}
#endif
