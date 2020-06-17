using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{


    // Rent Deduction System
    public int rentDeduction = 10;
    private float rentDeductionTimer = 0;
    public float rentDeductionPeriod = 0;

    // Food Deduction Mechanism
    public int foodDeduction = 10;
    private float foodDeductionTimer = 0;
    public float foodDeductionPeriod = 15;

    // General Vars and Objects
    private PlayerController player;
    private float gameTimeRaw = 0;

    public static int gameTime = 0;
    public static bool gameIsPaused = false;
    public static bool gameOver = false;


    void Start()
    {
        player = GameObject.Find("Character").GetComponent<PlayerController>();
        InvokeRepeating("RentIsDue", rentDeduction, rentDeductionPeriod);
    }

    // Update is called once per frame
    void Update()
    {
        gameTimeRaw += Time.deltaTime;
        gameTime = Mathf.RoundToInt(gameTimeRaw);


        if (player.GetHealth() <= 0 || player.GetGoldCount() < 0 || player.GetCarrotCount() < 0)
        {
            gameOver = true;
        }

        RentDeductionManager();
        //FoodDeductionManager(); // Deactivated
    }


    private void RentDeductionManager()
    {
        rentDeductionTimer += Time.deltaTime;
        if(rentDeductionTimer > rentDeductionPeriod)
        {
            player.AddGold(-rentDeduction);
            rentDeductionTimer = 0;
        }
    }

    private void FoodDeductionManager()
    {
        foodDeductionTimer += Time.deltaTime;
        if (foodDeductionTimer > foodDeductionPeriod)
        {
            player.AddCarrots(-foodDeduction);
            foodDeductionTimer = 0;
        }
    }

    public float GetRelativeRentDeductionTimer()
    {
        return (rentDeductionTimer / rentDeductionPeriod);
    }
    public float GetRelativeFoodDeductionTimer()
    {
        return (foodDeductionTimer / foodDeductionPeriod);
    }
}
