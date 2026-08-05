#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

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

        EditorGUILayout.HelpBox("This tool will create all required scenes with the basic hierarchy.\n\nMake sure you have already created the Scripts folder structure.", MessageType.Info);

        GUILayout.Space(10);

        if (GUILayout.Button("Generate All Scenes", GUILayout.Height(40)))
        {
            GenerateAllScenes();
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Generate Main Menu Only"))
        {
            CreateMainMenuScene();
            EditorUtility.DisplayDialog("Done", "MainMenu scene created successfully!", "OK");
        }

        if (GUILayout.Button("Generate All Realm Scenes"))
        {
            CreateRealmScenes();
            EditorUtility.DisplayDialog("Done", "All 5 Realm scenes + Reality Storm created!", "OK");
        }
    }

    private static void GenerateAllScenes()
    {
        CreateMainMenuScene();
        CreateRealmScenes();

        // Add scenes to Build Settings
        AddScenesToBuildSettings();

        EditorUtility.DisplayDialog("Success", 
            "All scenes have been generated!\n\n" +
            "- MainMenu\n" +
            "- Realm_01_NeoArcadia\n" +
            "- Realm_02_Wildlands\n" +
            "- Realm_03_Clockwork\n" +
            "- Realm_04_BoneDesert\n" +
            "- Realm_05_CrystalSanctum\n" +
            "- Realm_06_RealityStorm\n\n" +
            "Scenes have also been added to Build Settings.", "OK");
    }

    private static void CreateMainMenuScene()
    {
        string path = "Assets/Scenes/MainMenu.unity";
        EnsureScenesFolder();

        var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        // Create Canvas
        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasGO.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // Event System
        new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));

        // Main Menu Controller
        var controllerGO = new GameObject("MainMenuController");
        // Note: Script will need to be assigned manually or via reflection if compiled

        // Title
        CreateUIText(canvasGO.transform, "Title", "SHADOWRIFT CHRONICLES", 48, new Vector2(0, 200));

        // Buttons
        CreateUIButton(canvasGO.transform, "NewGameButton", "New Game", new Vector2(0, 50));
        CreateUIButton(canvasGO.transform, "ContinueButton", "Continue", new Vector2(0, -20));
        CreateUIButton(canvasGO.transform, "QuitButton", "Quit", new Vector2(0, -90));

        EditorSceneManager.SaveScene(scene, path);
        Debug.Log("Created: " + path);
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
            new Color(0.1f, 0.15f, 0.3f),   // Neo-Arcadia
            new Color(0.05f, 0.2f, 0.1f),   // Wildlands
            new Color(0.2f, 0.2f, 0.25f),   // Clockwork
            new Color(0.25f, 0.18f, 0.1f),  // Bone Desert
            new Color(0.15f, 0.1f, 0.3f),   // Crystal Sanctum
            new Color(0.3f, 0.05f, 0.35f)   // Reality Storm
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

        // Set camera background color
        Camera.main.backgroundColor = bgColor;
        Camera.main.orthographic = true;

        // === Core Systems ===
        var systems = new GameObject("GameSystems");

        // Layers
        var baseLayer = new GameObject("BaseLayer");
        var phaseLayer = new GameObject("PhaseLayer");
        phaseLayer.SetActive(false);

        // Player
        var player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<SpriteRenderer>();
        var rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        player.AddComponent<CapsuleCollider2D>();
        player.transform.position = Vector3.zero;

        // Enemy Spawner
        var spawner = new GameObject("EnemySpawner");
        for (int i = 0; i < 6; i++)
        {
            var point = new GameObject("SpawnPoint_" + i);
            point.transform.SetParent(spawner.transform);
            point.transform.position = new Vector3(Random.Range(-8f, 8f), Random.Range(-4f, 4f), 0);
        }

        // UI Canvas
        var canvasGO = new GameObject("Canvas");
        var canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasGO.AddComponent<UnityEngine.UI.CanvasScaler>();
        canvasGO.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        new GameObject("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));

        // Simple UI placeholders
        CreateUIText(canvasGO.transform, "LoyaltyText", "Loyalty: 65", 24, new Vector2(-700, 450));
        CreateUIText(canvasGO.transform, "ScoreText", "Score: 0", 24, new Vector2(700, 450));
        CreateUIText(canvasGO.transform, "HealthText", "Health: 100/100", 24, new Vector2(-700, 400));
        CreateUIText(canvasGO.transform, "RealmText", sceneName, 28, new Vector2(0, 450));

        // Final realm boss spawner
        if (isFinalRealm)
        {
            var bossSpawner = new GameObject("BossSpawner");
            var bossPoint = new GameObject("BossSpawnPoint");
            bossPoint.transform.SetParent(bossSpawner.transform);
            bossPoint.transform.position = new Vector3(0, 3, 0);
        }

        // Hand Gesture Manager (for Quest)
        new GameObject("HandGestureManager");

        EditorSceneManager.SaveScene(scene, path);
        Debug.Log("Created: " + path);
    }

    private static void CreateUIText(Transform parent, string name, string text, int fontSize, Vector2 position)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent);

        var rect = go.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(400, 60);

        var textComp = go.AddComponent<UnityEngine.UI.Text>();
        textComp.text = text;
        textComp.fontSize = fontSize;
        textComp.alignment = TextAnchor.MiddleCenter;
        textComp.color = Color.white;

        // Try to use Arial
        textComp.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
    }

    private static void CreateUIButton(Transform parent, string name, string label, Vector2 position)
    {
        var go = new GameObject(name);
        go.transform.SetParent(parent);

        var rect = go.AddComponent<RectTransform>();
        rect.anchoredPosition = position;
        rect.sizeDelta = new Vector2(220, 50);

        var image = go.AddComponent<UnityEngine.UI.Image>();
        image.color = new Color(0.2f, 0.2f, 0.3f, 0.9f);

        go.AddComponent<UnityEngine.UI.Button>();

        // Button label
        var textGO = new GameObject("Text");
        textGO.transform.SetParent(go.transform);
        var textRect = textGO.AddComponent<RectTransform>();
        textRect.anchoredPosition = Vector2.zero;
        textRect.sizeDelta = new Vector2(220, 50);

        var text = textGO.AddComponent<UnityEngine.UI.Text>();
        text.text = label;
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
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
        Debug.Log("Scenes added to Build Settings.");
    }
}
#endif
