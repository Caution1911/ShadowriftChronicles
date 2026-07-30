using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public int enemiesPerWave = 6;
    public float spawnInterval = 3f;
    public int maxEnemiesAlive = 12;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnWave), 1.5f, spawnInterval);
    }

    private void SpawnWave()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= maxEnemiesAlive)
            return;

        if (enemyPrefabs == null || enemyPrefabs.Length == 0 || spawnPoints == null || spawnPoints.Length == 0)
            return;

        for (int i = 0; i < enemiesPerWave; i++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            GameObject enemy = Instantiate(prefab, point.position, Quaternion.identity);
            enemy.tag = "Enemy";
        }
    }
}
