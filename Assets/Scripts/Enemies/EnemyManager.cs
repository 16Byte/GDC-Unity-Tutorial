using System.Collections;
using UnityEngine;

/// <summary>
/// Keeps the scene topped up with enemies.
/// One coroutine, running for the whole game: look around, spawn if we are short, wait, repeat.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int maxEnemies = 15;
    [SerializeField] private float spawnInterval = 0.5f;

    // x is left/right, y is forward/back.
    [SerializeField] private Vector2 spawnBounds = new Vector2(20f, 20f);

    private int enemiesAmount;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            enemiesAmount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;

            if (enemiesAmount < maxEnemies)
                SpawnOne();

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnOne()
    {
        // Random point on the flat ground around this object.
        // Note the 0 goes in the MIDDLE: x, y, z. y is up in Unity,
        // so putting the second random value there spawns enemies in the air.
        Vector3 offset = new Vector3(Random.Range(-spawnBounds.x, spawnBounds.x), 0f, Random.Range(-spawnBounds.y, spawnBounds.y));

        Instantiate(enemyPrefab, transform.position + offset, Quaternion.identity);
    }
}
