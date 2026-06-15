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
    public GameObject Crosshair;
    public GameObject GameoverScreen;
    public StarterAssetsInputs starterInputs;

    private void Awake()
    {
        starterInputs = FindFirstObjectByType<StarterAssetsInputs>();
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        Crosshair.SetActive(!isMenuOpen);
        Cursor.visible = isMenuOpen;
        Cursor.lockState = isMenuOpen ? CursorLockMode.None : CursorLockMode.Locked;

        if (starterInputs != null)
        {
            starterInputs.cursorLocked = !isMenuOpen;
            starterInputs.cursorInputForLook = !isMenuOpen;

            if (isMenuOpen)
            {
                starterInputs.LookInput(Vector2.zero);
            }
        }
    }

    public void toggleGameoverScreen(bool isGameover)
    {
        if (GameoverScreen == null) return;

        GameoverScreen.SetActive(isGameover);
        Crosshair.SetActive(!isGameover);
        Cursor.visible = isGameover;
        Cursor.lockState = isGameover ? CursorLockMode.None : CursorLockMode.Locked;

        if (starterInputs != null)
        {
            starterInputs.cursorLocked = !isGameover;
            starterInputs.cursorInputForLook = !isGameover;

            if (isGameover)
            {
                starterInputs.LookInput(Vector2.zero);
            }
        }
    }

    public void restartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


}
