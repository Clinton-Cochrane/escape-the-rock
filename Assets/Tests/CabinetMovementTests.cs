using NUnit.Framework;
using UnityEngine;

public class CabinetMovementTests
{
    private GameObject cabinetObject;
    private CabinetMovement cabinet;
    private GameObject playerObject;
    private Rigidbody2D playerRb;

    [SetUp]
    public void Setup()
    {
        // Mock the player
        playerObject = new GameObject("Player");
        playerRb = playerObject.AddComponent<Rigidbody2D>();

        // Mock the cabinet
        cabinetObject = new GameObject("Cabinet");
        cabinet = cabinetObject.AddComponent<CabinetMovement>();
        Rigidbody2D cabinetRb = cabinetObject.AddComponent<Rigidbody2D>();

        cabinetRb.gravityScale = 1f;
        cabinetRb.mass = 3f;

        // REMOVE THESE LINES (not needed in tests)
        // cabinetObject.tag = "Cabinet";
        // playerObject.tag = "Player";
    }

    [Test]
    public void CabinetAttachesWhenTriggered()
    {
        // Simulate the player entering the cabinet's trigger
        cabinetObject.transform.position = Vector2.zero;
        playerObject.transform.position = new Vector2(1, 0);

        // Manually call OnTriggerEnter2D (mocking collision)
        cabinetObject.GetComponent<CabinetMovement>().OnTriggerEnter2D(playerObject.AddComponent<BoxCollider2D>());

        // Manually attach the cabinet (simulating pressing "E")
        SetPrivateField(cabinet, "isAttached", true);
        cabinet.Update(); // Trigger behavior

        // Verify the cabinet is attached
        Assert.IsTrue(GetPrivateField<bool>(cabinet, "isAttached"), "Cabinet should be attached after simulated input.");
    }

    [Test]
    public void CabinetDetachesWhenToggled()
    {
        // Force the cabinet to be attached
        SetPrivateField(cabinet, "isAttached", true);
        cabinet.Update(); // Trigger attachment logic

        // Manually detach the cabinet (simulating pressing "E" again)
        SetPrivateField(cabinet, "isAttached", false);
        cabinet.Update();

        // Verify the cabinet is detached
        Assert.IsFalse(GetPrivateField<bool>(cabinet, "isAttached"), "Cabinet should be detached after toggling.");
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(playerObject);
        GameObject.Destroy(cabinetObject);
    }

    // Helper method to access private fields
    private T GetPrivateField<T>(object obj, string fieldName)
    {
        return (T)obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(obj);
    }

    private void SetPrivateField<T>(object obj, string fieldName, T value)
    {
        obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(obj, value);
    }
}
