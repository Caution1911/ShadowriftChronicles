using UnityEngine;

public enum Stance { Ravager, Sentinel, Harmonist }

public class StanceManager : MonoBehaviour
{
    public Stance currentStance = Stance.Harmonist;

    public void SwitchStance(Stance newStance)
    {
        currentStance = newStance;
        Debug.Log("Stance: " + newStance);
    }

    public void CycleStance()
    {
        int next = ((int)currentStance + 1) % 3;
        SwitchStance((Stance)next);
    }
}
