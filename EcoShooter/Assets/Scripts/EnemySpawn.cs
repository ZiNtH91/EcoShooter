using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemies;

    private float spawnTimer = 0;
    public float spawnCooldown = 2.5f;

    public int enemyCount_Min;
    public int enemyCount_Max;

    float spawnRadius = 12;

    void Start()
    {
        SpawnEnemies(2);
    }

    // Update is called once per frame
    void Update()
    {
        SpawnManager();
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
        for(int i = 0; i < enemyCount; i++)
        {
            // Determine Type of Enemy Randomly
            int enemyType = Random.Range(0, enemies.Length);


            // Determine Spawnposition Randomly
            float spawnAngle = Random.Range(0, 360);
            Vector2 spawnPosition;
            spawnPosition.x= spawnRadius * Mathf.Sin(spawnAngle * Mathf.Deg2Rad);
            spawnPosition.y = spawnRadius * Mathf.Cos(spawnAngle * Mathf.Deg2Rad);

            // Instantiate Enemy
            Instantiate(enemies[enemyType], spawnPosition, Quaternion.identity);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(Vector3.zero, spawnRadius);
    }

}
