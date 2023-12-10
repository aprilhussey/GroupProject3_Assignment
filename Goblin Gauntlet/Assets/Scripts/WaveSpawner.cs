using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [Tooltip("Enemies that can be spawned")]
	public List<Enemy> enemies = new List<Enemy>();

    [Tooltip("Currency to spend on enemies")]
    public int waveCurrency;  // Currency to spend on enemies
	[Tooltip("How long the wave lasts")]
	public int waveDuration;

	[Tooltip("How long the spawning should be initially delayed by")]
	public float delaySpawnBy;
    [Tooltip("The locations that the enemies can spawn at")]
    public List<Transform> spawnLocations;
    [Tooltip("The enemies that will be spawned - determined by the script")]
	public List<GameObject> enemiesToSpawn = new List<GameObject>();


	private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    // Start is called before the first frame update
    void Start()
    {
        GenerateWave();

        if (delaySpawnBy != null)
        {
            spawnTimer = delaySpawnBy;
        }
        else
        {
            spawnTimer = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnTimer <= 0)
        {
            // Spawn an enemy
            if (enemiesToSpawn.Count > 0)
            {
                // Select a random spawn location
                Transform spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Count)];

                Instantiate(enemiesToSpawn[0], spawnLocation.position, Quaternion.identity);
                enemiesToSpawn.RemoveAt(0);
                spawnTimer = spawnInterval;
            }
            else
            {
                waveTimer = 0;
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }
    }

    public void GenerateWave()
    {
        if (waveCurrency != null)
        {
            GenerateEnemies();

            spawnInterval = waveDuration / enemiesToSpawn.Count;    // Gives a float time between each enemy
            waveTimer = waveDuration;
        }
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();

        while (waveCurrency > 0)
        {
            int randomEnemyID = Random.Range(0, enemies.Count);
            int randomEnemyCost = enemies[randomEnemyID].cost;

            if (waveCurrency - randomEnemyCost >= 0)
            {
                generatedEnemies.Add(enemies[randomEnemyID].enemyPrefab);
                waveCurrency -= randomEnemyCost;
            }
            else if (waveCurrency <= 0)
            {
                break;
            }
        }
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int cost;
}
