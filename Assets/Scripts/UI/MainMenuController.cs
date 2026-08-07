using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main Menu controller for Shadowrift Chronicles.
/// Wire buttons to public methods below.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Scene Names")]
    public string firstRealmScene = "Realm_01_NeoArcadia";
    public string settingsScene = ""; // optional

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject controlsPanel;
    public GameObject creditsPanel;

    private void Start()
    {
        ShowMain();
        AudioManager.Instance?.PlayMusicForScene("MainMenu");
        AudioManager.Instance?.PlayUiConfirm();
    }

    public void OnPlayPressed()
    {
        AudioManager.Instance?.PlayUiConfirm();
        if (!string.IsNullOrEmpty(firstRealmScene))
            SceneManager.LoadScene(firstRealmScene);
        else
            Debug.LogWarning("MainMenu: firstRealmScene not set.");
    }

    public void OnControlsPressed()
    {
        AudioManager.Instance?.PlayUiConfirm();
        if (mainPanel != null) mainPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(true);
    }

    public void OnCreditsPressed()
    {
        AudioManager.Instance?.PlayUiConfirm();
        if (mainPanel != null) mainPanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(true);
    }

    public void OnBackPressed()
    {
        AudioManager.Instance?.PlayUiBack();
        ShowMain();
    }

    public void OnQuitPressed()
    {
        AudioManager.Instance?.PlayUiBack();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ShowMain()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (creditsPanel != null) creditsPanel.SetActive(false);
    }
}
