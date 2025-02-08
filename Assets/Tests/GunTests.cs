using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;


public class GunTests
{
    private GameObject gun;
    private GameObject player;
    private Transform firePoint;
    private GunPickup gunScript;
    private GameObject bulletPrefab;

    [SetUp]
    public void Setup()
    {
        //Mock player
        player = new GameObject("Player");
        player.AddComponent<Rigidbody2D>();

        //mock gun
        gun = new GameObject("Gun");
        gunScript = gun.AddComponent<GunPickup>();

        //mock firePoint
        firePoint = new GameObject("FirePoint").transform;
        firePoint.parent = gun.transform;
        firePoint.position = new Vector3(1f, 0, 0); //sets forward position

        //mock bullet prefab
        bulletPrefab = new GameObject("Bullet");
        bulletPrefab.AddComponent<Rigidbody2D>();
        bulletPrefab.AddComponent<Bullet>();

        //assign variables in script
        gunScript.bulletPrefab = bulletPrefab;
        gunScript.firePoint = firePoint;
    }

    [Test]
    public void PlayerCanShootBullets()
    {
        int initialBulletCount = GameObject.FindObjectsByType<Bullet>(FindObjectsSortMode.None).Length;

        gunScript.Shoot(); // Simulate shooting

        int newBulletCount = GameObject.FindObjectsByType<Bullet>(FindObjectsSortMode.None).Length;

        Assert.Greater(newBulletCount, initialBulletCount, "No bullets were spawned when shooting.");
    }


    [Test]
    public void BulletMovesForward()
    {
        GameObject bullet = GameObject.Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        bulletScript.Initialize(Vector2.right, player); // Fire right

        float initialX = bullet.transform.position.x;

        // Simulate movement manually
        for (int i = 0; i < 10; i++)
        {
            bullet.transform.position += (Vector3)Vector2.right * bulletScript.speed * Time.fixedDeltaTime;
        }

        Assert.Greater(bullet.transform.position.x, initialX, "Bullet did not move forward.");
    }


    [Test]
public void BulletDisappearsOnCollision()
{
    bool bulletWasDestroyed = false;
    
    Bullet.OnBulletDestroyed += () => bulletWasDestroyed = true; // Listen for event

    GameObject bullet = GameObject.Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
    Bullet bulletScript = bullet.GetComponent<Bullet>();

    GameObject wall = new GameObject("Wall");
    wall.AddComponent<BoxCollider2D>();

    bulletScript.OnTriggerEnter2D(wall.GetComponent<Collider2D>());

    Assert.IsTrue(bulletWasDestroyed, "Bullet did not trigger destruction event.");
}



    [TearDown]
    public void TearDown()
    {
        GameObject.Destroy(player);
        GameObject.Destroy(gun);
        GameObject.Destroy(bulletPrefab);
    }
}
