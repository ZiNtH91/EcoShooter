using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Raven : Enemy
{

    public LayerMask treeLayers;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        moveSpeed = 4;
        targetRefreshCooldown = 4;
    }

    protected override void Update()
    {
        base.Update();
    }


    protected override void CheckForTarget()
    {
        Vector2 playerDir = (player.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + 2 * (Vector3)playerDir, playerDir, 10, treeLayers);

        if (hit.collider != null)
        {
            // Attack Player
            target = hit.collider.gameObject;
        }
        else
        {
            // No direct hit possible - fly to the tree that is closest to player
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(player.transform.position, 10, treeLayers);

            if (hitColliders.Length > 0)
            {
                List<Collider2D> trees = hitColliders.ToList<Collider2D>();

                float nearestDist = float.MaxValue;
                GameObject nearestObject = null;

                foreach (Collider2D tree in trees)
                {
                    if (Vector2.Distance(player.transform.position, tree.gameObject.transform.position) < nearestDist)
                    {
                        nearestDist = Vector2.Distance(player.transform.position, tree.gameObject.transform.position);
                        nearestObject = tree.gameObject;
                    }
                }

                target = nearestObject;
            }
            else
            {
                target = player;
            }
        }
        

    }

    protected override void MovementManager()
    {
        // Reset velocity at any time
        rb.velocity = Vector2.zero;

        // Reset moveDirection and Velocity
        moveDirection = Vector2.zero;
        rb.velocity = moveDirection;

        if (target != null && Vector2.Distance(target.transform.position, transform.position) > 0.05f)
        {

            moveDirection = (target.transform.position - transform.position).normalized;

            //rb.velocity = moveSpeed * moveDirection;
            transform.position = (Vector2)transform.position + moveSpeed * moveDirection * Time.deltaTime;
            transform.up = moveDirection;

            targetRefreshTimer = 0;

            anim.SetBool("isWalking", true);
        }
        else
        {
            transform.up = (Vector2)(player.transform.position - transform.position).normalized;
            anim.SetBool("isWalking", false);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            AttackPlayer();
        }
    }
}
