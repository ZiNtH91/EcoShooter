using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;
    private float[] enemySpawnProbabilities;
    private int snakeSpawnWeight = 85;
    private int mouseSpawnWeight = 10;
    private int ravenSpawnWeight = 5;

    private float spawnTimer = 0;
    private float spawnCooldown = 5f;

    private int enemyCount_Min=4;
    private int enemyCount_Max=4;

    float spawnRadius = 12;

    void Start()
    {
        enemySpawnProbabilities = EnemySpawnProbabilities(new int[3] { snakeSpawnWeight, mouseSpawnWeight, ravenSpawnWeight });
        Debug.Log(enemySpawnProbabilities[0]);
        Debug.Log(enemySpawnProbabilities[1]);
        Debug.Log(enemySpawnProbabilities[2]);
        SpawnEnemies(2);
    }

    // Update is called once per frame
    void Update()
    {
        
        SpawnManager();
    }

    private float[] EnemySpawnProbabilities(int[] spawnWeights)
    {
        int _sum = spawnWeights.Sum();
        float _sum_i = 0;
        float[] _probabilities = new float[spawnWeights.Length];
        for (int i = 0; i < spawnWeights.Length; i++)
        {
            _sum_i += (float)spawnWeights[i];
            _probabilities[i] = _sum_i / _sum;
        }
        return _probabilities;
    }

    private void SpawnManager()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer > spawnCooldown)
        {
            int enemyCount = Random.Range(enemyCount_Min, enemyCount_Max + 1);
            SpawnEnemies(enemyCount);
            spawnTimer = 0;
        }
    }
    
    void SpawnEnemies(int enemyCount)
    {
        Debug.Log("Attempt to Spawn");
        for(int i = 0; i < enemyCount; i++)
        {
            // Determine Type of Enemy Randomly using Probabilities based on Spawn Weights
            int enemyIndex = GetEnemyIndex(enemies.Length, enemySpawnProbabilities);
      
            // Determine Spawnposition Randomly
            float spawnAngle = Random.Range(0, 360);
            Vector2 spawnPosition;
            spawnPosition.x= spawnRadius * Mathf.Sin(spawnAngle * Mathf.Deg2Rad);
            spawnPosition.y = spawnRadius * Mathf.Cos(spawnAngle * Mathf.Deg2Rad);

            // Instantiate Enemy
            Instantiate(enemies[enemyIndex], spawnPosition, Quaternion.identity);
        }
    }

    private int GetEnemyIndex(int numberOfEnemies, float[] spawnProbabilities)
    {
        // Determine Type of Enemy Randomly using Probabilities based on Spawn Weights
        float _enemyType = Random.Range(0f, 1f);
        int _enemyIndex = -1;
        for (int j = 0; j < numberOfEnemies; j++)
        {
            if (_enemyType <= enemySpawnProbabilities[j])
            {
                _enemyIndex = j;
                break;
            }
        }
        return _enemyIndex;
    }


    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Vector3.zero, spawnRadius);
    }

}
