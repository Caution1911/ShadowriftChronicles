using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button continueButton;

    private void Start()
    {
        bool hasSave = PlayerPrefs.HasKey("Loyalty");
        if (continueButton != null)
            continueButton.interactable = hasSave;
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Realm_01_NeoArcadia");
    }

    public void ContinueGame()
    {
        float loyalty;
        int realm;
        SaveSystem.Load(out loyalty, out realm);

        string[] scenes = {
            "Realm_01_NeoArcadia",
            "Realm_02_Wildlands",
            "Realm_03_Clockwork",
            "Realm_04_BoneDesert",
            "Realm_05_CrystalSanctum"
        };

        if (realm >= 0 && realm < scenes.Length)
            SceneManager.LoadScene(scenes[realm]);
        else
            SceneManager.LoadScene("Realm_01_NeoArcadia");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
