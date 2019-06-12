using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchController : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToLeaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("LevelTest");
    }

    public void GoToGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
