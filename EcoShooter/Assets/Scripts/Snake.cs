using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : Enemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        moveSpeed = 1;
        targetRefreshCooldown = 0;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void InteractionMananger()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, faceDirection.normalized, 0.5f);

        // Stop if there was no hit
        if (hit.collider == null)
        {
            return;
        }

        // If there was a hit, check the type of GameObject
        if (hit.collider.gameObject == target)
        {
            if (target = player)
            {
                AttackPlayer();
                Debug.Log("SnakeAttack!");
            }
        }
    }

}
