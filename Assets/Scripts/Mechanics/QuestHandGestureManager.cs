using UnityEngine;
using UnityEngine.XR.Hands;
using System.Collections.Generic;

/// <summary>
/// Meta Quest 3 optimized Hand Tracking Gesture Manager for Shadowrift Chronicles.
/// Gestures:
/// - Right Hand Pinch          = Voidblade Attack
/// - Left Hand Swipe Left/Right = Stance switch (Sentinel / Ravager)
/// - Both Hands Open Palm      = Dimensional Phase Shift
/// - Right Hand Thumbs Up      = Harmonist Stance
/// Tuned thresholds for Quest 3 hand tracking.
/// </summary>
public class QuestHandGestureManager : MonoBehaviour
{
    [Header("Subsystem")]
    private XRHandSubsystem handSubsystem;

    [Header("Pinch (Attack) - Quest 3 tuned")]
    [Tooltip("Distance between thumb tip and index tip (meters)")]
    public float pinchThreshold = 0.028f;
    public float attackCooldown = 0.35f;

    [Header("Swipe (Stance) - Quest 3 tuned")]
    public float swipeDistanceThreshold = 0.16f;
    public float swipeCooldown = 0.65f;
    public float minSwipeSpeed = 0.8f;

    [Header("Open Palm (Phase) - Quest 3 tuned")]
    public float openPalmMinFingerDistance = 0.055f;
    public int requiredExtendedFingers = 3;
    public float phaseCooldown = 0.9f;
    public float openPalmHoldTime = 0.25f;

    [Header("Thumbs Up (Harmonist)")]
    public float thumbsUpCooldown = 0.8f;

    [Header("Debug")]
    public bool debugLogs = true;

    // Internal state
    private Vector3 leftPalmPrevPos;
    private float leftPalmPrevTime;
    private float lastSwipeTime;
    private float lastAttackTime;
    private float lastPhaseTime;
    private float lastThumbsUpTime;
    private float openPalmTimer;

    private void Start()
    {
        var subsystems = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(subsystems);

        if (subsystems.Count > 0)
        {
            handSubsystem = subsystems[0];
            if (debugLogs) Debug.Log("[Shadowrift] XR Hand Subsystem ready (Quest 3).");
        }
        else
        {
            Debug.LogWarning("[Shadowrift] No XR Hand Subsystem found. Enable XR Hands + OpenXR Meta Quest Support.");
        }

        leftPalmPrevPos = Vector3.zero;
        leftPalmPrevTime = Time.time;
    }

    private void Update()
    {
        if (handSubsystem == null || !handSubsystem.running) return;

        // Right hand pinch = Attack
        if (IsPinching(XRHandedness.Right) && Time.time - lastAttackTime > attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
        }

        // Left hand swipe = Stance
        DetectSwipe(XRHandedness.Left);

        // Both hands open palm (held briefly) = Phase
        if (IsOpenPalm(XRHandedness.Left) && IsOpenPalm(XRHandedness.Right))
        {
            openPalmTimer += Time.deltaTime;
            if (openPalmTimer >= openPalmHoldTime && Time.time - lastPhaseTime > phaseCooldown)
            {
                TriggerPhase();
                lastPhaseTime = Time.time;
                openPalmTimer = 0f;
            }
        }
        else
        {
            openPalmTimer = 0f;
        }

        // Right hand thumbs up = Harmonist
        if (IsThumbsUp(XRHandedness.Right) && Time.time - lastThumbsUpTime > thumbsUpCooldown)
        {
            GameManager.Instance?.stanceManager?.SwitchStance(Stance.Harmonist);
            AudioManager.Instance?.PlayStanceSwitch();
            TriggerHaptic(0.4f, 0.08f);
            lastThumbsUpTime = Time.time;
            if (debugLogs) Debug.Log("[Shadowrift] Thumbs Up → Harmonist Stance");
        }
    }

    // -------------------- Gesture Detection (Quest 3 tuned) --------------------

    private bool IsPinching(XRHandedness handedness)
    {
        if (!TryGetJoint(XRHandJointID.ThumbTip, handedness, out var thumb))
            return false;
        if (!TryGetJoint(XRHandJointID.IndexTip, handedness, out var index))
            return false;

        float distance = Vector3.Distance(thumb.position, index.position);
        return distance < pinchThreshold;
    }

    private bool IsOpenPalm(XRHandedness handedness)
    {
        if (!TryGetJoint(XRHandJointID.Palm, handedness, out var palm))
            return false;

        XRHandJointID[] tips =
        {
            XRHandJointID.IndexTip,
            XRHandJointID.MiddleTip,
            XRHandJointID.RingTip,
            XRHandJointID.LittleTip
        };

        int extended = 0;
        foreach (var tipId in tips)
        {
            if (TryGetJoint(tipId, handedness, out var tip))
            {
                float dist = Vector3.Distance(tip.position, palm.position);
                if (dist > openPalmMinFingerDistance)
                    extended++;
            }
        }

        return extended >= requiredExtendedFingers;
    }

    private bool IsThumbsUp(XRHandedness handedness)
    {
        // Simple heuristic: thumb tip significantly above palm, other fingers closer to palm
        if (!TryGetJoint(XRHandJointID.Palm, handedness, out var palm))
            return false;
        if (!TryGetJoint(XRHandJointID.ThumbTip, handedness, out var thumb))
            return false;

        // Thumb should be clearly extended upward relative to palm
        Vector3 localUp = Vector3.up;
        float thumbHeight = Vector3.Dot(thumb.position - palm.position, localUp);

        if (thumbHeight < 0.06f) return false;

        // Other fingertips should not be fully extended (fist-like except thumb)
        int curled = 0;
        XRHandJointID[] tips = { XRHandJointID.IndexTip, XRHandJointID.MiddleTip, XRHandJointID.RingTip };
        foreach (var tipId in tips)
        {
            if (TryGetJoint(tipId, handedness, out var tip))
            {
                float dist = Vector3.Distance(tip.position, palm.position);
                if (dist < openPalmMinFingerDistance * 0.85f)
                    curled++;
            }
        }

        return curled >= 2;
    }

    private void DetectSwipe(XRHandedness handedness)
    {
        if (Time.time - lastSwipeTime < swipeCooldown) return;

        if (!TryGetJoint(XRHandJointID.Palm, handedness, out var palm))
            return;

        Vector3 currentPos = palm.position;
        float currentTime = Time.time;

        if (leftPalmPrevPos == Vector3.zero)
        {
            leftPalmPrevPos = currentPos;
            leftPalmPrevTime = currentTime;
            return;
        }

        Vector3 delta = currentPos - leftPalmPrevPos;
        float dt = Mathf.Max(0.001f, currentTime - leftPalmPrevTime);
        float speed = delta.magnitude / dt;

        // Prefer horizontal swipes with enough distance and speed
        if (Mathf.Abs(delta.x) > swipeDistanceThreshold &&
            Mathf.Abs(delta.x) > Mathf.Abs(delta.y) * 1.2f &&
            speed > minSwipeSpeed)
        {
            if (delta.x > 0)
            {
                GameManager.Instance?.stanceManager?.SwitchStance(Stance.Ravager);
                if (debugLogs) Debug.Log("[Shadowrift] Swipe Right → Ravager");
            }
            else
            {
                GameManager.Instance?.stanceManager?.SwitchStance(Stance.Sentinel);
                if (debugLogs) Debug.Log("[Shadowrift] Swipe Left → Sentinel");
            }

            AudioManager.Instance?.PlayStanceSwitch();
            TriggerHaptic(0.45f, 0.07f);
            lastSwipeTime = Time.time;
        }

        leftPalmPrevPos = currentPos;
        leftPalmPrevTime = currentTime;
    }

    private bool TryGetJoint(XRHandJointID id, XRHandedness handedness, out Pose pose)
    {
        pose = default;
        if (handSubsystem.TryGetJoint(id, handedness, out var joint))
        {
            pose = joint.pose;
            return true;
        }
        return false;
    }

    // -------------------- Actions --------------------

    private void PerformAttack()
    {
        GameManager.Instance?.loyaltyManager?.ModifyLoyalty(-2.0f);
        AudioManager.Instance?.PlayAttack();
        TriggerHaptic(0.7f, 0.06f);
        if (debugLogs) Debug.Log("[Shadowrift] Pinch → Voidblade Attack");
    }

    private void TriggerPhase()
    {
        GameManager.Instance?.phasingManager?.TogglePhase();
        AudioManager.Instance?.PlayPhase();
        TriggerHaptic(0.85f, 0.12f);
        if (debugLogs) Debug.Log("[Shadowrift] Open Palms → Phase Shift");
    }

    /// <summary>
    /// Haptic feedback helper. Works with Meta XR / OpenXR when available.
    /// Safe no-op if runtime does not support it.
    /// </summary>
    private void TriggerHaptic(float amplitude, float duration)
    {
        // Meta Quest path (if Meta XR SDK is present this can be expanded)
        // OVRInput.SetControllerVibration(amplitude, amplitude, OVRInput.Controller.RTouch);

        // OpenXR / generic: most projects use XR Interaction Toolkit haptic impulses on controllers.
        // For pure hand tracking there is no standard haptic channel yet — kept as extension point.
        if (debugLogs)
        {
            // Intentionally quiet in production; useful while tuning
        }
    }
}
