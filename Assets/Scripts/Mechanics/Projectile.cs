using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 12f;
    public int damage = 3;
    public float lifetime = 3f;
    public string ownerLayer = "phase";

    private Vector2 direction;

    public void Initialize(Vector2 dir, string layer)
    {
        direction = dir.normalized;
        ownerLayer = layer;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            string playerLayer = GameManager.Instance.phasingManager.CurrentLayer;
            if (playerLayer == ownerLayer)
            {
                GameManager.Instance.loyaltyManager.ModifyLoyalty(-5f);
                other.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
