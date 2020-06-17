using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : PowerUp
{
    [SerializeField]
    int healthAmount = 5;

    protected override void PowerUpEffect()
    {
        base.PowerUpEffect();
        player.GetComponent<PlayerController>().AddHealth(healthAmount);
    }

}
