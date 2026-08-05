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
        GUILayout.Label("Shadowrift Chronicles - Full Scene Generator", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.HelpBox(
            "Creates all scenes, attaches scripts, and auto-wires the most important references.\n\n" +
            "Run this after all scripts have finished compiling.",
            MessageType.Info);

        GUILayout.Space(15);

        if (GUILayout.Button("Generate All Scenes (Full Setup)", GUILayout.Height(50)))
        {
            GenerateAllScenes();
        }

        GUILayout.Space(8);

        if (GUILayout.Button("Generate Main Menu Only"))
        {
            CreateMainMenuScene();
            EditorUtility.DisplayDialog("Done", "MainMenu created.", "OK");
        }

        if (GUILayout.Button("Generate All Realm Scenes"))
        {
            CreateRealmScenes();
            EditorUtility.DisplayDialog("Done", "All realm scenes created.", "OK");
        }
    }

    private static void GenerateAllScenes()
    {
        CreateMainMenuScene();
        CreateRealmScenes();
        AddScenesToBuildSettings();

        EditorUtility.DisplayDialog("Success",
            "All scenes generated with scripts + auto-wiring!\n\n" +
            "✓ MainMenu\n" +
            "✓ 5 Realms + Reality Storm\n" +
            "✓ Scripts attached\n" +
            "✓ Key references wired\n" +
            "✓ Added to Build Settings\n\n" +
            "Open any realm scene and check the GameSystems object.", "OK");
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

        new GameObject("EventSystem",
            typeof(UnityEngine.EventSystems.EventSystem),
            typeof(UnityEngine.EventSystems.StandaloneInputModule));

        var controllerGO = new GameObject("MainMenuController");
        AddScriptIfExists(controllerGO, "MainMenuController");

        CreateUIText(canvasGO.transform, "Title", "SHADOWRIFT CHRONICLES", 52, new Vector2(0, 220));
        CreateUIButton(canvasGO.transform, "NewGameButton", "New Game", new Vector2(0, 60));
        CreateUIButton(canvasGO.transform, "ContinueButton", "Continue", new Vector2(0, -20));
        CreateUIButton(canvasGO.transform, "QuitButton", "Quit", new Vector2(0, -100));

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
            new Color(0.08f, 0.12f, 0.28f),
            new Color(0.04f, 0.18f, 0.09f),
            new Color(0.18f, 0.18f, 0.22f),
            new Color(0.22f, 0.15f, 0.08f),
            new Color(0.12f, 0.08f, 0.28f),
            new Color(0.28f, 0.04f, 0.32f)
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

        // Camera
        Camera.main.backgroundColor = bgColor;
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 5.5f;

        // ========== GameSystems ==========
        var systems = new GameObject("GameSystems");

        var gameManager = AddScriptIfExists(systems, "GameManager");
        var phasingManager = AddScriptIfExists(systems, "PhasingManager");
        var stanceManager = AddScriptIfExists(systems, "StanceManager");
        var loyaltyManager = AddScriptIfExists(systems, "LoyaltyManager");
        var scoreManager = AddScriptIfExists(systems, "ScoreManager");
        var audioManager = AddScriptIfExists(systems, "AudioManager");
        var realmLoader = AddScriptIfExists(systems, "RealmLoader");
        var realmProgression = AddScriptIfExists(systems, "RealmProgression");

        // ========== Layers ==========
        var baseLayer = new GameObject("BaseLayer");
        var phaseLayer = new GameObject("PhaseLayer");
        phaseLayer.SetActive(false);

        // Auto-wire PhasingManager layers
        if (phasingManager != null)
        {
            SetPrivateOrPublicField(phasingManager, "baseLayer", baseLayer);
            SetPrivateOrPublicField(phasingManager, "phaseLayer", phaseLayer);
        }

        // ========== Player ==========
        var player = new GameObject("Player");
        player.tag = "Player";
        player.AddComponent<SpriteRenderer>();
        var rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        player.AddComponent<CapsuleCollider2D>();
        player.transform.position = Vector3.zero;

        var playerController = AddScriptIfExists(player, "PlayerController");
        var playerHealth = AddScriptIfExists(player, "PlayerHealth");

        // Wire GameManager references
        if (gameManager != null)
        {
            SetPrivateOrPublicField(gameManager, "phasingManager", phasingManager);
            SetPrivateOrPublicField(gameManager, "stanceManager", stanceManager);
            SetPrivateOrPublicField(gameManager, "loyaltyManager", loyaltyManager);
            SetPrivateOrPublicField(gameManager, "playerController", playerController);
        }

        // ========== Enemy Spawner ==========
        var spawner = new GameObject("EnemySpawner");
        AddScriptIfExists(spawner, "EnemySpawner");

        for (int i = 0; i < 6; i++)
        {
            var point = new GameObject("SpawnPoint_" + i);
            point.transform.SetParent(spawner.transform);
            float angle = i * Mathf.PI * 2f / 6f;
            point.transform.position = new Vector3(Mathf.Cos(angle) * 6.5f, Mathf.Sin(angle) * 3.8f, 0);
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

        var uiManagerGO = new GameObject("UIManager");
        uiManagerGO.transform.SetParent(canvasGO.transform, false);
        AddScriptIfExists(uiManagerGO, "UIManager");

        CreateUIText(canvasGO.transform, "LoyaltyText", "Loyalty: 65", 22, new Vector2(-700, 450));
        CreateUIText(canvasGO.transform, "ScoreText", "Score: 0", 22, new Vector2(700, 450));
        CreateUIText(canvasGO.transform, "HealthText", "Health: 100/100", 22, new Vector2(-700, 400));
        CreateUIText(canvasGO.transform, "RealmText", sceneName.Replace("_", " "), 26, new Vector2(0, 450));
        CreateUIText(canvasGO.transform, "StanceText", "Stance: Harmonist", 20, new Vector2(0, 400));

        // ========== Hand Tracking ==========
        var handManager = new GameObject("HandGestureManager");
        AddScriptIfExists(handManager, "QuestHandGestureManager");

        // ========== Final Realm extras ==========
        if (isFinalRealm)
        {
            var bossSpawner = new GameObject("BossSpawner");
            AddScriptIfExists(bossSpawner, "BossSpawner");

            var bossPoint = new GameObject("BossSpawnPoint");
            bossPoint.transform.SetParent(bossSpawner.transform);
            bossPoint.transform.position = new Vector3(0, 3.5f, 0);

            var endingGO = new GameObject("EndingManager");
            AddScriptIfExists(endingGO, "EndingManager");
        }

        EditorSceneManager.SaveScene(scene, path);
        Debug.Log("Created + wired: " + path);
    }

    // -------------------- Helpers --------------------

    private static Component AddScriptIfExists(GameObject target, string className)
    {
        string[] guids = AssetDatabase.FindAssets(className + " t:MonoScript");

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);

            if (script != null && script.GetClass() != null && script.GetClass().Name == className)
            {
                var component = target.AddComponent(script.GetClass());
                Debug.Log($"Attached {className} to {target.name}");
                return component;
            }
        }

        Debug.LogWarning($"Could not find script: {className}");
        return null;
    }

    private static void SetPrivateOrPublicField(Component comp, string fieldName, Object value)
    {
        if (comp == null) return;

        var type = comp.GetType();
        var field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        if (field != null)
        {
            field.SetValue(comp, value);
            Debug.Log($"Wired {comp.GetType().Name}.{fieldName}");
        }
        else
        {
            // Try property
            var prop = type.GetProperty(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(comp, value);
                Debug.Log($"Wired property {comp.GetType().Name}.{fieldName}");
            }
        }
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
        Debug.Log("Scenes added to Build Settings.");
    }
}
#endif
