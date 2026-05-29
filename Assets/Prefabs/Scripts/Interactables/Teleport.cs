using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{

    string lvl = "Level1";

    public void StartNewGame()
    {
        SceneManager.LoadScene(lvl);
    }
}
