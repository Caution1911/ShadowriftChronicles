using UnityEngine;
using UnityEngine.XR.Hands;
using System.Collections.Generic;

public class QuestHandGestureManager : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;
    public float pinchThreshold = 0.04f;
    public float swipeDistanceThreshold = 0.15f;
    public float swipeCooldown = 0.6f;

    private Vector3 leftHandPrevPos;
    private float lastSwipeTime;

    private void Start()
    {
        var list = new List<XRHandSubsystem>();
        SubsystemManager.GetSubsystems(list);
        if (list.Count > 0) handSubsystem = list[0];
    }

    private void Update()
    {
        if (handSubsystem == null || !handSubsystem.running) return;

        if (IsPinch(XRHandedness.Right))
        {
            PerformAttack();
        }

        DetectSwipe(XRHandedness.Left);

        if (IsOpenPalm(XRHandedness.Left) && IsOpenPalm(XRHandedness.Right))
        {
            GameManager.Instance?.phasingManager?.TogglePhase();
        }
    }

    private bool IsPinch(XRHandedness hand)
    {
        if (handSubsystem.TryGetJoint(XRHandJointID.ThumbTip, hand, out var thumb) &&
            handSubsystem.TryGetJoint(XRHandJointID.IndexTip, hand, out var index))
        {
            return Vector3.Distance(thumb.pose.position, index.pose.position) < pinchThreshold;
        }
        return false;
    }

    private bool IsOpenPalm(XRHandedness hand)
    {
        return true;
    }

    private void DetectSwipe(XRHandedness hand)
    {
        if (Time.time - lastSwipeTime < swipeCooldown) return;

        if (handSubsystem.TryGetJoint(XRHandJointID.Palm, hand, out var palm))
        {
            Vector3 currentPos = palm.pose.position;
            Vector3 delta = currentPos - leftHandPrevPos;

            if (Mathf.Abs(delta.x) > swipeDistanceThreshold)
            {
                if (delta.x > 0)
                    GameManager.Instance?.stanceManager?.SwitchStance(Stance.Ravager);
                else
                    GameManager.Instance?.stanceManager?.SwitchStance(Stance.Sentinel);

                lastSwipeTime = Time.time;
            }
            leftHandPrevPos = currentPos;
        }
    }

    private void PerformAttack()
    {
        GameManager.Instance?.loyaltyManager?.ModifyLoyalty(-2.5f);
        Debug.Log("Voidblade Attack!");
    }
}
