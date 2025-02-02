using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private float nextWallJumpTime = 0f;
    public float wallJumpCooldown = 0.2f;
    public float wallSlideSpeed = -2f; // Maximum downward speed when wall sliding

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Horizontal Movement
        float moveInput = Input.GetAxis("Horizontal");
        if (!isWallSliding) // Allow movement if not actively wall sliding
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        // Wall Sliding
        if (isTouchingWall && !isGrounded && rb.linearVelocity.y < 0)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }

        // Jumping
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (isWallSliding && Time.time >= nextWallJumpTime)
            {
                // Perform a wall jump
                float jumpDirection = -Mathf.Sign(transform.localScale.x); // Jump away from the wall
                rb.linearVelocity = new Vector2(jumpDirection * moveSpeed, jumpForce);
                isWallSliding = false; // Exit wall sliding state
                nextWallJumpTime = Time.time + wallJumpCooldown; // Add cooldown to wall jumping
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            isTouchingWall = false;
            isWallSliding = false; // Exit wall sliding when leaving the wall
        }
    }

    public void TestMove(float moveInput)
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    public void TestJump()
    {
        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    public void SetGrounded(bool grounded)
    {
        isGrounded = grounded;
    }

}
