using UnityEngine;

public static class SaveSystem
{
    public static void Save(float loyalty, int realm)
    {
        PlayerPrefs.SetFloat("Loyalty", loyalty);
        PlayerPrefs.SetInt("CurrentRealm", realm);
        PlayerPrefs.Save();
    }

    public static void Load(out float loyalty, out int realm)
    {
        loyalty = PlayerPrefs.GetFloat("Loyalty", 65f);
        realm = PlayerPrefs.GetInt("CurrentRealm", 0);
    }
}
