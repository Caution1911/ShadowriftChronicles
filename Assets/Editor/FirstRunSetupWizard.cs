#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;

public class FirstRunSetupWizard : EditorWindow
{
    private int currentStep = 0;
    private readonly string[] stepTitles = {
        "Welcome",
        "Check Packages",
        "Generate Scenes",
        "Create Sample Prefabs",
        "Finish"
    };

    [MenuItem("Shadowrift/First Run Setup Wizard")]
    public static void ShowWindow()
    {
        var window = GetWindow<FirstRunSetupWizard>("Shadowrift Setup");
        window.minSize = new Vector2(520, 420);
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.Label("Shadowrift Chronicles – First Run Setup", EditorStyles.boldLabel);
        GUILayout.Label($"Step {currentStep + 1} of {stepTitles.Length}: {stepTitles[currentStep]}", EditorStyles.miniBoldLabel);
        GUILayout.Space(15);

        switch (currentStep)
        {
            case 0: DrawWelcome(); break;
            case 1: DrawPackageStep(); break;
            case 2: DrawSceneStep(); break;
            case 3: DrawPrefabStep(); break;
            case 4: DrawFinish(); break;
        }

        GUILayout.FlexibleSpace();
        DrawNavigation();
    }

    private void DrawWelcome()
    {
        EditorGUILayout.HelpBox(
            "Welcome to the Shadowrift Chronicles setup wizard.\n\n" +
            "This will guide you through:\n" +
            "• Verifying required packages\n" +
            "• Generating all game scenes\n" +
            "• Creating sample prefabs\n\n" +
            "Make sure your scripts have finished compiling before continuing.",
            MessageType.Info);
    }

    private void DrawPackageStep()
    {
        EditorGUILayout.HelpBox(
            "Required packages:\n" +
            "• Input System\n" +
            "• XR Plugin Management\n" +
            "• OpenXR\n" +
            "• XR Hands\n" +
            "• XR Interaction Toolkit\n" +
            "• TextMeshPro\n\n" +
            "Click the button below to open the validator.",
            MessageType.None);

        if (GUILayout.Button("Open Project Validator", GUILayout.Height(35)))
        {
            EditorApplication.ExecuteMenuItem("Shadowrift/Validate Project Setup");
        }

        GUILayout.Space(8);
        if (GUILayout.Button("Open Package Manager"))
        {
            EditorApplication.ExecuteMenuItem("Window/Package Manager");
        }
    }

    private void DrawSceneStep()
    {
        EditorGUILayout.HelpBox(
            "This will create:\n" +
            "• MainMenu\n" +
            "• 5 Realm scenes\n" +
            "• Reality Storm final scene\n\n" +
            "Scripts will be attached and key references wired automatically.",
            MessageType.Info);

        if (GUILayout.Button("Generate All Scenes (Full Setup)", GUILayout.Height(40)))
        {
            EditorApplication.ExecuteMenuItem("Shadowrift/Generate All Scenes");
        }
    }

    private void DrawPrefabStep()
    {
        EditorGUILayout.HelpBox(
            "Create basic placeholder prefabs so you can test the game quickly.\n\n" +
            "These are simple colored sprites you can replace later with real art.",
            MessageType.Info);

        if (GUILayout.Button("Create Sample Prefabs", GUILayout.Height(40)))
        {
            SamplePrefabGenerator.GenerateAll();
            EditorUtility.DisplayDialog("Prefabs Created",
                "Sample prefabs have been created in Assets/Prefabs/Sample/", "OK");
        }
    }

    private void DrawFinish()
    {
        EditorGUILayout.HelpBox(
            "Setup complete!\n\n" +
            "Recommended next steps:\n" +
            "1. Open Realm_01_NeoArcadia\n" +
            "2. Assign sample enemy prefabs to the EnemySpawner\n" +
            "3. Press Play and test phasing (Space) and movement\n" +
            "4. Replace placeholders with real art when ready\n\n" +
            "Documentation is available in the Docs/ folder.",
            MessageType.Info);

        if (GUILayout.Button("Open Neo-Arcadia Scene", GUILayout.Height(35)))
        {
            var scenePath = "Assets/Scenes/Realm_01_NeoArcadia.unity";
            if (File.Exists(scenePath))
            {
                EditorSceneManager.OpenScene(scenePath);
            }
            else
            {
                EditorUtility.DisplayDialog("Scene Missing",
                    "Please generate scenes first (Step 3).", "OK");
            }
        }
    }

    private void DrawNavigation()
    {
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();

        GUI.enabled = currentStep > 0;
        if (GUILayout.Button("← Back", GUILayout.Height(30)))
        {
            currentStep--;
        }
        GUI.enabled = true;

        if (currentStep < stepTitles.Length - 1)
        {
            if (GUILayout.Button("Next →", GUILayout.Height(30)))
            {
                currentStep++;
            }
        }
        else
        {
            if (GUILayout.Button("Close", GUILayout.Height(30)))
            {
                Close();
            }
        }

        EditorGUILayout.EndHorizontal();
    }
}
#endif
