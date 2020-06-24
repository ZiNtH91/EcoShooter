using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingPowerUp : PowerUp
{
    protected override void PowerUpEffect()
    {
        base.PowerUpEffect();
        PlayerController.hasPiercingPowerUp = true;
        Debug.Log("PiercingMode!");
    }
}
