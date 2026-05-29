using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    string newGameScene = "Tutorial";
    string MainMenu = "MainMenu";

    public void StartNewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(MainMenu);
    }
}
