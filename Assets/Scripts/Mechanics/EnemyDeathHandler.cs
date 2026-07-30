using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    public GameObject deathParticlePrefab;
    public int scoreValue = 25;

    public void Die()
    {
        if (deathParticlePrefab != null)
        {
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        }

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScore(scoreValue);

        if (GameManager.Instance != null)
            GameManager.Instance.loyaltyManager.ModifyLoyalty(2f);

        Destroy(gameObject);
    }
}
