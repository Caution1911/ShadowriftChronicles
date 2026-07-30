using UnityEngine;

public class AggressiveEnemy : MonoBehaviour
{
    public string nativeLayer = "base";
    public float speed = 4.5f;
    public int health = 5;
    public int damage = 8;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        string playerLayer = GameManager.Instance.phasingManager.CurrentLayer;

        if (nativeLayer == playerLayer)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Random.insideUnitSphere * 0.5f * Time.deltaTime;
        }
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
