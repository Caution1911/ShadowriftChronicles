using UnityEngine;
using UnityEngine.SceneManagement;

public class RealmLoader : MonoBehaviour
{
    public static RealmLoader Instance;

    public string[] realmScenes = {
        "Realm_01_NeoArcadia",
        "Realm_02_Wildlands",
        "Realm_03_Clockwork",
        "Realm_04_BoneDesert",
        "Realm_05_CrystalSanctum"
    };

    public int currentRealmIndex = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void LoadRealm(int index)
    {
        if (index < 0 || index >= realmScenes.Length) return;
        currentRealmIndex = index;
        SceneManager.LoadScene(realmScenes[index]);
    }

    public void LoadNextRealm()
    {
        if (currentRealmIndex + 1 < realmScenes.Length)
        {
            LoadRealm(currentRealmIndex + 1);
        }
        else
        {
            SceneManager.LoadScene("Realm_06_RealityStorm");
        }
    }
}
