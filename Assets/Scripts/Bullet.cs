using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; // Bullet speed
    public float damage = 20f;
    private Rigidbody2D rb;
    private Vector2 direction;
    public delegate void BulletDestroyedHandler();
    public static event BulletDestroyedHandler OnBulletDestroyed;

    public void Initialize(Vector2 shootDirection)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // No gravity for bullets
        direction = shootDirection.normalized;
        rb.linearVelocity = direction * speed; // Move bullet
        transform.localScale = new Vector3(0.1f, 0.1f, 1f); //bullet size
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) // Ignore player collisions
        {
            Debug.Log("Bullet hit: " + collision.gameObject.name);
            Health targetHealth = collision.GetComponent<Health>();
            if(targetHealth)
            {
                targetHealth.TakeDamage(damage);
            }
            OnBulletDestroyed?.Invoke(); // Trigger event before destroying
            Destroy(gameObject); // Remove bullet on impact
        }
    }
}
