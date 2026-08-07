using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Vertical-slice encounter director for Neo-Arcadia.
/// Flow: Intro → Wave 1 → Phase gate → Wave 2 → Elite → Extract.
/// Attach to an empty GameObject in Realm_01_NeoArcadia.
/// </summary>
public class NeoArcadiaEncounter : MonoBehaviour
{
    public enum Phase
    {
        Intro,
        Wave1,
        PhaseGate,
        Wave2,
        Elite,
        Complete
    }

    [Header("References")]
    public Transform[] wave1SpawnPoints;
    public Transform[] wave2SpawnPoints;
    public Transform eliteSpawnPoint;
    public GameObject aggressiveEnemyPrefab;
    public GameObject rangedEnemyPrefab;
    public GameObject eliteEnemyPrefab;
    public GameObject extractGate;
    public GameObject phaseBarrier; // exists only in Base layer until player phases

    [Header("UI (optional)")]
    public UnityEngine.UI.Text objectiveText;

    [Header("Tuning")]
    public int wave1Count = 3;
    public int wave2Count = 4;
    public float introDelay = 1.5f;
    public string nextSceneName = "MainMenu";

    public Phase CurrentPhase { get; private set; } = Phase.Intro;

    private int aliveEnemies;
    private bool gateOpened;

    private void Start()
    {
        if (extractGate != null) extractGate.SetActive(false);
        StartCoroutine(RunEncounter());
    }

    private IEnumerator RunEncounter()
    {
        SetObjective("Survive Neo-Arcadia. Learn to Phase.");
        CurrentPhase = Phase.Intro;
        yield return new WaitForSeconds(introDelay);

        // Wave 1
        CurrentPhase = Phase.Wave1;
        SetObjective("Clear the street patrol.");
        SpawnWave(wave1SpawnPoints, wave1Count, false);
        yield return new WaitUntil(() => aliveEnemies <= 0);

        // Phase gate moment
        CurrentPhase = Phase.PhaseGate;
        SetObjective("Phase Shift to pass the barrier.");
        if (phaseBarrier != null) phaseBarrier.SetActive(true);

        // Wait until player has toggled phase at least once after this point
        bool phased = false;
        var phasing = GameManager.Instance != null ? GameManager.Instance.phasingManager : null;
        bool startLayerIsBase = phasing == null || phasing.IsBaseLayer;
        float timeout = 60f;
        float t = 0f;
        while (t < timeout && !phased)
        {
            if (phasing != null && phasing.IsBaseLayer != startLayerIsBase)
                phased = true;
            t += Time.deltaTime;
            yield return null;
        }

        if (phaseBarrier != null) phaseBarrier.SetActive(false);
        SetObjective("Barrier down. Push forward.");
        yield return new WaitForSeconds(1f);

        // Wave 2
        CurrentPhase = Phase.Wave2;
        SetObjective("Eliminate the phase-aware hostiles.");
        SpawnWave(wave2SpawnPoints, wave2Count, true);
        yield return new WaitUntil(() => aliveEnemies <= 0);

        // Elite
        CurrentPhase = Phase.Elite;
        SetObjective("Defeat the district enforcer.");
        SpawnElite();
        yield return new WaitUntil(() => aliveEnemies <= 0);

        // Extract
        CurrentPhase = Phase.Complete;
        SetObjective("Extract through the gate.");
        if (extractGate != null)
        {
            extractGate.SetActive(true);
            gateOpened = true;
        }
        else
        {
            yield return new WaitForSeconds(2f);
            FinishSlice();
        }
    }

    private void SpawnWave(Transform[] points, int count, bool mixRanged)
    {
        if (points == null || points.Length == 0) return;

        for (int i = 0; i < count; i++)
        {
            Transform p = points[i % points.Length];
            GameObject prefab = aggressiveEnemyPrefab;
            if (mixRanged && rangedEnemyPrefab != null && i % 2 == 1)
                prefab = rangedEnemyPrefab;

            if (prefab == null) continue;

            var go = Instantiate(prefab, p.position, Quaternion.identity);
            aliveEnemies++;
            var death = go.GetComponent<EnemyDeathHandler>();
            if (death != null)
                death.OnDied += OnEnemyDied;
            else
            {
                // Fallback: count down when object is destroyed
                var tracker = go.AddComponent<EncounterEnemyTracker>();
                tracker.owner = this;
            }
        }
    }

    private void SpawnElite()
    {
        if (eliteSpawnPoint == null) return;
        var prefab = eliteEnemyPrefab != null ? eliteEnemyPrefab : aggressiveEnemyPrefab;
        if (prefab == null) return;

        var go = Instantiate(prefab, eliteSpawnPoint.position, Quaternion.identity);
        go.transform.localScale *= 1.4f;
        aliveEnemies++;

        var death = go.GetComponent<EnemyDeathHandler>();
        if (death != null)
            death.OnDied += OnEnemyDied;
        else
        {
            var tracker = go.AddComponent<EncounterEnemyTracker>();
            tracker.owner = this;
        }
    }

    public void NotifyEnemyDied()
    {
        OnEnemyDied();
    }

    private void OnEnemyDied()
    {
        aliveEnemies = Mathf.Max(0, aliveEnemies - 1);
        AudioManager.Instance?.PlayEnemyDeath();
    }

    private void SetObjective(string text)
    {
        if (objectiveText != null)
            objectiveText.text = text;
        Debug.Log("[NeoArcadia] " + text);
    }

    /// <summary>Call from ExtractGate trigger OnTriggerEnter2D.</summary>
    public void OnPlayerReachedExtract()
    {
        if (!gateOpened) return;
        FinishSlice();
    }

    private void FinishSlice()
    {
        SetObjective("District clear. Well done.");
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
    }
}

/// <summary>Fallback death notifier when EnemyDeathHandler has no event.</summary>
public class EncounterEnemyTracker : MonoBehaviour
{
    [HideInInspector] public NeoArcadiaEncounter owner;

    private void OnDestroy()
    {
        if (owner != null)
            owner.NotifyEnemyDied();
    }
}
