using UnityEngine;
using UnityEngine.XR.Hands;
using System.Collections.Generic;

/// <summary>
/// Meta Quest Hand Tracking Gesture Manager for Shadowrift Chronicles
/// Supports:
/// - Right hand pinch = Voidblade Attack
/// - Left hand swipe left/right = Stance switch
/// - Both hands open palm = Dimensional Phase Shift
/// </summary>
public class QuestHandGestureManager : MonoBehaviour
{
    [Header("References")]
    private XRHandSubsystem handSubsystem;

    [Header("Pinch Settings")]
    public float pinchThreshold = 0.035f;          // Distance between thumb and index tip

    [Header("Swipe Settings")]
    public float swipeDistanceThreshold = 0.18f;  // How far the hand must move
    public float swipeCooldown = 0.7f;

    [Header("Open Palm Settings")]
    public float openPalmFingerThreshold = 0.04f; // How straight fingers need to be

    // Internal tracking
    private Vector3 leftPalmPrevPos;
    private float lastSwipeTime;
    private float lastPhaseTime;
    private float phaseCooldown = 1.0f;

    private void Start()
    {
        // Get the XR Hand Subsystem
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);

        if (subsystems.Count > 0)
        {
            handSubsystem = subsystems[0];
            Debug.Log("XR Hand Subsystem found and ready.");
        }
        else
        {
            Debug.LogWarning("No XR Hand Subsystem found. Hand tracking will not work.");
        }

        leftPalmPrevPos = Vector3.zero;
    }

    private void Update()
    {
        if (handSubsystem == null || !handSubsystem.running) return;

        // === RIGHT HAND PINCH = Attack ===
        if (IsPinching(XRHandedness.Right))
        {
            PerformAttack();
        }

        // === LEFT HAND SWIPE = Stance Switch ===
        DetectSwipe(XRHandedness.Left);

        // === BOTH HANDS OPEN PALM = Phase Shift ===
        if (IsOpenPalm(XRHandedness.Left) && IsOpenPalm(XRHandedness.Right))
        {
            if (Time.time - lastPhaseTime > phaseCooldown)
            {
                TriggerPhase();
                lastPhaseTime = Time.time;
            }
        }
    }

    // -------------------- Gesture Detection --------------------

    private bool IsPinching(XRHandedness handedness)
    {
        if (!handSubsystem.TryGetJoint(XRHandJointID.ThumbTip, handedness, out var thumbTip))
            return false;
        if (!handSubsystem.TryGetJoint(XRHandJointID.IndexTip, handedness, out var indexTip))
            return false;

        float distance = Vector3.Distance(thumbTip.pose.position, indexTip.pose.position);
        return distance < pinchThreshold;
    }

    private bool IsOpenPalm(XRHandedness handedness)
    {
        // Check that major finger tips are relatively far from the palm (fingers extended)
        if (!handSubsystem.TryGetJoint(XRHandJointID.Palm, handedness, out var palm))
            return false;

        XRHandJointID[] fingerTips = {
            XRHandJointID.IndexTip,
            XRHandJointID.MiddleTip,
            XRHandJointID.RingTip,
            XRHandJointID.LittleTip
        };

        int extendedCount = 0;

        foreach (var tipID in fingerTips)
        {
            if (handSubsystem.TryGetJoint(tipID, handedness, out var tip))
            {
                float dist = Vector3.Distance(tip.pose.position, palm.pose.position);
                if (dist > openPalmFingerThreshold)
                    extendedCount++;
            }
        }

        // Consider open palm if at least 3 fingers are extended
        return extendedCount >= 3;
    }

    private void DetectSwipe(XRHandedness handedness)
    {
        if (Time.time - lastSwipeTime < swipeCooldown) return;

        if (!handSubsystem.TryGetJoint(XRHandJointID.Palm, handedness, out var palm))
            return;

        Vector3 currentPos = palm.pose.position;

        // First frame initialization
        if (leftPalmPrevPos == Vector3.zero)
        {
            leftPalmPrevPos = currentPos;
            return;
        }

        Vector3 delta = currentPos - leftPalmPrevPos;

        // Horizontal swipe detection
        if (Mathf.Abs(delta.x) > swipeDistanceThreshold && Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
            {
                // Swipe Right → Ravager
                GameManager.Instance?.stanceManager?.SwitchStance(Stance.Ravager);
                AudioManager.Instance?.PlayStanceSwitch();
                Debug.Log("Swipe Right → Ravager Stance");
            }
            else
            {
                // Swipe Left → Sentinel
                GameManager.Instance?.stanceManager?.SwitchStance(Stance.Sentinel);
                AudioManager.Instance?.PlayStanceSwitch();
                Debug.Log("Swipe Left → Sentinel Stance");
            }

            lastSwipeTime = Time.time;
        }

        leftPalmPrevPos = currentPos;
    }

    // -------------------- Actions --------------------

    private void PerformAttack()
    {
        // Simple attack trigger (you can expand this with raycasts or hitboxes)
        GameManager.Instance?.loyaltyManager?.ModifyLoyalty(-2.0f);
        AudioManager.Instance?.PlayAttack();
        Debug.Log("Right Hand Pinch → Voidblade Attack");
    }

    private void TriggerPhase()
    {
        GameManager.Instance?.phasingManager?.TogglePhase();
        AudioManager.Instance?.PlayPhase();
        Debug.Log("Both Hands Open → Dimensional Phase Shift");
    }
}
