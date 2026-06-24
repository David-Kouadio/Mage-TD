using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayIndicators : MonoBehaviour
{
    private Canvas uiCanvas;
    private GameObject objectivePanel;
    private TextMeshProUGUI objectiveText;

    private GameObject controlsPanel;

    // Temporary welcome message variables
    private Canvas tempCanvas;
    private GameObject tempMessagePanel;
    private TextMeshProUGUI tempMessageText;

    private void Awake()
    {
        CreateUIElements();
    }

    private void Start()
    {
        string activeScene = SceneManager.GetActiveScene().name;
        if (activeScene == "Level1")
        {
            CreateTemporaryMessageUI("ELIMINE OS PUKEKOS!", Color.red, 52f, new Color(0f, 0f, 0f, 0.85f));
            StartCoroutine(ShowTemporaryMessageRoutine(6f));
        }
        else if (activeScene == "Tutorial")
        {
            // Friendly welcoming message with inviting gold and green theme
            string welcomeMsg = "Bem-vindo, Mago!\nSiga o caminho brilhante e pegue o seu cajado.";
            Color invitingGold = new Color(1f, 0.84f, 0f); // Warm Gold color
            Color invitingForestBg = new Color(0.05f, 0.15f, 0.1f, 0.9f); // Dark cozy forest green banner
            CreateTemporaryMessageUI(welcomeMsg, invitingGold, 38f, invitingForestBg);
            StartCoroutine(ShowTemporaryMessageRoutine(4f)); // 4 seconds duration
        }
    }

    private void CreateTemporaryMessageUI(string message, Color textColor, float fontSize, Color bgColor)
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("TemporaryMessageCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        tempCanvas = canvasObj.GetComponent<Canvas>();
        tempCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        tempCanvas.sortingOrder = 999; // Very high priority

        CanvasScaler scaler = canvasObj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        // Create Message Panel in the center
        tempMessagePanel = new GameObject("TempMessagePanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        tempMessagePanel.transform.SetParent(canvasObj.transform, false);

        RectTransform panelRect = tempMessagePanel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0f, 0.38f);
        panelRect.anchorMax = new Vector2(1f, 0.62f); // Banner across center
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = Vector2.zero;

        Image panelBg = tempMessagePanel.GetComponent<Image>();
        panelBg.color = bgColor;

        // Create Text
        GameObject textObj = new GameObject("TempMessageText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(tempMessagePanel.transform, false);

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = new Vector2(-40, -40); // small padding
        textRect.anchoredPosition = Vector2.zero;

        tempMessageText = textObj.GetComponent<TextMeshProUGUI>();
        tempMessageText.fontSize = fontSize;
        tempMessageText.color = textColor;
        tempMessageText.alignment = TextAlignmentOptions.Center;
        tempMessageText.fontStyle = FontStyles.Bold;
        tempMessageText.text = message;
    }

    private System.Collections.IEnumerator ShowTemporaryMessageRoutine(float duration)
    {
        // Wait specified duration
        yield return new WaitForSeconds(duration);

        // Fade out
        if (tempMessagePanel != null)
        {
            Image panelBg = tempMessagePanel.GetComponent<Image>();
            float elapsed = 0f;
            float fadeDuration = 1.5f;

            Color startBgColor = panelBg.color;
            Color endBgColor = new Color(startBgColor.r, startBgColor.g, startBgColor.b, 0f);

            Color startTextColor = tempMessageText.color;
            Color endTextColor = new Color(startTextColor.r, startTextColor.g, startTextColor.b, 0f);

            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                if (panelBg != null) panelBg.color = Color.Lerp(startBgColor, endBgColor, t);
                if (tempMessageText != null) tempMessageText.color = Color.Lerp(startTextColor, endTextColor, t);
                yield return null;
            }
        }

        // Destroy Canvas
        if (tempCanvas != null)
        {
            Destroy(tempCanvas.gameObject);
            tempCanvas = null;
        }
    }

    private void CreateUIElements()
    {
        // 1. Create Canvas
        GameObject canvasObj = new GameObject("GameplayIndicatorsCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        uiCanvas = canvasObj.GetComponent<Canvas>();
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        uiCanvas.sortingOrder = 99; // Render on top of most other UI elements

        CanvasScaler scaler = canvasObj.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        // Keep canvas alive across scenes if desired, but we will let it destroy and recreate per scene to adapt easily
        
        // 2. Create Objective Panel (Top Center)
        objectivePanel = new GameObject("ObjectivePanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        objectivePanel.transform.SetParent(canvasObj.transform, false);

        RectTransform objRect = objectivePanel.GetComponent<RectTransform>();
        objRect.anchorMin = new Vector2(0.5f, 1f);
        objRect.anchorMax = new Vector2(0.5f, 1f);
        objRect.pivot = new Vector2(0.5f, 1f);
        objRect.sizeDelta = new Vector2(800, 70);
        objRect.anchoredPosition = new Vector2(0, -30);

        Image objBg = objectivePanel.GetComponent<Image>();
        objBg.color = new Color(0.1f, 0.1f, 0.1f, 0.8f); // Semi-transparent dark grey

        // Add Objective Text
        GameObject textObj = new GameObject("ObjectiveText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textObj.transform.SetParent(objectivePanel.transform, false);

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;

        objectiveText = textObj.GetComponent<TextMeshProUGUI>();
        objectiveText.fontSize = 24f;
        objectiveText.color = Color.yellow;
        objectiveText.alignment = TextAlignmentOptions.Center;
        objectiveText.fontStyle = FontStyles.Bold;
        objectiveText.text = "OBJETIVO: Carregando...";

        // 3. Create Controls Presentation Panel (Top Right or Bottom Left - let's put it on the Left Middle/Top Left)
        controlsPanel = new GameObject("ControlsPanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        controlsPanel.transform.SetParent(canvasObj.transform, false);

        RectTransform ctrlRect = controlsPanel.GetComponent<RectTransform>();
        ctrlRect.anchorMin = new Vector2(0f, 0.5f);
        ctrlRect.anchorMax = new Vector2(0f, 0.5f);
        ctrlRect.pivot = new Vector2(0f, 0.5f);
        ctrlRect.sizeDelta = new Vector2(340, 320);
        ctrlRect.anchoredPosition = new Vector2(20, 100);

        Image ctrlBg = controlsPanel.GetComponent<Image>();
        ctrlBg.color = new Color(0.1f, 0.1f, 0.1f, 0.85f);

        // Add Controls Title
        GameObject ctrlTitleObj = new GameObject("ControlsTitle", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        ctrlTitleObj.transform.SetParent(controlsPanel.transform, false);

        RectTransform ctrlTitleRect = ctrlTitleObj.GetComponent<RectTransform>();
        ctrlTitleRect.anchorMin = new Vector2(0f, 1f);
        ctrlTitleRect.anchorMax = new Vector2(1f, 1f);
        ctrlTitleRect.pivot = new Vector2(0.5f, 1f);
        ctrlTitleRect.sizeDelta = new Vector2(-20, 40);
        ctrlTitleRect.anchoredPosition = new Vector2(0, -10);

        TextMeshProUGUI ctrlTitleText = ctrlTitleObj.GetComponent<TextMeshProUGUI>();
        ctrlTitleText.fontSize = 20f;
        ctrlTitleText.color = Color.cyan;
        ctrlTitleText.alignment = TextAlignmentOptions.Center;
        ctrlTitleText.fontStyle = FontStyles.Bold;
        ctrlTitleText.text = "CONTROLES / COMANDOS";

        // Add Controls Content Text
        GameObject ctrlContentObj = new GameObject("ControlsContent", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        ctrlContentObj.transform.SetParent(controlsPanel.transform, false);

        RectTransform ctrlContentRect = ctrlContentObj.GetComponent<RectTransform>();
        ctrlContentRect.anchorMin = Vector2.zero;
        ctrlContentRect.anchorMax = new Vector2(1f, 1f);
        ctrlContentRect.sizeDelta = new Vector2(-30, -60);
        ctrlContentRect.anchoredPosition = new Vector2(0, -25);

        TextMeshProUGUI ctrlContentText = ctrlContentObj.GetComponent<TextMeshProUGUI>();
        ctrlContentText.fontSize = 16f;
        ctrlContentText.color = Color.white;
        ctrlContentText.alignment = TextAlignmentOptions.Left;
        ctrlContentText.text = 
            "<b>[W, A, S, D]</b> - Mover\n" +
            "<b>[Espaço]</b> - Pular\n" +
            "<b>[Mouse Esq.]</b> - Atirar\n" +
            "<b>[R]</b> - Recarregar Arma\n" +
            "<b>[F]</b> - Soco (Reflete Projéteis!)\n" +
            "<b>[E]</b> - Interagir / Pegar\n" +
            "<b>[ESC]</b> - Pausar Jogo";
    }

    private void Update()
    {
        string activeScene = SceneManager.GetActiveScene().name;

        if (activeScene == "Tutorial")
        {
            // Check if player picked up the staff
            bool hasWeapon = false;
            if (WeaponManager.Instance != null && WeaponManager.Instance.activeWeaponSlot != null)
            {
                if (WeaponManager.Instance.activeWeaponSlot.transform.childCount > 0)
                {
                    hasWeapon = true;
                }
            }

            // Immediately destroy the welcome message canvas when weapon is acquired
            if (hasWeapon && tempCanvas != null)
            {
                StopAllCoroutines();
                Destroy(tempCanvas.gameObject);
                tempCanvas = null;
            }

            if (!hasWeapon)
            {
                objectiveText.text = "<b>OBJETIVO:</b> Vá até o cajado e pegue-o (Pressione <b>[E]</b> para pegar).";
            }
            else
            {
                objectiveText.text = "<b>OBJETIVO:</b> Vá até o portal/teletransporte para ir ao Level 1.";
            }
        }
        else if (activeScene == "Level1")
        {
            objectiveText.text = "<b>OBJETIVO:</b> Encontre e derrote os pukekos no Level 1!";
        }
        else
        {
            // Any other scene
            objectiveText.text = "<b>OBJETIVO:</b> Explore o cenário!";
        }
    }
}
