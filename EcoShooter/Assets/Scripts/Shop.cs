using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{
    PlayerController player;

    int currentGold;
    int currentCarrots;
    int currentHealth;

    public TextMeshProUGUI currentCarrotsText;
    public TextMeshProUGUI currentHealthText;

    int carrotBasket = 0;
    int healthBasket = 0;

    int carrotsTotalPrice = 0;
    int healthTotalPrice = 0;

    public TextMeshProUGUI carrotBasketText;
    public TextMeshProUGUI healthBasketText;

    public TextMeshProUGUI carrotsTotalPriceText;
    public TextMeshProUGUI healthTotalPriceText;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Character").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentResources();
        UodateBasketTexts();
        UpdateBill();

        if (Input.GetKey(KeyCode.Escape))
        {
            ResetShop();
        }
    }

    private void UpdateCurrentResources()
    {
        // Get Current Resources / Amounts
        currentCarrots = player.GetCarrotCount();
        currentGold = player.GetGoldCount();
        currentHealth = player.GetHealth();

        // Update Texts
        currentCarrotsText.text = "Carrots (" + currentCarrots.ToString() + ")";
        currentHealthText.text = "Health (" + currentHealth.ToString() + ")";
    }

    private void UodateBasketTexts()
    {
        carrotBasketText.text = carrotBasket.ToString();
        healthBasketText.text = healthBasket.ToString();
    }

    private void UpdateBill()
    {
        // Calculate Total Costs
        carrotsTotalPrice = 10 * carrotBasket;
        healthTotalPrice = 1 * healthBasket;

        // Update Texts
        carrotsTotalPriceText.text = carrotsTotalPrice.ToString();
        if(currentCarrots + carrotBasket >= 0)
        {
            carrotsTotalPriceText.color = Color.white;
        }
        else
        {
            carrotsTotalPriceText.color = Color.red;
        }

        healthTotalPriceText.text = healthTotalPrice.ToString();
        if (currentHealth + healthBasket > 0)
        {
            healthTotalPriceText.color = Color.white;
        }
        else
        {
            healthTotalPriceText.color = Color.red;
        }
    }


    public void AddItem(string itemName)
    {
        switch (itemName)
        {
            case "Carrot":
                carrotBasket++;
                break;
            case "Health":
                if(healthBasket + player.GetHealth() < 100)
                {
                    healthBasket += 10;
                }
                break;
            default:
                Debug.Log("Problem!");
                break;
        }
    }


    public void RemoveItem(string itemName)
    {
        switch (itemName)
        {
            case "Carrot":
                if(carrotBasket >= 1)
                {
                    carrotBasket--;
                }
                break;
            case "Health":
                if(healthBasket >= 10)
                {
                    healthBasket -= 10;
                }
                break;
            default:
                Debug.Log("Problem!");
                break;
        }
    }

    public void Confirm(string itemName)
    {
        switch (itemName)
        {
            case "Carrot":
                // Carrots may not be sold to the point of having less than 0 carrots
                if (carrotsTotalPrice <= currentGold && currentCarrots + carrotBasket >= 0)
                {
                    player.AddCarrots(carrotBasket);
                    player.AddGold(-carrotsTotalPrice);
                    carrotBasket = 0;
                }
                break;
            case "Health":
                // Health may not be sold to the point of having 0 or less health
                if (healthTotalPrice <= currentGold && currentHealth + healthBasket > 0)
                {
                    player.AddHealth(healthBasket);
                    player.AddGold(-healthTotalPrice);
                    healthBasket = 0;
                }
                break;
            default:
                Debug.Log("Problem!");
                break;
        }
    }

    private void ResetShop()
    {
        carrotBasket = 0;
        healthBasket = 0;
    }


}
