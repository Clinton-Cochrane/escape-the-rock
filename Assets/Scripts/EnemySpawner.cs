using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Drag your enemy prefab here
    public Transform[] spawnPoints; // Assign spawn points in Inspector
    public float waveInterval = 15f; // Time between waves
    public int enemiesPerWave = 1;  // Start with 1 enemy

    private int waveCount = 0; // Track wave number

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true) // Infinite waves
        {
            waveCount++; // Increase wave number
            Debug.Log($"Wave {waveCount}: Spawning {enemiesPerWave} enemies.");

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
            }

            yield return new WaitForSeconds(waveInterval); // Wait for next wave
            waveInterval = Mathf.Min(waveInterval + 5f, 120f); // Increase wave delay
            enemiesPerWave++; // Increase enemy count per wave
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return; // Prevents errors if no spawn points

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}
