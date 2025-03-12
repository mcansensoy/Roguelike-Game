using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject[] easyEnemies;
    public GameObject[] normalEnemies;
    public GameObject[] hardEnemies;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public GameObject spawnEffect;

    [Header("Spawn Numbers")]
    public int numEasyEnemies = 4;
    public int numNormalEnemies = 2;
    public int numHardEnemies = 0;

    [Header("Wave Settings")]
    public int totalWaves = 3;
    private int waves = 0;

    [Header("Timing")]
    public float spawnDelay = 0.75f; 

    [Header("Finish Trigger")]
    public GameObject finishTrigger; 

    private int totalEnemiesInWave; 
    [SerializeField] private int enemiesAlive; 

    private void Start()
    {
        finishTrigger.SetActive(false); 
        StartCoroutine(SpawnWave());
        waves = totalWaves;
    }

    // This will manage the wave spawning
    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < totalWaves; i++)
        {
            totalEnemiesInWave = numEasyEnemies + numNormalEnemies + numHardEnemies;
            enemiesAlive = totalEnemiesInWave; 

            yield return StartCoroutine(SpawnEnemies());

            yield return new WaitUntil(() => enemiesAlive <= 0);

            waves--;
            if (waves == 0)
            {
                EnableFinishTrigger();
                yield break; 
            }
        }
    }

    private IEnumerator SpawnEnemies()
    {
        int spawnIndex = 0;

        for (int i = 0; i < numEasyEnemies; i++)
        {
            GameObject selectedEnemy = easyEnemies[Random.Range(0, easyEnemies.Length)];
            yield return StartCoroutine(SpawnEnemyWithEffect(selectedEnemy, spawnIndex));
            spawnIndex = (spawnIndex + 1) % spawnPoints.Length; // Rotate through spawn points
        }

        for (int i = 0; i < numNormalEnemies; i++)
        {
            GameObject selectedEnemy = normalEnemies[Random.Range(0, normalEnemies.Length)];
            yield return StartCoroutine(SpawnEnemyWithEffect(selectedEnemy, spawnIndex));
            spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
        }

        for (int i = 0; i < numHardEnemies; i++)
        {
            GameObject selectedEnemy = hardEnemies[Random.Range(0, hardEnemies.Length)];
            yield return StartCoroutine(SpawnEnemyWithEffect(selectedEnemy, spawnIndex));
            spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
        }
    }

    private IEnumerator SpawnEnemyWithEffect(GameObject enemyPrefab, int spawnIndex)
    {
        Transform spawnPoint = spawnPoints[spawnIndex];

        GameObject effectInstance = Instantiate(spawnEffect, spawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(spawnDelay); 

        Destroy(effectInstance, 1.5f);

        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        // Subscribe to the enemy's death event (we assume the enemy has a method or event when it dies)
        Enemy enemyScript = enemyInstance.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.OnEnemyDeath += HandleEnemyDeath; // Add death event listener
        }
    }

    private void HandleEnemyDeath()
    {
        enemiesAlive--;
    }

    private void EnableFinishTrigger()
    {
        Debug.Log("Finish");
        finishTrigger.SetActive(true);
    }
}
