using UnityEngine;
using System.Collections;

public class RealmProgression : MonoBehaviour
{
    public float checkInterval = 1.5f;
    public float delayBeforeNextRealm = 2.5f;
    private bool isTransitioning = false;

    private void Start()
    {
        StartCoroutine(CheckEnemiesCleared());
    }

    private IEnumerator CheckEnemiesCleared()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);

            if (isTransitioning) continue;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length == 0)
            {
                isTransitioning = true;
                Debug.Log("All enemies cleared! Preparing next realm...");
                yield return new WaitForSeconds(delayBeforeNextRealm);

                if (RealmLoader.Instance != null)
                {
                    SaveSystem.Save(GameManager.Instance.loyaltyManager.loyalty, RealmLoader.Instance.currentRealmIndex);
                    RealmLoader.Instance.LoadNextRealm();
                }
            }
        }
    }
}
