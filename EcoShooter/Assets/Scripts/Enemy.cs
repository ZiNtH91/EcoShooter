using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    // Central Variables / GameObjects
    protected GameObject player;
    protected int damage=5;

    // Central Components
    protected EnemyAI ai;
    protected Animator anim;
    protected Rigidbody2D rb;
    public GameObject explosion;


    // Target Manager Variables
    protected GameObject target;
    protected float targetRefreshTimer = 0;
    protected float targetRefreshCooldown = 0;

    // Movement Manager Variables
    protected float moveSpeed = 1;
    protected Vector2 moveDirection;
    protected Vector2 faceDirection;
    protected bool isMoving = false;
    protected bool isOccupied = false;

    // Attack Manager Variables
    protected bool isAttacking = false;
    protected bool canAttack = false;
    protected float attackTimer = 0;
    protected float attackCooldown = 1;
    protected float attackDuration = 0.5f;

    protected float powerUpProbability = 0.3f;

    protected virtual void Start()
    {
        player = GameObject.Find("Character");
        // destinationSetter = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();
        ai = GetComponent<EnemyAI>();
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    protected virtual void Update()
    {

        // Do nothing if game is paused
        if (GameController.gameIsPaused)
        {
            return;
        }

        TargetManager();
        MovementManager();
        AttackManager();
        InteractionMananger();
    }


    /*
     * Manages Target Cooldowns and triggers Target Selection.
     * Currently not to be overwriten
     */

    protected void TargetManager()
    {
        targetRefreshTimer += Time.deltaTime;

        if (target == null || targetRefreshTimer > targetRefreshCooldown)
        {
            CheckForTarget();
            targetRefreshTimer = 0;
        }
    }


    /*
     * Handles Interactions with target object. 
     * 
     */

    protected virtual void InteractionMananger()
    {

    }

    /*
     * Handles the Attack-Cooldown and determines if attacks are possible
     * 
     */

    protected virtual void AttackManager()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            canAttack = (attackTimer > attackCooldown) && !isAttacking;
        }
    }

    /*
     * Controls the Target Selection Process and can be overriden for 
     * different targeting patterns     * 
     */

    protected virtual void CheckForTarget()
    {
        target = player;
    }


    protected virtual void MovementManager()
    {
        // Reset velocity at any time
        rb.velocity = Vector2.zero;

        // Set Target and Calculate Movedirection
        ai.target = target.transform;
        moveDirection = ai.GetDirection().normalized;

        // Set up Facedirection, do not update if no new direction is delivered
        if (moveDirection != Vector2.zero)
        {
            faceDirection = moveDirection;
        }

        // Perform movement this way to avoid unwanted physical drag etc.
        transform.position = (Vector2)transform.position + moveSpeed * moveDirection * Time.deltaTime;

        transform.up = faceDirection;

        // Determine if Character is Moving by checking its movedirection
        isMoving = (moveDirection != Vector2.zero);
        anim.SetBool("isWalking", true);
    }

    protected void AttackPlayer()
    {
        if (canAttack)
        {
            player.GetComponent<PlayerController>().TakeDamage(damage);
            anim.SetTrigger("attack");
            canAttack = false;
            attackTimer = 0;

            isAttacking = true;
            Invoke("ResetIsAttacking", attackDuration);
        }
    }

    protected void ResetIsAttacking()
    {
        isAttacking = false;
    }


    public void Die()
    {
        if (Random.Range(0,1f) < powerUpProbability)
        {
            GameObject powerUpInstance = FindObjectOfType<PowerUpController>().GetPowerUp();
            Instantiate(powerUpInstance, transform.position, Quaternion.identity);
        }
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

}
