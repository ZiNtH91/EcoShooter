using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public GameObject[] powerUps;
    private float[] powerUpProbabilities;


    private int goldSpawnWeight = 40;
    private int healthSpawnWeight = 40;
    private int piercingSpawnWeight = 20;

    // Start is called before the first frame update

    void Start()
    {
        powerUpProbabilities = PowerUpSpawnProbabilities(new int[3] { goldSpawnWeight, healthSpawnWeight, piercingSpawnWeight });
    }

    public GameObject GetPowerUp()
    {
        int powerUpIndex = Random.Range(0, powerUps.Length);
        return powerUps[GetPowerUpIndex(powerUps.Length, powerUpProbabilities)];
    }

    private float[] PowerUpSpawnProbabilities(int[] spawnWeights)
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

    private int GetPowerUpIndex(int numberOfPowerUps, float[] spawnProbabilities)
    {
        // Determine Type of Enemy Randomly using Probabilities based on Spawn Weights
        float _enemyType = Random.Range(0f, 1f);
        int _enemyIndex = -1;
        for (int j = 0; j < numberOfPowerUps; j++)
        {
            if (_enemyType <= spawnProbabilities[j])
            {
                _enemyIndex = j;
                break;
            }
        }
        return _enemyIndex;
    }

}
