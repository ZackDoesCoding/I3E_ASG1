using TMPro;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text HpText;
    public TMP_Text BatteryText;
    public TMP_Text ScoreText;
    public TMP_Text FinalTimeTakenText;
    public TMP_Text FinalSecretOrbsText;
    public TMP_Text FinalTotalDeathsText;
    public TMP_Text FinalScoreText;
    public int currentBattery = 0;
    public int totalSecretOrbs = 3;
    public int secretOrbBonusScore = 100;
    public int timePenaltyPerMinute = 20;
    public int deathPenaltyPerDeath = 20;
    public Image HealthFillImage;
    public GameObject MenuPanel;
    public GameObject AdminPanel;
    public GameObject Crosshair;
    public GameObject GameoverScreen;
    public GameObject FinalResultsScreen;
    public GameObject AchievementPanel;
    public GameObject GasmaskIcon;
    public GameObject ScrewdriverIcon;
    public StarterAssetsInputs starterInputs;
    public UIMessage uiMessage;

    private float elapsedSeconds;
    private bool timerRunning = true;
    private int liveScore;
    private int secretOrbsFound;
    private int deathCount;

    private void Awake()
    {
        starterInputs = FindFirstObjectByType<StarterAssetsInputs>();
    }

    void Start()
    {
        RefreshScoreUI();
        ApplyInputAndCursorState();
    }

    private void Update()
    {
        if (timerRunning)
        {
            elapsedSeconds += Time.deltaTime;
        }
    }

    private void ApplyInputAndCursorState()
    {
        bool isGameoverOpen = GameoverScreen != null && GameoverScreen.activeSelf;
        bool isFinalResultsOpen = FinalResultsScreen != null && FinalResultsScreen.activeSelf;
        bool isMenuOpen = MenuPanel != null && MenuPanel.activeSelf;
        bool isAdminOpen = AdminPanel != null && AdminPanel.activeSelf;
        bool shouldShowCursor = isGameoverOpen || isFinalResultsOpen || isMenuOpen || isAdminOpen;

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
            BatteryText.text = "Battery Collected : " + currentBattery.ToString() + " / 5 ";
        }

        if (currentBattery >= 5 && uiMessage != null)
        {
            uiMessage.ShowBatteriesCollectedMessage();
        }
    }

    public void RegisterInteraction(int scoreValue)
    {
        liveScore += scoreValue;
        RefreshScoreUI();
    }

    public void RegisterSecretOrb(int scoreValue)
    {
        secretOrbsFound++;
        RegisterInteraction(scoreValue);
        RefreshScoreUI();
        
        // Check if all secrets collected
        if (secretOrbsFound >= totalSecretOrbs)
        {
            ShowAchievement();
        }
    }
    
    private void ShowAchievement()
    {
        if (AchievementPanel != null)
        {
            AchievementPanel.SetActive(true);
            StartCoroutine(HideAchievementAfterDelay(3f));
        }
    }
    
    private System.Collections.IEnumerator HideAchievementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (AchievementPanel != null)
        {
            AchievementPanel.SetActive(false);
        }
    }

    public void RegisterDeath()
    {
        deathCount++;
        RefreshScoreUI();
    }

    public int GetTotalDeaths()
    {
        return deathCount;
    }

    public float GetElapsedSeconds()
    {
        return elapsedSeconds;
    }

    public int GetLiveScore()
    {
        return liveScore;
    }

    public int GetFinalScore()
    {
        int baseScore = liveScore + (secretOrbsFound * secretOrbBonusScore);
        int minutesTaken = Mathf.FloorToInt(elapsedSeconds / 60f);
        int finalScore = baseScore - (minutesTaken * timePenaltyPerMinute) - (deathCount * deathPenaltyPerDeath);
        return Mathf.Max(0, finalScore);
    }

    public void ShowFinalResults()
    {
        timerRunning = false;

        if (FinalResultsScreen != null)
        {
            FinalResultsScreen.SetActive(true);
        }

        if (FinalTimeTakenText != null)
        {
            FinalTimeTakenText.text = "Time Taken : " + FormatTime(elapsedSeconds);
        }

        if (FinalSecretOrbsText != null)
        {
            FinalSecretOrbsText.text = "Secret Orbs Found : " + secretOrbsFound + "/" + Mathf.Max(0, totalSecretOrbs);
        }

        if (FinalTotalDeathsText != null)
        {
            FinalTotalDeathsText.text = "Total Deaths : " + deathCount;
        }

        if (FinalScoreText != null)
        {
            FinalScoreText.text = "Final Score : " + GetFinalScore();
        }

        ApplyInputAndCursorState();
    }

    private void RefreshScoreUI()
    {
        if (ScoreText != null)
        {
            ScoreText.text = "Score : " + GetLiveScore();
        }
    }

    private static string FormatTime(float seconds)
    {
        int totalSeconds = Mathf.Max(0, Mathf.FloorToInt(seconds));
        int minutes = totalSeconds / 60;
        int remainingSeconds = totalSeconds % 60;
        return minutes.ToString("00") + ":" + remainingSeconds.ToString("00");
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

        if (isGameover)
        {
            if (MenuPanel != null)
            {
                MenuPanel.SetActive(false);
            }

            if (AdminPanel != null)
            {
                AdminPanel.SetActive(false);
            }
        }

        GameoverScreen.SetActive(isGameover);
        ApplyInputAndCursorState();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
