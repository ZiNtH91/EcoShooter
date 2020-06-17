using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Mouse : Enemy
{
    float targetRange = 5;
    public LayerMask targetLayer;

    // Interaction Manager Variables
    protected bool isEating = false;
    protected float eatDuration = 1f;

    protected override void Start()
    {
        base.Start();
        moveSpeed = 2;
        targetRefreshCooldown = 2.5f;
    }

    protected override void Update()
    {
        base.Update();
        InteractionMananger();
    }



    protected override void InteractionMananger()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, faceDirection.normalized, 0.5f);
        
        // Stop if there was no hit
        if (hit.collider == null)
        {
            return;
        }
        Debug.Log("Hit!");
        // If there was a hit, check the type of GameObject
        if (hit.collider.gameObject==target)
        {
            if (target == player)
            {
                AttackPlayer();
            }
            if (!isEating) 
            {
                if (target.GetComponent<CarrotPlant>() != null)
                {
                    target.GetComponent<CarrotPlant>().EatCarrot();
                    
                    isEating = true;
                    isOccupied = true;
                    Invoke("ResetIsEating", eatDuration);
                }

                if (target.GetComponent<GoldPlant>() != null)
                {
                    target.GetComponent<GoldPlant>().EatGold();

                    isEating = true;
                    isOccupied = true;
                    Invoke("ResetIsEating", eatDuration);
                }

            }
        }
    }


    protected override void CheckForTarget()
    {

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, targetRange, targetLayer);

        if (hitColliders.Length > 0)
        {
            List<Collider2D> plants = hitColliders.ToList<Collider2D>();

            float nearestDist = float.MaxValue;
            GameObject nearestObject = null;

            foreach (Collider2D plant in plants)
            {
                if(Vector2.Distance(transform.position, plant.gameObject.transform.position) < nearestDist){
                    nearestDist = Vector2.Distance(transform.position, plant.gameObject.transform.position);
                    nearestObject = plant.gameObject;
                }
            }

            target = nearestObject;
        }
        else
        {
            target = player;
        }
    }

    protected void ResetIsEating()
    {
        isEating = false;
        isOccupied = false;
    }

}
