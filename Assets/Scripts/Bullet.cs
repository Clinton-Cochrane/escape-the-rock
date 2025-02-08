using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 20f;
    private Rigidbody2D rb;
    private Vector2 direction;
    private GameObject shooter;
    public delegate void BulletDestroyedHandler();
    public static event BulletDestroyedHandler OnBulletDestroyed;

    public void Initialize(Vector2 shootDirection, GameObject shooter)
    {
        this.shooter = shooter;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // No gravity for bullets
        direction = shootDirection.normalized;
        rb.linearVelocity = direction * speed;
        transform.localScale = new Vector3(0.1f, 0.1f, 1f); // Bullet size

        // Ignore collisions with shooter & shooter's children (like the gun)
        Collider2D[] shooterColliders = shooter.GetComponentsInChildren<Collider2D>();
        Collider2D bulletCollider = GetComponent<Collider2D>();

        foreach (var collider in shooterColliders)
        {
            if (bulletCollider && collider)
            {
                Physics2D.IgnoreCollision(bulletCollider, collider);
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (shooter != null && (collision.gameObject == shooter || collision.transform.IsChildOf(shooter.transform)))
        {
            return; // Ignore bullets hitting the shooter's own body or gun
        }

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Bullet hit player!");
            if (collision.TryGetComponent<PlayerHealth>(out var playerHealth))
            {
                playerHealth.TakeDamage(damage); // ✅ Apply damage
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Bullet hit enemy!");
            if (collision.TryGetComponent<Health>(out var enemyHealth))
            {
                enemyHealth.TakeDamage(damage); // ✅ Apply damage
            }
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Bullet hit: " + collision.gameObject.name);
            OnBulletDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}
