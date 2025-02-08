using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float stopDistance = 3f;
    public float shootingCooldown = 1.5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private Transform player;
    private Rigidbody2D rb;
    private float lastShotTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        if (player)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer > stopDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.linearVelocity = direction * moveSpeed;
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
                if (Time.time >= lastShotTime + shootingCooldown)
                {
                    Shoot();
                    lastShotTime = Time.time;
                }
            }
        }
    }

    private void Shoot()
    {
        if (bulletPrefab && firePoint)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Vector2 shootDirection = (player.position - transform.position).normalized;

            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript)
            {
                bulletScript.Initialize(shootDirection,gameObject);
            }
            // Collider2D bulletCollider = bullet.GetComponent<Collider2D>();
            // Collider2D enemyCollider = GetComponent<Collider2D>();
            // if (bulletCollider && enemyCollider)
            // {
            //     Physics2D.IgnoreCollision(bulletCollider, enemyCollider);
            // }
            Debug.Log("Enemy shot a bullet!"); // Debug log to confirm shooting

        }
    }
}
