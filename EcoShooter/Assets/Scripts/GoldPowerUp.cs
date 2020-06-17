using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPowerUp : PowerUp
{
    [SerializeField]
    int goldamount = 5;

    protected override void PowerUpEffect()
    {
        base.PowerUpEffect();
        player.GetComponent<PlayerController>().AddGold(goldamount);
    }

}
