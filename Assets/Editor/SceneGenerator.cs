#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;
using System.Reflection;

public class SceneGenerator : EditorWindow
{
    [MenuItem("Shadowrift/Generate All Scenes")]
    public static void ShowWindow()
    {
        GetWindow<SceneGenerator>("Scene Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Shadowrift Chronicles - Scene Generator", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "This tool creates all required scenes and automatically attaches the correct scripts.\n\n" +
            "Make sure all scripts have finished compiling before running.", 
            MessageType.Info);

        GUILayout.Space(15);

        if (GUILayout.Button("Generate All Scenes + Attach Scripts", GUILayout.Height(45)))
        {
            GenerateAllScenes();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Main Menu Only"))
        {
            CreateMainMenuScene();
            EditorUtility.DisplayDialog("Done", "MainMenu scene created and configured!", "OK");
        }

        if (GUILayout.Button("Generate All Realm Scenes"))
        {
            CreateRealmScenes();
            EditorUtility.DisplayDialog("Done", "All Realm scenes created and configured!", "OK");
        }
    }

    private static void GenerateAllScenes()
    {
        CreateMainMenuScene();
        CreateRealmScenes();
        AddScenesToBuildSettings();

        EditorUtility.DisplayDialog("Success",
            "All scenes generated successfully!\n\n" +
            "✓ MainMenu\n" +
            "✓ Realm_01_NeoArcadia\n" +
            "✓ Realm_02_Wildlands\n" +
            "✓ Realm_03_Clockwork\n" +
            "✓ Realm_04_BoneDesert\n" +
            "✓ Realm_05_CrystalSanctum\n" +
            "✓ Realm_06_RealityStorm\n\n" +
            "Scripts have been automatically attached where possible.\n" +
            "Scenes added to Build Settings.", "OK");
    }

    private static void CreateMainMenuScene()
    {
        EnsureScenesFolder();
        string path = "Assets/Scenes/MainMenu.unity";

        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Canvas
        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasGO.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // Event System
        new GameObject("EventSystem", 
            typeof(UnityEngine.EventSystems.EventSystem), 
            typeof(UnityEngine.EventSystems.StandaloneInputModule));

        // Main Menu Controller
        var controllerGO = new GameObject("MainMenuController");
        AddScriptIfExists(controllerGO, "MainMenuController");

        // Title
        CreateUIText(canvasGO.transform, "Title", "SHADOWRIFT CHRONICLES", 52, new Vector2(0, 220));

        // Buttons
        var newGameBtn = CreateUIButton(canvasGO.transform, "NewGameButton", "New Game", new Vector2(0, 60));
        var continueBtn = CreateUIButton(canvasGO.transform, "ContinueButton", "Continue", new Vector2(0, -20));
        var quitBtn = CreateUIButton(canvasGO.transform, "QuitButton", "Quit", new Vector2(0, -100));

        EditorSceneManager.SaveScene(scene, path);
        Debug.Log("Created + configured: " + path);
    }

    private static void CreateRealmScenes()
    {
        string[] realmNames = {
            "Realm_01_NeoArcadia",
            "Realm_02_Wildlands",
            "Realm_03_Clockwork",
            "Realm_04_BoneDesert",
            "Realm_05_CrystalSanctum",
            "Realm_06_RealityStorm"
        };

        Color[] backgroundColors = {
            new Color(0.08f, 0.12f, 0.28f),  // Neo-Arcadia
            new Color(0.04f, 0.18f, 0.09f),  // Wildlands
            new Color(0.18f, 0.18f, 0.22f),  // Clockwork
            new Color(0.22f, 0.15f, 0.08f),  // Bone Desert
            new Color(0.12f, 0.08f, 0.28f),  // Crystal Sanctum
            new Color(0.28f, 0.04f, 0.32f)   // Reality Storm
        };

        EnsureScenesFolder();

        for (int i = 0; i < realmNames.Length; i++)
        {
            CreateSingleRealmScene(realmNames[i], backgroundColors[i], i == 5);
        }
    }

    private static void CreateSingleRealmScene(string sceneName, Color bgColor, bool isFinalRealm)
    {
        string path = "Assets/Scenes/" + sceneName + ".unity";
        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Camera setup
        Camera.main.backgroundColor = bgColor;
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 5.5f;

        // ========== GameSystems ==========
        var systems = new GameObject("GameSystems");

        // Attach core scripts
        AddScriptIfExists(systems, "GameManager");
        AddScriptIfExists(systems, "PhasingManager");
        AddScriptIfExists(systems, "StanceManager");
        AddScriptIfExists(systems, "LoyaltyManager");
        AddScriptIfExists(systems, "ScoreManager");
        AddScriptIfExists(systems, "AudioManager");
        AddScriptIfExists(systems, "RealmLoader");
        AddScriptIfExists(systems, "RealmProgression");

        // ========== Layers ==========
        var baseLayer = new GameObject("BaseLayer");
        var phaseLayer = new GameObject("PhaseLayer");
        phaseLayer.SetActive(false);

        // ========== Player ==========
        var player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<SpriteRenderer>();
        var rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.AddComponent<CapsuleCollider2D>();
        player.transform.position = Vector3.zero;

        AddScriptIfExists(player, "PlayerController");
        AddScriptIfExists(player, "PlayerHealth");

        // ========== Enemy Spawner ==========
        var spawner = new GameObject("EnemySpawner");
        AddScriptIfExists(spawner, "EnemySpawner");

        for (int i = 0; i < 6; i++)
        {
            var point = new GameObject("SpawnPoint_" + i);
            point.transform.SetParent(spawner.transform);
            float x = Mathf.Cos(i * Mathf.PI * 2f / 6f) * 6f;
            float y = Mathf.Sin(i * Mathf.PI * 2f / 6f) * 3.5f;
            point.transform.position = new Vector3(x, y, 0);
        }

        // ========== UI ==========
        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasGO.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        new GameObject("EventSystem",
            typeof(UnityEngine.EventSystems.EventSystem),
            typeof(UnityEngine.EventSystems.StandaloneInputModule));

        // UI Manager
        var uiManagerGO = new GameObject("UIManager");
        uiManagerGO.transform.SetParent(canvasGO.transform);
        AddScriptIfExists(uiManagerGO, "UIManager");

        CreateUIText(canvasGO.transform, "LoyaltyText", "Loyalty: 65", 22, new Vector2(-700, 450));
        CreateUIText(canvasGO.transform, "ScoreText", "Score: 0", 22, new Vector2(700, 450));
        CreateUIText(canvasGO.transform, "HealthText", "Health: 100/100", 22, new Vector2(-700, 400));
        CreateUIText(canvasGO.transform, "RealmText", sceneName.Replace("_", " "), 26, new Vector2(0, 450));
        CreateUIText(canvasGO.transform, "StanceText", "Stance: Harmonist", 20, new Vector2(0, 400));

        // ========== Hand Tracking ==========
        var handManager = new GameObject("HandGestureManager");
        AddScriptIfExists(handManager, "QuestHandGestureManager");

        // ========== Final Boss ==========
        if (isFinalRealm)
        {
            var bossSpawner = new GameObject("BossSpawner");
            AddScriptIfExists(bossSpawner, "BossSpawner");

            var bossPoint = new GameObject("BossSpawnPoint");
            bossPoint.transform.SetParent(bossSpawner.transform);
            bossPoint.transform.position = new Vector3(0, 3.5f, 0);

            // Ending Manager
            var endingGO = new GameObject("EndingManager");
            AddScriptIfExists(endingGO, "EndingManager");
        }

        EditorSceneManager.SaveScene(scene, path);
        Debug.Log("Created + configured: " + path);
    }

    // -------------------- Helpers --------------------

    private static void AddScriptIfExists(GameObject target, string className)
    {
        // Find the MonoScript by class name
        string[] guids = AssetDatabase.FindAssets(className + " t:MonoScript");

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);

            if (script != null && script.GetClass() != null && script.GetClass().Name == className)
            {
                target.AddComponent(script.GetClass());
                Debug.Log($"Attached {className} to {target.name}");
                return;
            }
        }

        Debug.LogWarning($"Could not find script: {className}. You will need to attach it manually.");
    }

    private static void CreateUIText(Transform parent, string name, string text, int fontSize, Vector2 position)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var rect = go.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(420, 50);

        var textComp = go.AddComponent<UnityEngine.UI.Text>();
        textComp.text = text;
        textComp.fontSize = fontSize;
        textComp.alignment = TextAnchor.MiddleCenter;
        textComp.color = Color.white;
        textComp.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
    }

    private static GameObject CreateUIButton(Transform parent, string name, string label, Vector2 position)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent, false);

        var rect = go.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(240, 55);

        var image = go.AddComponent<UnityEngine.UI.Image>();
        image.color = new Color(0.15f, 0.15f, 0.25f, 0.95f);

        go.AddComponent<UnityEngine.UI.Button>();

        var textGO = new GameObject("Text");
        textGO.transform.SetParent(go.transform, false);

        var textRect = textGO.AddComponent<RectTransform>();
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = new Vector2(240, 55);

        var text = textGO.AddComponent<UnityEngine.UI.Text>();
        text.text = label;
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        return go;
    }

    private static void EnsureScenesFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
        {
            AssetDatabase.CreateFolder("Assets", "Scenes");
        }
    }

    private static void AddScenesToBuildSettings()
    {
        string[] scenePaths = {
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/Realm_01_NeoArcadia.unity",
            "Assets/Scenes/Realm_02_Wildlands.unity",
            "Assets/Scenes/Realm_03_Clockwork.unity",
            "Assets/Scenes/Realm_04_BoneDesert.unity",
            "Assets/Scenes/Realm_05_CrystalSanctum.unity",
            "Assets/Scenes/Realm_06_RealityStorm.unity"
        };

        var buildScenes = new EditorBuildSettingsScene[scenePaths.Length];
        for (int i = 0; i < scenePaths.Length; i++)
        {
            buildScenes[i] = new EditorBuildSettingsScene(scenePaths[i], true);
        }

        EditorBuildSettings.scenes = buildScenes;
        Debug.Log("All scenes added to Build Settings.");
    }
}
#endif
