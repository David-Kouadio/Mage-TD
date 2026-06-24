using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    string newGameScene = "Tutorial";

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;

    [Header("Options Controls")]
    public Slider volumeSlider;

    void Start()
    {
        // Set up the panels on start
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);

        // Set slider value to current volume
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        // Dynamically find and bind buttons in case they aren't bound in Inspector
        Button startBtn = GameObject.Find("Start")?.GetComponent<Button>() ?? GameObject.Find("Iniciar")?.GetComponent<Button>();
        Button optionsBtn = GameObject.Find("OPÇÕES")?.GetComponent<Button>() ?? GameObject.Find("Opcoes")?.GetComponent<Button>();
        Button exitBtn = GameObject.Find("SAIR")?.GetComponent<Button>();

        if (startBtn != null)
        {
            startBtn.onClick.RemoveAllListeners();
            startBtn.onClick.AddListener(StartNewGame);
        }
        if (optionsBtn != null)
        {
            optionsBtn.onClick.RemoveAllListeners();
            optionsBtn.onClick.AddListener(OpenOptions);
        }
        if (exitBtn != null)
        {
            exitBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.AddListener(ExitApplication);
        }

        Button backBtn = GameObject.Find("BtnVoltar")?.GetComponent<Button>();
        if (backBtn == null && optionsPanel != null)
        {
            backBtn = optionsPanel.transform.Find("BtnVoltar")?.GetComponent<Button>();
        }
        if (backBtn != null)
        {
            backBtn.onClick.RemoveAllListeners();
            backBtn.onClick.AddListener(CloseOptions);
        }
    }

    public void StartNewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(newGameScene);
    }

    public void OpenOptions()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
