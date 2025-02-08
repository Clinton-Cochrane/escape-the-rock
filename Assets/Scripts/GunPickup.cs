using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class GunPickup : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    private bool isPickedUp = false;
    private Transform player;
    private Vector3 rightOffset = new Vector3(0.5f, 0f, 0); // Gun on right side
    private Vector3 leftOffset = new Vector3(-0.5f, 0f, 0); // Gun on left side
    private float lastDirection = 1f; // 1 = Right, -1 = Left

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isPickedUp)
        {
            isPickedUp = true;
            player = collision.transform;
            PlayerController playerController = player.GetComponent<PlayerController>(); // Reference to movement script
            AttachToPlayer();
        }
    }

    private void AttachToPlayer()
    {
        transform.SetParent(player);
        transform.SetLocalPositionAndRotation(rightOffset, Quaternion.identity);
    }

    private void Update()
    {
        if (isPickedUp && player != null)
        {
            // Get movement input from player
            float moveInput = Input.GetAxis("Horizontal");

            // Update last direction if the player is moving
            if (moveInput != 0) lastDirection = Mathf.Sign(moveInput); // 1 for right, -1 for left

            // Switch gun side based on last movement direction
            // Flip gun visually
            transform.SetLocalPositionAndRotation(lastDirection > 0 ? rightOffset : leftOffset, lastDirection > 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0));

            // Fire bullet on left mouse click
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + new Vector3(0.5f * lastDirection, 0, 0), Quaternion.identity);
        Vector2 shootDirection = new(lastDirection, 0);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript)
        {
            bulletScript.Initialize(shootDirection, gameObject); // Pass gun as the shooter
        }
    }



}
