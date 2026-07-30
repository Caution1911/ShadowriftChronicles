using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;
    public float invincibilityTime = 1.2f;

    private bool isInvincible = false;
    private Vector3 respawnPoint;

    private void Start()
    {
        currentHealth = maxHealth;
        respawnPoint = transform.position;
        UIManager.Instance?.UpdateHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        UIManager.Instance?.UpdateHealth(currentHealth, maxHealth);
        AudioManager.Instance?.PlayPlayerHurt();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
        }
    }

    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    private void Die()
    {
        currentHealth = maxHealth;
        transform.position = respawnPoint;
        UIManager.Instance?.UpdateHealth(currentHealth, maxHealth);
        GameManager.Instance?.loyaltyManager?.ModifyLoyalty(-8f);
    }

    public void SetRespawnPoint(Vector3 point)
    {
        respawnPoint = point;
    }
}
