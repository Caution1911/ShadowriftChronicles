using UnityEngine;

public class LoyaltyEnemy : MonoBehaviour
{
    public string nativeLayer = "base";
    public float speed = 3.2f;
    public int health = 6;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float loyalty = GameManager.Instance.loyaltyManager.loyalty;
        string playerLayer = GameManager.Instance.phasingManager.CurrentLayer;

        if (nativeLayer != playerLayer) return;

        if (loyalty > 70)
            speed = 1.5f;
        else if (loyalty < 30)
            speed = 5.5f;
        else
            speed = 3.2f;

        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            GetComponent<EnemyDeathHandler>()?.Die();
        }
    }
}
