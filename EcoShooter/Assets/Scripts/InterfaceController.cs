using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InterfaceController : MonoBehaviour
{
    public Image healthbar;
    public Image rentDeductionBar;
    public TextMeshProUGUI gameTime;
    public TextMeshProUGUI highScore;

    public Color greenColor;
    public Color goldColor;

    public Image[] seedMarkers;
    public TextMeshProUGUI carrotCount;
    public TextMeshProUGUI goldCount;

    private GameController gameController;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {

        gameController = FindObjectOfType<GameController>();
        player = GameObject.Find("Character").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        gameTime.text = GameController.gameTime.ToString();
        highScore.text = HighscoreController.highScore.ToString();
        carrotCount.text = player.GetCarrotCount().ToString();
        goldCount.text = player.GetGoldCount().ToString();
        healthbar.fillAmount = (float)player.GetHealth() / 100;
        rentDeductionBar.fillAmount = 1 - gameController.GetRelativeRentDeductionTimer();
        if (GameController.rentDelivered)
        {
            rentDeductionBar.color = greenColor;
        }
        else
        {
            rentDeductionBar.color = goldColor;
        }
        HandleSeedMarkers(player.GetCurrentSeed());


        //CrosshairController();

    }

    void HandleSeedMarkers(int activeSeed)
    {
        for(int i=0; i < seedMarkers.Length; i++)
        {
            seedMarkers[i].gameObject.SetActive(i == activeSeed);
        }
    }


    private void CrosshairController()
    {
        if (GameController.gameIsPaused || SceneManager.GetActiveScene().name == "MainMenue")
        {
            Cursor.visible = true;
        }
        else
        {
            Cursor.visible = false;
        }

    }

}
