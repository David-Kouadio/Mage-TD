using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;

    [Header("UI Panels")]
    public GameObject pauseMenuUI;
    public GameObject mainPanel;
    public GameObject optionsPanel;

    [Header("Options Controls")]
    public Slider volumeSlider;
    public Slider sensitivitySlider;

    private PlayerLook playerLook;
    private PlayerMotor playerMotor;

    void Start()
    {
        // Find player scripts to disable/enable
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        if (player == null)
        {
            player = GameObject.Find("Player 1");
        }

        if (player != null)
        {
            playerLook = player.GetComponent<PlayerLook>();
            playerMotor = player.GetComponent<PlayerMotor>();
        }

        // Initialize UI panels state
        if (mainPanel != null) mainPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);

        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            
            // Dynamically bind Main Panel buttons
            Button btnContinuar = mainPanel != null 
                ? (mainPanel.transform.Find("BtnContinuar/Button")?.GetComponent<Button>() ?? mainPanel.transform.Find("BtnContinuar")?.GetComponent<Button>())
                : (pauseMenuUI.transform.Find("BtnContinuar")?.GetComponent<Button>());
            
            Button btnOpcoes = mainPanel != null
                ? (mainPanel.transform.Find("BtnOpcoes/Button")?.GetComponent<Button>() ?? mainPanel.transform.Find("BtnOpcoes")?.GetComponent<Button>())
                : (pauseMenuUI.transform.Find("BtnOpcoes")?.GetComponent<Button>());

            Button btnMenu = mainPanel != null
                ? (mainPanel.transform.Find("BtnMenu/Button")?.GetComponent<Button>() ?? mainPanel.transform.Find("BtnMenu")?.GetComponent<Button>())
                : (pauseMenuUI.transform.Find("BtnMenu")?.GetComponent<Button>());

            if (btnContinuar != null)
            {
                btnContinuar.onClick.RemoveAllListeners();
                btnContinuar.onClick.AddListener(Resume);
            }
            if (btnOpcoes != null)
            {
                btnOpcoes.onClick.RemoveAllListeners();
                btnOpcoes.onClick.AddListener(OpenOptions);
            }
            if (btnMenu != null)
            {
                btnMenu.onClick.RemoveAllListeners();
                btnMenu.onClick.AddListener(BackToMainMenu);
            }

            // Set sliders initial values
            if (volumeSlider != null)
            {
                volumeSlider.value = AudioListener.volume;
                volumeSlider.onValueChanged.RemoveAllListeners();
                volumeSlider.onValueChanged.AddListener(SetVolume);
            }
            if (sensitivitySlider != null)
            {
                float savedSens = PlayerPrefs.GetFloat("CameraSensitivity", 30f);
                sensitivitySlider.value = savedSens;
                sensitivitySlider.onValueChanged.RemoveAllListeners();
                sensitivitySlider.onValueChanged.AddListener(SetSensitivity);
            }

            // Bind Options Back button
            if (optionsPanel != null)
            {
                Button btnVoltar = optionsPanel.transform.Find("BtnVoltar")?.GetComponent<Button>() 
                                ?? optionsPanel.transform.Find("BtnVoltar/Button")?.GetComponent<Button>()
                                ?? optionsPanel.GetComponentInChildren<Button>();
                if (btnVoltar != null)
                {
                    btnVoltar.onClick.RemoveAllListeners();
                    btnVoltar.onClick.AddListener(CloseOptions);
                }
            }
        }
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // Check if player is dead before pausing! If dead, don't allow pause
            var playerHealth = FindFirstObjectByType<PlayerHealth>();
            if (playerHealth != null && playerHealth.isDead)
            {
                return;
            }

            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        if (mainPanel != null) mainPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        
        Time.timeScale = 1f;
        isPaused = false;

        if (playerLook != null) playerLook.enabled = true;
        if (playerMotor != null) playerMotor.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        if (pauseMenuUI != null) pauseMenuUI.SetActive(true);
        if (mainPanel != null) mainPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
        
        Time.timeScale = 0f;
        isPaused = true;

        if (playerLook != null) playerLook.enabled = false;
        if (playerMotor != null) playerMotor.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OpenOptions()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (optionsPanel != null) optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        if (mainPanel != null) mainPanel.SetActive(true);
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("CameraSensitivity", sensitivity);
        PlayerPrefs.Save();
        
        if (playerLook != null)
        {
            playerLook.xSensitivity = sensitivity;
            playerLook.ySensitivity = sensitivity;
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }
}