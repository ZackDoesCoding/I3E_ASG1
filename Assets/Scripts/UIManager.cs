using TMPro;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text hpText;
    public Image healthFillImage;

    public GameObject MenuPanel;
    public GameObject AdminPanel;
    public GameObject Crosshair;
    public GameObject GameoverScreen;
    public StarterAssetsInputs starterInputs;

    private void Awake()
    {
        starterInputs = FindFirstObjectByType<StarterAssetsInputs>();
    }

    void Start()
    {
        ApplyInputAndCursorState();
    }

    private void ApplyInputAndCursorState()
    {
        bool isGameoverOpen = GameoverScreen != null && GameoverScreen.activeSelf;
        bool isMenuOpen = MenuPanel != null && MenuPanel.activeSelf;
        bool isAdminOpen = AdminPanel != null && AdminPanel.activeSelf;
        bool shouldShowCursor = isGameoverOpen || isMenuOpen || isAdminOpen;

        if (Crosshair != null)
        {
            Crosshair.SetActive(!shouldShowCursor);
        }

        Cursor.visible = shouldShowCursor;
        Cursor.lockState = shouldShowCursor ? CursorLockMode.None : CursorLockMode.Locked;

        if (starterInputs != null)
        {
            starterInputs.cursorLocked = !shouldShowCursor;
            starterInputs.cursorInputForLook = !shouldShowCursor;

            if (shouldShowCursor)
            {
                starterInputs.LookInput(Vector2.zero);
            }
        }
    }

    public void RefreshUIState()
    {
        ApplyInputAndCursorState();
    }

    public void updateScore(int score)
    {
        scoreText.text = "Score : " + score.ToString();
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        int safeMaxHealth = Mathf.Max(1, maxHealth);
        int clampedHealth = Mathf.Clamp(currentHealth, 0, safeMaxHealth);

        if (hpText != null)
        {
            hpText.text = "HP " + clampedHealth + "/" + safeMaxHealth;
        }

        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = (float)clampedHealth / safeMaxHealth;
        }
    }

    public void toggleMenu()
    {
        if (MenuPanel == null) return;

        bool isMenuOpen = !MenuPanel.activeSelf;
        MenuPanel.SetActive(isMenuOpen);
        ApplyInputAndCursorState();
    }

    public void toggleAdminPanel()
    {
        if (AdminPanel == null) return;

        bool isAdminPanelOpen = !AdminPanel.activeSelf;
        AdminPanel.SetActive(isAdminPanelOpen);
        if (MenuPanel != null)
        {
            MenuPanel.SetActive(!isAdminPanelOpen);
        }

        ApplyInputAndCursorState();

    }

    public void ReturnToMenu(params GameObject[] panelsToClose)
    {
        if (panelsToClose != null)
        {
            foreach (GameObject panel in panelsToClose)
            {
                if (panel != null)
                {
                    panel.SetActive(false);
                }
            }
        }

        if (MenuPanel != null)
        {
            MenuPanel.SetActive(true);
        }

        ApplyInputAndCursorState();
    }

    public void toggleGameoverScreen(bool isGameover)
    {
        if (GameoverScreen == null) return;

        GameoverScreen.SetActive(isGameover);
        ApplyInputAndCursorState();
    }

    public void restartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


}
