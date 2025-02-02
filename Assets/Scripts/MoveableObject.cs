using UnityEngine;

public class CabinetMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    private bool isAttached = false;

    public float moveSpeed = 10f; // Fine-tuned for responsiveness
    public float followOffsetX = 0.2f; // Slightly increase to ensure proper alignment
    public float putDownOffset = 1.0f; // Distance to place cabinet when put down

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.mass = 3f;
        rb.freezeRotation = true;
        rb.gravityScale = 1f; // Enable gravity when detached
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void FixedUpdate()
    {
        if (isAttached && player != null)
        {
            // Directly set target position to be exactly behind/next to the player
            float targetX = player.position.x + (player.localScale.x > 0 ? -followOffsetX : followOffsetX);
            Vector2 targetPosition = new Vector2(targetX, transform.position.y);

            // Use MovePosition directly without Lerp to prevent lag
            rb.MovePosition(targetPosition);
        }
    }

    public void Update()
    {
        if (player == null) return;

        // Press "E" to attach/detach
        if (Input.GetKeyDown(KeyCode.E))
        {
            isAttached = !isAttached;
            Debug.Log("E Pressed - Toggled Attachment: " + isAttached);

            if (isAttached)
            {
                Attach();
            }
            else
            {
                PutDown();
            }
        }
    }

    private void Attach()
    {
        if (player == null) return;

        // Instantly place the cabinet at the correct position before moving
        transform.position = new Vector2(player.position.x + followOffsetX, transform.position.y);

        // Stop existing movement before attaching
        rb.linearVelocity = Vector2.zero;

        // Disable gravity while attached
        rb.gravityScale = 0;
    }

    private void PutDown()
    {
        isAttached = false;
        rb.gravityScale = 1f; // Re-enable gravity

        // Place the cabinet in front of the player instead of dropping it on their head
        float dropX = player.position.x + (player.localScale.x > 0 ? putDownOffset : -putDownOffset);
        transform.position = new Vector2(dropX, transform.position.y);

        rb.linearVelocity = Vector2.zero; // Stop movement
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isAttached)
        {
            player = null; // Remove reference only if not attached
        }
    }
}
