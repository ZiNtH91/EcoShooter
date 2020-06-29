using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject interfaceUI;
    public GameObject gameOverUI;


    private void Update()
    {

        // Stop here during Main Menue
        if (SceneManager.GetActiveScene().name == "MainMenue")
        {
            return;
        }

        if (GameController.gameOver)
        {
            GameOver();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !GameController.gameOver)
        {
            if (GameController.gameIsPaused)
            {
                ResumeButton();
            }
            else
            {
                PauseButton();
            }
        }
    }

    public void StartButton()
    {
        SceneTransitionController.LoadScene("Game");
    }

    public void QuitButton()
    {
        Application.Quit();
    }


    public void ResumeButton()
    {
        Time.timeScale = 1f;
        GameController.gameIsPaused = false;
        GameController.gameOver = false;
        pauseMenuUI.SetActive(false);
        interfaceUI.SetActive(true);
    }

    public void PauseButton()
    {
        Time.timeScale = 0f;
        GameController.gameIsPaused = true;
        pauseMenuUI.SetActive(true);
        interfaceUI.SetActive(false);
        Cursor.visible = true;
    }

    public void RestartButton()
    {
        SceneTransitionController.LoadScene(SceneManager.GetActiveScene().name);
        Invoke("ResumeButton", 0);
    }

    public void MenueButton()
    {
        SceneTransitionController.LoadScene("MainMenue");
        Cursor.visible = true;
        Invoke("ResumeButton", 0);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        GameController.gameIsPaused = true;
        gameOverUI.SetActive(true);
    }



}
