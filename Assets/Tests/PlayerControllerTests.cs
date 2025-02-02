using NUnit.Framework;
using UnityEngine;

public class PlayerControllerTests
{
    private GameObject player;
    private PlayerController controller;
    private Rigidbody2D rb;

    [SetUp]
    public void Setup()
    {
        // Create a mock player object and attach necessary components
        player = new GameObject("Player");
        controller = player.AddComponent<PlayerController>();
        rb = player.AddComponent<Rigidbody2D>();

        // Manually initialize the Rigidbody reference in the controller
        var rbField = typeof(PlayerController).GetField("rb", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        rbField.SetValue(controller, rb);

        controller.moveSpeed = 5f;
        controller.jumpForce = 10f;
    }


    [Test]
    public void PlayerCanMoveHorizontally()
    {
        controller.TestMove(1f); // Simulate moving right
        Assert.AreEqual(5f, rb.linearVelocity.x, "Player failed to move horizontally.");
    }

    [Test]
    public void PlayerCanJump()
    {
        controller.SetGrounded(true); // Simulate grounded state
        controller.TestJump(); // Simulate jump
        Assert.AreEqual(10f, rb.linearVelocity.y, "Player failed to jump.");
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.Destroy(player);
    }
}
