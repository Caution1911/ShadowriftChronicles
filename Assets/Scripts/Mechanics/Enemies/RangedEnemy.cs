using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public string nativeLayer = "phase";
    public float attackRange = 8f;
    public float fireRate = 1.8f;
    public int health = 4;
    public GameObject projectilePrefab;

    private float nextFireTime;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        string playerLayer = GameManager.Instance.phasingManager.CurrentLayer;

        if (nativeLayer == playerLayer && distance < attackRange && Time.time > nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        proj.GetComponent<Projectile>()?.Initialize(direction, nativeLayer);
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
