using UnityEngine;
using UnityEngine.XR.Hands;
using System.Collections.Generic;

/// <summary>
/// Performance-optimized Meta Quest 3 hand tracking for Shadowrift Chronicles.
/// Goals: minimal per-frame cost, zero GC in steady state, reliable gestures.
/// </summary>
public class QuestHandGestureManager : MonoBehaviour
{
    [Header("Subsystem")]
    private XRHandSubsystem handSubsystem;
    private bool subsystemReady;

    [Header("Pinch (Attack)")]
    public float pinchThreshold = 0.028f;
    public float attackCooldown = 0.35f;

    [Header("Swipe (Stance)")]
    public float swipeDistanceThreshold = 0.16f;
    public float swipeCooldown = 0.65f;
    public float minSwipeSpeed = 0.8f;

    [Header("Open Palm (Phase)")]
    public float openPalmMinFingerDistance = 0.055f;
    public int requiredExtendedFingers = 3;
    public float phaseCooldown = 0.9f;
    public float openPalmHoldTime = 0.25f;

    [Header("Thumbs Up (Harmonist)")]
    public float thumbsUpCooldown = 0.8f;

    [Header("Performance")]
    [Tooltip("How often non-critical gestures are evaluated (seconds). Attack/pinch stays every frame.")]
    public float secondaryGestureInterval = 0.033f; // ~30 Hz
    public bool debugLogs = false;

    // Timing
    private float lastSwipeTime;
    private float lastAttackTime;
    private float lastPhaseTime;
    private float lastThumbsUpTime;
    private float openPalmTimer;
    private float nextSecondaryCheckTime;

    // Swipe tracking (no allocations)
    private Vector3 leftPalmPrevPos;
    private float leftPalmPrevTime;
    private bool hasLeftPalmPrev;

    // Cached poses (reused every frame)
    private Pose cachedPalm;
    private Pose cachedThumb;
    private Pose cachedIndex;
    private Pose cachedMiddle;
    private Pose cachedRing;
    private Pose cachedLittle;

    private void Start()
    {
        ResolveSubsystem();
        leftPalmPrevTime = Time.time;
        nextSecondaryCheckTime = 0f;
    }

    private void OnEnable()
    {
        // Re-resolve if domain reload / subsystem restart
        if (!subsystemReady)
            ResolveSubsystem();
    }

    private void ResolveSubsystem()
    {
        var subsystems = new List<XRHandSubsystem>(1);
        SubsystemManager.GetSubsystems(subsystems);
        if (subsystems.Count > 0)
        {
            handSubsystem = subsystems[0];
            subsystemReady = true;
            if (debugLogs)
                Debug.Log("[Shadowrift] Hand subsystem ready.");
        }
        else
        {
            subsystemReady = false;
            handSubsystem = null;
            if (debugLogs)
                Debug.LogWarning("[Shadowrift] No XRHandSubsystem. Check XR Hands + OpenXR Meta Quest Support.");
        }
    }

    private void Update()
    {
        // Cheap early-outs
        if (!subsystemReady || handSubsystem == null)
            return;

        if (!handSubsystem.running)
            return;

        float now = Time.time;

        // --- High priority: attack pinch every frame (responsive combat) ---
        if (now - lastAttackTime >= attackCooldown)
        {
            if (IsPinching(XRHandedness.Right))
            {
                PerformAttack();
                lastAttackTime = now;
            }
        }

        // --- Secondary gestures at reduced rate (~30 Hz) to save CPU ---
        if (now < nextSecondaryCheckTime)
            return;

        nextSecondaryCheckTime = now + secondaryGestureInterval;

        DetectSwipe(XRHandedness.Left, now);
        UpdateOpenPalmPhase(now);

        if (now - lastThumbsUpTime >= thumbsUpCooldown && IsThumbsUp(XRHandedness.Right))
        {
            GameManager.Instance?.stanceManager?.SwitchStance(Stance.Harmonist);
            AudioManager.Instance?.PlayStanceSwitch();
            lastThumbsUpTime = now;
            if (debugLogs)
                Debug.Log("[Shadowrift] Thumbs Up → Harmonist");
        }
    }

    // -------------------- Gestures (allocation-free) --------------------

    private bool IsPinching(XRHandedness hand)
    {
        if (!TryGetPose(XRHandJointID.ThumbTip, hand, ref cachedThumb))
            return false;
        if (!TryGetPose(XRHandJointID.IndexTip, hand, ref cachedIndex))
            return false;

        float dx = cachedThumb.position.x - cachedIndex.position.x;
        float dy = cachedThumb.position.y - cachedIndex.position.y;
        float dz = cachedThumb.position.z - cachedIndex.position.z;
        float distSq = dx * dx + dy * dy + dz * dz;
        float thresh = pinchThreshold * pinchThreshold;
        return distSq < thresh;
    }

    private bool IsOpenPalm(XRHandedness hand)
    {
        if (!TryGetPose(XRHandJointID.Palm, hand, ref cachedPalm))
            return false;

        int extended = 0;
        float minDist = openPalmMinFingerDistance;
        float minDistSq = minDist * minDist;

        if (TryGetPose(XRHandJointID.IndexTip, hand, ref cachedIndex) &&
            DistanceSq(cachedIndex.position, cachedPalm.position) > minDistSq)
            extended++;

        if (TryGetPose(XRHandJointID.MiddleTip, hand, ref cachedMiddle) &&
            DistanceSq(cachedMiddle.position, cachedPalm.position) > minDistSq)
            extended++;

        if (TryGetPose(XRHandJointID.RingTip, hand, ref cachedRing) &&
            DistanceSq(cachedRing.position, cachedPalm.position) > minDistSq)
            extended++;

        if (TryGetPose(XRHandJointID.LittleTip, hand, ref cachedLittle) &&
            DistanceSq(cachedLittle.position, cachedPalm.position) > minDistSq)
            extended++;

        return extended >= requiredExtendedFingers;
    }

    private bool IsThumbsUp(XRHandedness hand)
    {
        if (!TryGetPose(XRHandJointID.Palm, hand, ref cachedPalm))
            return false;
        if (!TryGetPose(XRHandJointID.ThumbTip, hand, ref cachedThumb))
            return false;

        // Thumb clearly above palm in world up
        float thumbHeight = cachedThumb.position.y - cachedPalm.position.y;
        if (thumbHeight < 0.06f)
            return false;

        float curlLimit = openPalmMinFingerDistance * 0.85f;
        float curlLimitSq = curlLimit * curlLimit;
        int curled = 0;

        if (TryGetPose(XRHandJointID.IndexTip, hand, ref cachedIndex) &&
            DistanceSq(cachedIndex.position, cachedPalm.position) < curlLimitSq)
            curled++;

        if (TryGetPose(XRHandJointID.MiddleTip, hand, ref cachedMiddle) &&
            DistanceSq(cachedMiddle.position, cachedPalm.position) < curlLimitSq)
            curled++;

        if (TryGetPose(XRHandJointID.RingTip, hand, ref cachedRing) &&
            DistanceSq(cachedRing.position, cachedPalm.position) < curlLimitSq)
            curled++;

        return curled >= 2;
    }

    private void DetectSwipe(XRHandedness hand, float now)
    {
        if (now - lastSwipeTime < swipeCooldown)
            return;

        if (!TryGetPose(XRHandJointID.Palm, hand, ref cachedPalm))
            return;

        Vector3 currentPos = cachedPalm.position;

        if (!hasLeftPalmPrev)
        {
            leftPalmPrevPos = currentPos;
            leftPalmPrevTime = now;
            hasLeftPalmPrev = true;
            return;
        }

        Vector3 delta = currentPos - leftPalmPrevPos;
        float dt = now - leftPalmPrevTime;
        if (dt < 0.001f)
            dt = 0.001f;

        float speed = delta.magnitude / dt;
        float absX = delta.x < 0f ? -delta.x : delta.x;
        float absY = delta.y < 0f ? -delta.y : delta.y;

        if (absX > swipeDistanceThreshold &&
            absX > absY * 1.2f &&
            speed > minSwipeSpeed)
        {
            if (delta.x > 0f)
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
            lastSwipeTime = now;
        }

        leftPalmPrevPos = currentPos;
        leftPalmPrevTime = now;
    }

    private void UpdateOpenPalmPhase(float now)
    {
        bool bothOpen = IsOpenPalm(XRHandedness.Left) && IsOpenPalm(XRHandedness.Right);

        if (bothOpen)
        {
            openPalmTimer += secondaryGestureInterval; // approximate hold time at reduced rate
            if (openPalmTimer >= openPalmHoldTime && now - lastPhaseTime >= phaseCooldown)
            {
                TriggerPhase();
                lastPhaseTime = now;
                openPalmTimer = 0f;
            }
        }
        else
        {
            openPalmTimer = 0f;
        }
    }

    // -------------------- Math / joint helpers --------------------

    private static float DistanceSq(Vector3 a, Vector3 b)
    {
        float dx = a.x - b.x;
        float dy = a.y - b.y;
        float dz = a.z - b.z;
        return dx * dx + dy * dy + dz * dz;
    }

    private bool TryGetPose(XRHandJointID id, XRHandedness hand, ref Pose pose)
    {
        if (handSubsystem.TryGetJoint(id, hand, out var joint))
        {
            // Prefer tracked joints only when possible
            if (joint.TryGetPose(out pose))
                return true;

            // Fallback: some runtimes still fill joint.pose
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
        if (debugLogs)
            Debug.Log("[Shadowrift] Pinch → Attack");
    }

    private void TriggerPhase()
    {
        GameManager.Instance?.phasingManager?.TogglePhase();
        AudioManager.Instance?.PlayPhase();
        if (debugLogs)
            Debug.Log("[Shadowrift] Open Palms → Phase");
    }
}
