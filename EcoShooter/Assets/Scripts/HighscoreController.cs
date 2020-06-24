using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreController
{

    public static int carrotHarvestScore=10;
    public static int goldHarvestScore=15;
    public static int enemyShotScore=5;
    public static int goldDeliveryScore=20;

    public static int gameScore = 0;
    public static int highScore = 0;

    public static int gameTimeMultiplicator = 2;


    public static void AddToHighScore(int scoreToBeAdded)
    {
        gameScore += scoreToBeAdded;
        highScore = gameScore + gameTimeMultiplicator * GameController.gameTime;
    }

}
