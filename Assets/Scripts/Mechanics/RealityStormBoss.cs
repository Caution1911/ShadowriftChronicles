using UnityEngine;
using System.Collections;

public class RealityStormBoss : MonoBehaviour
{
    public int maxHealth = 50;
    public int currentHealth;
    public string nativeLayer = "phase";
    public float phaseSwitchInterval = 4f;

    private void Start()
    {
        currentHealth = maxHealth;
        StartCoroutine(PhaseCycle());
    }

    private IEnumerator PhaseCycle()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(phaseSwitchInterval);
            nativeLayer = nativeLayer == "base" ? "phase" : "base";
            Debug.Log("Boss switched to layer: " + nativeLayer);
        }
    }

    public void TakeDamage(int damage)
    {
        string playerLayer = GameManager.Instance.phasingManager.CurrentLayer;
        if (playerLayer != nativeLayer) return;

        currentHealth -= damage;
        GameManager.Instance.loyaltyManager.ModifyLoyalty(3f);

        if (currentHealth <= 0)
        {
            Debug.Log("Reality Storm Boss defeated!");
            // Trigger ending here
            Destroy(gameObject);
        }
    }
}
