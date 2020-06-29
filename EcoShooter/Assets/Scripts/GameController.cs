using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{


    // Rent Deduction System
    public static int rentDeduction = 30;
    private float rentDeductionTimer = 0;
    private float rentDeductionPeriod = 20;
    public static bool rentDelivered = false;

    // Food Deduction Mechanism
    public int foodDeduction = 10;
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
    }


    private void RentDeductionManager()
    {
        rentDeductionTimer += Time.deltaTime;
        if(rentDeductionTimer > rentDeductionPeriod)
        {
            if (!rentDelivered)
            {
                gameOver = true;
            }
            rentDeductionTimer = 0;
            rentDelivered = false;
        }
    }

    public float GetRelativeRentDeductionTimer()
    {
        return (rentDeductionTimer / rentDeductionPeriod);
    }

}
