#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Collections.Generic;

public class ProjectValidator : EditorWindow
{
    private static ListRequest listRequest;
    private static List<string> missingPackages = new List<string>();
    private static bool hasChecked = false;

    private static readonly string[] RequiredPackages = {
        "com.unity.inputsystem",
        "com.unity.xr.management",
        "com.unity.xr.openxr",
        "com.unity.xr.hands",
        "com.unity.xr.interaction.toolkit",
        "com.unity.textmeshpro"
    };

    [MenuItem("Shadowrift/Validate Project Setup")]
    public static void ShowWindow()
    {
        GetWindow<ProjectValidator>("Project Validator");
        CheckPackages();
    }

    private void OnGUI()
    {
        GUILayout.Label("Shadowrift Project Validator", EditorStyles.boldLabel);
        GUILayout.Space(10);

        if (GUILayout.Button("Re-check Packages", GUILayout.Height(30)))
        {
            CheckPackages();
        }

        GUILayout.Space(15);

        if (!hasChecked)
        {
            EditorGUILayout.HelpBox("Click the button above to validate package installation.", MessageType.Info);
            return;
        }

        if (missingPackages.Count == 0)
        {
            EditorGUILayout.HelpBox("All required packages appear to be installed.", MessageType.Info);
        }
        else
        {
            EditorGUILayout.HelpBox("Missing packages detected:", MessageType.Warning);
            foreach (var pkg in missingPackages)
            {
                GUILayout.Label("• " + pkg);
            }

            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Install the missing packages via Window → Package Manager.", MessageType.None);
        }

        GUILayout.Space(20);
        GUILayout.Label("Quick Actions", EditorStyles.boldLabel);

        if (GUILayout.Button("Open Package Manager"))
        {
            EditorApplication.ExecuteMenuItem("Window/Package Manager");
        }

        if (GUILayout.Button("Open XR Plug-in Management"))
        {
            SettingsService.OpenProjectSettings("Project/XR Plug-in Management");
        }

        if (GUILayout.Button("Generate All Scenes"))
        {
            EditorApplication.ExecuteMenuItem("Shadowrift/Generate All Scenes");
        }
    }

    private static void CheckPackages()
    {
        missingPackages.Clear();
        hasChecked = false;

        listRequest = Client.List(true);
        EditorApplication.update += OnPackageListProgress;
    }

    private static void OnPackageListProgress()
    {
        if (!listRequest.IsCompleted) return;

        EditorApplication.update -= OnPackageListProgress;

        if (listRequest.Status == StatusCode.Success)
        {
            var installed = new HashSet<string>();
            foreach (var package in listRequest.Result)
            {
                installed.Add(package.name);
            }

            foreach (var required in RequiredPackages)
            {
                if (!installed.Contains(required))
                {
                    missingPackages.Add(required);
                }
            }
        }
        else
        {
            Debug.LogError("Failed to list packages: " + listRequest.Error.message);
        }

        hasChecked = true;
    }
}
#endif
