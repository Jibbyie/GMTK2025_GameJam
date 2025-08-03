using System.Collections.Generic;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawn Values")]
    [SerializeField] private GameObject enemyPrefab;
    [Tooltip("Defining radius around the spawner where enemies can appear")][SerializeField] private float spawnRadius = 10f;
    [Tooltip("Ensure enemies don't spawn on top of each other")][SerializeField] private float minDistanceBetweenEnemies = 2f;
    [Tooltip("Int for maximum number of enemies that can be alive at once")][SerializeField] private int maxActiveEnemies = 2;
    [Tooltip("A Int for total number of enemies this spawner can create")][SerializeField] private float totalSpawnLimit = 5f;
    [Tooltip("A float for time between each spawn attempt")][SerializeField] private float spawnInterval = 1.5f;
    [Tooltip("Defines a collider in which if the player is detected, the spawner will start spawning enemies")][SerializeField] private EnemySpawnerDetectionZone playerDetectionZone;

    [Header("Enemy Internal Values")]
    [Tooltip("Count down time to next spawn")][SerializeField] private float spawnTimer;
    [Tooltip("Track how many enemies have been created so far")][SerializeField] private int totalEnemiesSpawned = 0;
    [SerializeField] private List<GameObject> activeEnemiesList;

    private void Awake()
    {
        activeEnemiesList = new List<GameObject>();
        spawnTimer = spawnInterval;
    }
    private void Update()
    {
        if (playerDetectionZone.playerDetected)
        {
            for (int i = activeEnemiesList.Count - 1; i >= 0; i--) // counts backwards in activenemieslist
            {
                if (activeEnemiesList[i] == null) // if an enemy was killed and is now null
                {
                    activeEnemiesList.RemoveAt(i); // remove this enemy
                }
            }
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0)
            {
                spawnTimer = spawnInterval;
                if (activeEnemiesList.Count < maxActiveEnemies) // if list of active enemies less than max active enemies
                {
                    if (totalEnemiesSpawned < totalSpawnLimit) // if total enemies spawned less than max enemies spawnable
                    {
                        SpawnEnemy(); // spawn enemy
                    }
                }
            }
        }
    }
    private void SpawnEnemy()
    {
        // Random spawn position in a circle
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector2 spawnPosition = (Vector2)transform.position + randomOffset;

        if (activeEnemiesList.Count == 0) // if no enemies are spawned
        {
            // spawn a new enemy and increment counter
            var newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            activeEnemiesList.Add(newEnemy);
            totalEnemiesSpawned += 1;

            // if we've reached the total number of allowable spawnable enemies
            if (totalEnemiesSpawned == totalSpawnLimit)
            {
                DestroySpawner(); // Destroy this spawner
            }
            return; // exit function if an enemy is spawned
        }

        for (int i = 0; i < 10; i++) // Try 10 spawn attempts to prevent game from freezing
        {
            // Generate new random spawn position
            Vector2 newRandomOffset = Random.insideUnitCircle * spawnRadius;
            Vector2 newSpawnPosition = (Vector2)transform.position + newRandomOffset;
            bool isPositionValid = true; // assume first spawn position is valid

            foreach (GameObject enemy in activeEnemiesList) // iterate through every enemy in the list
            {
                if(Vector2.Distance(enemy.transform.position, newSpawnPosition) < minDistanceBetweenEnemies) // if the distance between enemy and new position is too small
                {
                    isPositionValid = false; // not a valid position
                    break; // reiterate through the above for loop
                }
            }

            if (isPositionValid) // if we've found a valid position
            {
                // instantiate enemy at new random spawn position
                var newEnemy = Instantiate(enemyPrefab, newSpawnPosition, Quaternion.identity);
                activeEnemiesList.Add(newEnemy);

                // Destroy spawner if total allowable spawnable enemies is reached
                totalEnemiesSpawned += 1;
                if(totalEnemiesSpawned == totalSpawnLimit)
                {
                    DestroySpawner();
                }
                return;
            }
        }
    }

    

    private void DestroySpawner()
    {
            Destroy(this.gameObject);
    }
}
