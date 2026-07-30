using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 8f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    public void OnPhase()
    {
        GameManager.Instance?.phasingManager?.TogglePhase();
    }

    public void OnRavager()
    {
        GameManager.Instance?.stanceManager?.SwitchStance(Stance.Ravager);
    }

    public void OnSentinel()
    {
        GameManager.Instance?.stanceManager?.SwitchStance(Stance.Sentinel);
    }

    public void OnHarmonist()
    {
        GameManager.Instance?.stanceManager?.SwitchStance(Stance.Harmonist);
    }
}
