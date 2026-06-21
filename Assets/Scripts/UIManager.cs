using TMPro;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text HpText;
    public TMP_Text BatteryText;
    private int currentBattery = 0;
    public Image HealthFillImage;
    public GameObject MenuPanel;
    public GameObject AdminPanel;
    public GameObject Crosshair;
    public GameObject GameoverScreen;
    public GameObject GasmaskIcon;
    public GameObject ScrewdriverIcon;
    public StarterAssetsInputs starterInputs;
    public UIMessage uiMessage;

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

    public void UpdateBattery(int battery)
    {
        currentBattery += battery;
        if (BatteryText != null)
        {
            BatteryText.text = "Battery Collected : " + currentBattery.ToString();
        }
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        int safeMaxHealth = Mathf.Max(1, maxHealth);
        int clampedHealth = Mathf.Clamp(currentHealth, 0, safeMaxHealth);

        if (HpText != null)
        {
            HpText.text = clampedHealth + "/" + safeMaxHealth;
        }

        if (HealthFillImage != null)
        {
            HealthFillImage.fillAmount = (float)clampedHealth / safeMaxHealth;
        }
    }

    public void ToggleMenu()
    {
        if (MenuPanel == null) return;

        bool isMenuOpen = !MenuPanel.activeSelf;
        MenuPanel.SetActive(isMenuOpen);
        ApplyInputAndCursorState();
    }
    public void ToggleScrewdriverIcon()
    {
        if (ScrewdriverIcon == null) return;

        bool isIconActive = !ScrewdriverIcon.activeSelf;
        ScrewdriverIcon.SetActive(isIconActive);
    }
    public void ToggleGasmaskIcon()
    {
        if (GasmaskIcon == null) return;

        bool isIconActive = !GasmaskIcon.activeSelf;
        GasmaskIcon.SetActive(isIconActive);
    }

    public void ToggleGameoverScreen(bool isGameover)
    {
        if (GameoverScreen == null) return;

        GameoverScreen.SetActive(isGameover);
        ApplyInputAndCursorState();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


}
