using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject realityStormBossPrefab;
    public Transform bossSpawnPoint;

    private void Start()
    {
        // Only spawn the boss in the final realm
        if (RealmLoader.Instance != null && RealmLoader.Instance.currentRealmIndex >= 4)
        {
            if (realityStormBossPrefab != null && bossSpawnPoint != null)
            {
                Instantiate(realityStormBossPrefab, bossSpawnPoint.position, Quaternion.identity);
                Debug.Log("Reality Storm Boss has been summoned!");
            }
        }
    }
}
