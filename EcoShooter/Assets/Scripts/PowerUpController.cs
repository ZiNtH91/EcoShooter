using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public GameObject[] powerUps;

    // Start is called before the first frame update


    public GameObject GetPowerUp()
    {
        int powerUpIndex = Random.Range(0, powerUps.Length);
        return powerUps[powerUpIndex];
    }

}
