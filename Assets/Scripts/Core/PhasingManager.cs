using UnityEngine;
using System.Collections;

public class PhasingManager : MonoBehaviour
{
    public GameObject baseLayer;
    public GameObject phaseLayer;
    public float transitionTime = 0.18f;

    private bool isTransitioning = false;
    public string CurrentLayer { get; private set; } = "base";

    public void TogglePhase()
    {
        if (isTransitioning) return;
        isTransitioning = true;
        StartCoroutine(DoPhaseTransition());
    }

    private IEnumerator DoPhaseTransition()
    {
        CurrentLayer = CurrentLayer == "base" ? "phase" : "base";

        if (baseLayer != null) baseLayer.SetActive(CurrentLayer == "base");
        if (phaseLayer != null) phaseLayer.SetActive(CurrentLayer == "phase");

        yield return new WaitForSeconds(transitionTime);
        isTransitioning = false;
    }
}
