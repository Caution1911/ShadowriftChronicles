using UnityEngine;

/// <summary>
/// Place on the extract gate collider (Is Trigger = true).
/// Notifies NeoArcadiaEncounter when the player enters.
/// </summary>
public class ExtractGate : MonoBehaviour
{
    public NeoArcadiaEncounter encounter;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (encounter == null)
            encounter = FindObjectOfType<NeoArcadiaEncounter>();
        encounter?.OnPlayerReachedExtract();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (encounter == null)
            encounter = FindObjectOfType<NeoArcadiaEncounter>();
        encounter?.OnPlayerReachedExtract();
    }
}
