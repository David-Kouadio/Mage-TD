using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    string newGameScene = "Tutorial";
    string mainMenuScene = "MainMenu";

    public void StartNewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGameScene);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }
}
