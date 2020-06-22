using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    // Input Manager Variables
    private Vector2 inputDirection;
    private Vector2 inputFaceDirection;

    // Central GameObjects and Variables
    public GameObject projectile;
    public GameObject sackOfGold;
    private Tilemap tilemap;
    private Bounds deliveryArea;
    public LayerMask seedLayer;
    public LayerMask interactLayer;

    public Plant[] seedTypes;
    //private int[] seedAmount;  Seed Mechanism Deactivated
    int currentSeed = 0;

    // Movement Variables
    private float moveSpeed = 2;
    private float acceleration = 1;
    private Vector2 moveDirection;
    private Vector2 faceDirection;
    private bool isMoving = false;


    // Attack Manager Variables
    private bool isAttacking = false;
    private bool canAttack = false;
    private bool triggerAttackAnimation = false;
    private float attackTimer = 0;
    private float attackCooldown = 0.2f;
    private float attackDuration = 0.15f;

    // Interaction Variables
    float interactConstant = 0.25f;
    float interactRadius = 0.25f;
    float seedRadius = 0.5f;
    float bendDuration = 0.4f;
    int waterCost = 0;
    bool triggerHarvestAnimation = false;
    bool triggerSeedAnimation = false;
    bool triggerDeliverAnimation = false;
    private bool isOccupied = false;

    // Basic Attributes
    private int carrotCount;
    private int goldCount;
    private int waterCount;
    private int health;
     

    //Central Components
    private Animator anim;
    private Rigidbody2D rb;
    private ParticleSystem ps_dust;




    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        waterCount = 100;
        carrotCount = 10;
        goldCount = 30;
        //seedAmount = new int[2] { 10, 2};  Seed Mechanism Deactivated
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        ps_dust = GetComponentInChildren<ParticleSystem>();
        tilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        deliveryArea = FindObjectOfType<DeliveryArea>().gameObject.GetComponent<BoxCollider2D>().bounds;
    }

    // Update is called once per frame
    void Update()
    {

        // Do nothing if game is paused
        if (GameController.gameIsPaused)
        {
            return;
        }


        InputHandler();
        AttackManager();
        MovementManager();
        AnimationMananger();
        AccelerationManager();
    }

    /*
     * Gathers Inputs 
     */

    void InputHandler()
    {
        inputDirection = InputController.moveDirectionInput;
        inputFaceDirection = InputController.faceDirectionInput;

        if (InputController.shootInput)
        {
            Shoot();
            InputController.shootInput = false;
        }
        if (InputController.interactInput)
        {
            Interact(faceDirection.normalized);
            InputController.interactInput = false;
        }

        if (InputController.switchInput)
        {
            ChangeSeed();
            InputController.switchInput = false;
        }
    }


    /*
     * Handles Character Movement
     */
    #region Movement System

    void MovementManager()
    {
        // Reset velocity at any time
        rb.velocity = Vector2.zero;

        // Check if movement is possible, else do not update
        if (CanMove())
        {
            moveDirection = inputDirection.normalized;
            //faceDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            faceDirection = inputFaceDirection.normalized;
        }
        else
        {
            moveDirection = Vector2.zero;
        }


        // Set up Movement relative to Face-Direction
        Vector2 relativeMoveDirection = Vector2.zero;
        float _angle = (Vector2.SignedAngle(Vector2.up, moveDirection));

        // Calculate Relative Movement Angle
        if (moveDirection != Vector2.zero)
        {
            relativeMoveDirection = RotateVector(faceDirection, _angle).normalized;
        }


        // Perform Movement relative to facedirection;
        //transform.position = (Vector2)transform.position + moveSpeed * acceleration * relativeMoveDirection * Time.deltaTime;

        // Perform Movement towards facedirection / movedirection
        transform.position = (Vector2)transform.position + moveSpeed * acceleration * moveDirection * Time.deltaTime;

        transform.up = (Vector3)faceDirection;

        // Determine if Character is Moving by checking its movedirection
        isMoving = (relativeMoveDirection != Vector2.zero);
    }

    void AccelerationManager()
    {
        if (isMoving)
        {
            float accelerationRate;
            if (acceleration < 1.2f)
            {
                accelerationRate = 0.15f;
            }
            else
            {
                accelerationRate = 0.3f;
            }
            acceleration = Math.Min(1.5f, acceleration * (1 + accelerationRate * Time.deltaTime));
        }
        else
        {
            acceleration = 1;
        }
    }


    private bool CanMove()
    {
        bool canMove = !isAttacking && !isOccupied;
        return canMove;
    }

    #endregion




    #region Attack System

    /*
     * Handles the Attack-Cooldown and determines if attacks are possible
     * 
     */

    protected virtual void AttackManager()
    {
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            canAttack = (attackTimer > attackCooldown) && !isAttacking && !isOccupied;
        }
    }

    void Shoot()
    {
        // Check if there are Carrots to throw
        if (canAttack && carrotCount > 0)
        {
            // Instantiate Projectile and set up its direction
            GameObject instance = Instantiate(projectile, transform.position, Quaternion.identity);
            instance.GetComponent<ProjectileController>().SetMoveDirection(faceDirection.normalized);

            // Reduce Carrot Count
            carrotCount -= 1;

            // Trigger Animation
            triggerAttackAnimation = true;

            // Set Attackmanager-Variables to impose Cooldown
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

    #endregion



    #region Interaction System

    /*
     * Handles Character Interactions. Currently Distinguishes between Seed and Harvest carrot
     */
    void Interact(Vector3 interactDirection)
    {
        // Determine Position of Interaction
        // Determines where Character looks for interaction
        Vector2 interactionPosition = transform.position + interactConstant * interactDirection;

        // Check if interaction targets the Gold Delivery Area
        if (deliveryArea.Contains(interactionPosition))
        {
            DeliverGold(interactionPosition);
        }

        // Find Plants and Harvest the first Plant to be found
        // Else Seed new Plant if possible
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(interactionPosition, interactRadius, interactLayer);
        if (hitColliders.Length > 0)
        {
            // Determine the kind of Interaction
            if (hitColliders[0].gameObject.GetComponent<CarrotPlant>() != null)
            {
                // Get the first harvestable plant to be found
                GameObject target = hitColliders[0].gameObject;
                HarvestPlant(target);
            }
            // Determine the kind of Interaction
            if (hitColliders[0].gameObject.GetComponent<GoldPlant>() != null)
            {
                // Get the first harvestable plant to be found
                GameObject target = hitColliders[0].gameObject;
                HarvestPlant(target);
            }
            if (hitColliders[0].gameObject.GetComponent<Water>() != null)
            {
                waterCount = 100;
            }
        }
        else
        {
            Seed(interactionPosition);
        }

    }


    void HarvestPlant(GameObject plant)
    {
        // Determine type of Plant to harvest
        if (plant.GetComponent<CarrotPlant>() != null)
        {
            carrotCount += plant.GetComponent<CarrotPlant>().HarvestCarrot();
        }
        if (plant.GetComponent<GoldPlant>() != null)
        {
            Debug.Log("Tried to harvest Gold Plant");
            goldCount += plant.GetComponent<GoldPlant>().HarvestGold();
        }

        triggerHarvestAnimation = true;
        isOccupied = true;
        Invoke("ResetIsOccupied", bendDuration);
    }

    void Seed(Vector3 seedPosition)
    {

        // Get Tile and Coordinates
        Vector3 worldCoords = tilemap.WorldToCell(seedPosition);
        Vector3Int tileCoords = tilemap.WorldToCell(worldCoords);

        // Check if Interaction Position represents an Acre
        if (tilemap.GetTile(tileCoords).name == "Acre")
        {
            // Check if there are plants at the position (Radius should be half a unit)
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(seedPosition, seedRadius, seedLayer);
            if (hitColliders.Length == 0 && waterCount >= waterCost)
            {
                Instantiate(seedTypes[currentSeed], seedPosition, Quaternion.identity);
                waterCount -= waterCost;

                triggerSeedAnimation = true;
                isOccupied = true;
                Invoke("ResetIsOccupied", bendDuration);
            }
        }
    }

    void DeliverGold(Vector3 deliveryPosition)
    {

        if (goldCount >= GameController.rentDeduction && !GameController.rentDelivered)
        {
            AddGold(-GameController.rentDeduction);
            GameController.rentDelivered = true;

            GameObject instance = Instantiate(sackOfGold, deliveryPosition, Quaternion.identity);
            Destroy(instance, 3);

            triggerDeliverAnimation = true;
            isOccupied = true;
            Invoke("ResetIsOccupied", bendDuration);
            Debug.Log("Delivered!");
        }
    }

    /*
     * Manage currently active seed
     */

    private void ChangeSeed()
    {
        currentSeed++;
        if (currentSeed >= seedTypes.Length)
        {
            currentSeed = 0;
        }
    }

    private void ResetIsOccupied()
    {
        isOccupied = false;
    }

    #endregion


    #region Animation System

    private void AnimationMananger()
    {

        // Trigger Attack
        if (triggerAttackAnimation)
        {
            anim.SetTrigger("attack");
            triggerAttackAnimation = false;
        }

        // Trigger Seed and Harvest
        if(triggerSeedAnimation || triggerHarvestAnimation || triggerDeliverAnimation)
        {
            anim.SetTrigger("bend");
            triggerSeedAnimation = false;
            triggerHarvestAnimation = false;
            triggerDeliverAnimation = false;
        }

        // Movement
        anim.SetBool("isWalking", isMoving);
        anim.speed = acceleration;

        var em = ps_dust.emission;
        if (acceleration <= 1.04f)
        {
            em.rateOverTime = 0;
        }
        else if (acceleration <= 1.3f)
        {
            em.rateOverTime = 4f;
        }
        else
        {
            em.rateOverTime = 25;
        }
        
    }


    #endregion


    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetCarrotCount()
    {
        return carrotCount;
    }

    public int GetGoldCount()
    {
        return goldCount;
    }

    public int GetWaterCount()
    {
        return waterCount;
    }



    #region Utility
    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + interactConstant * (Vector3)faceDirection.normalized, interactRadius);
    } 
    #endregion


    #region Shop Interactions


    public int GetCurrentSeed()
    {
        return currentSeed;
    }



    public void AddCarrots(int amount)
    {
        carrotCount += amount;
    }



    public void AddHealth(int amount)
    {
        health += amount;
    }

    public void AddGold(int amount)
    {
        goldCount += amount;
        Debug.Log(amount);
    }

    #endregion


}
