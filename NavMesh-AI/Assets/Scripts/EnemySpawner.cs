using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public Transform player;
    public int numberOfEnemiesToSpawn = 5;
    public float spawnDelay = 1f;
    public List<Enemy> enemyPrefabs = new List<Enemy>();
    public SpawnMethod enemySpawnMethod = SpawnMethod.RoundRobin;
    
    private Dictionary<int, ObjectPool> enemyObjectPools = new Dictionary<int, ObjectPool>();
    private NavMeshTriangulation triangulation;

    private void Awake()
    {
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            enemyObjectPools.Add(i, ObjectPool.CreateInstance(enemyPrefabs[i], numberOfEnemiesToSpawn));
        }
    }

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();
        Debug.Log("Started spawning enemy");
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        var wait = new WaitForSeconds(spawnDelay);
        var spawnedEnemies = 0;
        while (spawnedEnemies < numberOfEnemiesToSpawn)
        {
            if (enemySpawnMethod == SpawnMethod.RoundRobin)
            {
                SpawnRoundRobinEnemy(spawnedEnemies);
            }else if (enemySpawnMethod == SpawnMethod.Random)
            {
                SpawnRandomEnemy();
            }
            spawnedEnemies++;
            yield return wait;
        }
    }

    private void SpawnRoundRobinEnemy(int spawnedEnemies)
    {
        int spawnIndex = spawnedEnemies % enemyPrefabs.Count;
        DoSpawnEnemy(spawnIndex);
    }

    private void SpawnRandomEnemy()
    {
        int spawnIndex = UnityEngine.Random.Range(0, enemyPrefabs.Count);
        DoSpawnEnemy(spawnIndex);
    }

    private void DoSpawnEnemy(int spawnIndex)
    {
        Debug.Log("spaning the enemy");
        PoolableObject poolableObject = enemyObjectPools[spawnIndex].GetObject();

        if (poolableObject != null)
        {
            var enemy = poolableObject.GetComponent<Enemy>();
            
            var vertexIndex = Random.Range(0, triangulation.vertices.Length);

            NavMeshHit hit;
            if (NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out  hit, 2f, -1))
            {
                enemy.agent.Warp(hit.position);
                enemy.movement.targetPlayer = player;
                enemy.agent.enabled = true;
                enemy.movement.StartChasing();
            }
            else
            {
                Debug.LogError($"Unable to place NavMeshAgent on Navmesh. " +
                               $"Tried to use {triangulation.vertices[vertexIndex]}.");
            }
        }
        else
        {
            Debug.LogError($"Unable to fetch enemy of type {spawnIndex} from object pool. Out of objects?");
        }
    }

    public enum SpawnMethod
    {
        RoundRobin,
        Random
    }
}
