using TMPro;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // INSPECTOR FIELDS
    // UI TEXT ELEMENTS
    public TMP_Text HpText;
    public TMP_Text BatteryText;
    public TMP_Text ScoreText;
    public TMP_Text FinalTimeTakenText;
    public TMP_Text FinalSecretOrbsText;
    public TMP_Text FinalTotalDeathsText;
    public TMP_Text FinalScoreText;
    
    // ADJUSTABLE GAME SETTINGS
    public int currentBattery = 0;
    public int requiredBatteryCount = 10;
    public int totalSecretOrbs = 3;
    public int secretOrbBonusScore = 100;
    public int timePenaltyPerMinute = 20;
    public int deathPenaltyPerDeath = 20;

    // UI ELEMENT REFERENCES
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

    // RUNTIME STATE (END GAME STATS)
    private float elapsedSeconds;
    private bool timerRunning = true;
    private int liveScore;
    private int secretOrbsFound;
    private int deathCount;

    // UNITY LIFECYCLE
    private void Awake()
    {
        // Cache the input reference used for cursor and look control
        starterInputs = FindFirstObjectByType<StarterAssetsInputs>();
    }

    private void Start()
    {
        // Initialize score text and cursor state when the scene starts
        RefreshScoreUI();
        ApplyInputAndCursorState();
    }

    private void Update()
    {
        // Keep track of elapsed time while gameplay is active
        if (timerRunning)
        {
            elapsedSeconds += Time.deltaTime;
        }
    }

    // CURSOR AND INPUT STATE
    private void ApplyInputAndCursorState()
    {
        // Show the cursor whenever a menu-style panel is open
        bool isGameoverOpen = GameoverScreen != null && GameoverScreen.activeSelf;
        bool isFinalResultsOpen = FinalResultsScreen != null && FinalResultsScreen.activeSelf;
        bool isMenuOpen = MenuPanel != null && MenuPanel.activeSelf;
        bool isAdminOpen = AdminPanel != null && AdminPanel.activeSelf;

        // Hide the crosshair when UI needs the mouse
        bool shouldShowCursor = isGameoverOpen || isFinalResultsOpen || isMenuOpen || isAdminOpen;
        if (Crosshair != null) 
        {
            Crosshair.SetActive(!shouldShowCursor);
        }

        // Set the cursor visibility and lock state to match the current UI state
        Cursor.visible = shouldShowCursor;
        Cursor.lockState = shouldShowCursor ? CursorLockMode.None : CursorLockMode.Locked;

        // Disable look input while the cursor is visible
        if (starterInputs != null)
        {
            starterInputs.cursorLocked = !shouldShowCursor;
            starterInputs.cursorInputForLook = !shouldShowCursor;

            // Clear look input so the camera does not drift when UI opens
            if (shouldShowCursor)
            {
                starterInputs.LookInput(Vector2.zero);
            }
        }
    }
    public void RefreshUIState()
    {
        // Refresh cursor and input state after a panel changes
        ApplyInputAndCursorState();
    }

    // RESOURCE UI
    public void UpdateBattery(int battery)
    {
        // Add the collected batteries to the running total
        currentBattery += battery;
        if (BatteryText != null)
        {
            BatteryText.text = "Battery Collected : " + currentBattery.ToString() + " / " + requiredBatteryCount + " ";
        }
        
        // Show the collection message once the required amount is reached
        if (currentBattery >= requiredBatteryCount && uiMessage != null)
        {
            uiMessage.ShowBatteriesCollectedMessage();
        }
    }

    public bool HasRequiredBatteries()
    {
        // Check whether the player has enough batteries to use generator doors
        return currentBattery >= requiredBatteryCount;
    }

    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        // Clamp the displayed health so the UI never shows invalid values
        int safeMaxHealth = Mathf.Max(1, maxHealth);
        int clampedHealth = Mathf.Clamp(currentHealth, 0, safeMaxHealth);

        // Update the health text to show the current and maximum health values
        if (HpText != null)
        {
            HpText.text = clampedHealth + "/" + safeMaxHealth;
        }
        
        // Update the health bar fill amount to reflect the current health percentage
        if (HealthFillImage != null)
        {
            HealthFillImage.fillAmount = (float)clampedHealth / safeMaxHealth;
        }
    }
    // SCORING AND STATS
    public void RegisterInteraction(int scoreValue)
    {
        // Add score for a completed interaction and refresh the display
        liveScore += scoreValue;
        RefreshScoreUI();
    }

    public void RegisterSecretOrb(int scoreValue)
    {
        // Track collected secrets and award their score bonus
        secretOrbsFound++;
        RegisterInteraction(scoreValue);
        RefreshScoreUI();
        
        // Open the achievement panel once all secrets are found
        if (secretOrbsFound >= totalSecretOrbs)
        {
            ShowAchievement();
        }
    }
    
    private void ShowAchievement()
    {
        if (AchievementPanel != null)
        {
            // Show the achievement panel and hide it after a short delay
            AchievementPanel.SetActive(true);
            StartCoroutine(HideAchievementAfterDelay(3f));
        }
    }
    
    private System.Collections.IEnumerator HideAchievementAfterDelay(float delay)
    {
        // Hide the achievement panel after a short delay
        yield return new WaitForSeconds(delay);
        if (AchievementPanel != null)
        {
            AchievementPanel.SetActive(false);
        }
    }

    public void RegisterDeath()
    {
        // Track the player's death count for final scoring
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
        // Apply secret, time, and death adjustments to the live score
        int baseScore = liveScore + (secretOrbsFound * secretOrbBonusScore);
        int minutesTaken = Mathf.FloorToInt(elapsedSeconds / 60f);
        int finalScore = baseScore - (minutesTaken * timePenaltyPerMinute) - (deathCount * deathPenaltyPerDeath);
        return Mathf.Max(0, finalScore);
    }
    // ENDGAME UI
    public void ShowFinalResults()
    {
        // Stop the timer and populate the final results screen
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
        // Refresh the UI state to ensure the cursor is visible and input is disabled
        ApplyInputAndCursorState();
    }
    // UI HELPERS
    private void RefreshScoreUI()
    {
        if (ScoreText != null)
        {
            // Keep the score text in sync with the current score
            ScoreText.text = "Score : " + GetLiveScore();
        }
    }

    private static string FormatTime(float seconds)
    {
        // Convert seconds into a mm:ss display string
        int totalSeconds = Mathf.Max(0, Mathf.FloorToInt(seconds));
        int minutes = totalSeconds / 60;
        int remainingSeconds = totalSeconds % 60;
        return minutes.ToString("00") + ":" + remainingSeconds.ToString("00");
    }
    // PANEL TOGGLES
    public void ToggleMenu()
    {
        if (MenuPanel == null) return;

        // Open or close the menu panel
        bool isMenuOpen = !MenuPanel.activeSelf;
        MenuPanel.SetActive(isMenuOpen);
        ApplyInputAndCursorState();
    }
    public void ToggleScrewdriverIcon()
    {
        if (ScrewdriverIcon == null) return;

        // Toggle the screwdriver icon visibility
        bool isIconActive = !ScrewdriverIcon.activeSelf;
        ScrewdriverIcon.SetActive(isIconActive);
    }

    public void ToggleGasmaskIcon()
    {
        if (GasmaskIcon == null) return;

        // Toggle the gasmask icon visibility
        bool isIconActive = !GasmaskIcon.activeSelf;
        GasmaskIcon.SetActive(isIconActive);
    }

    public void ToggleGameoverScreen(bool isGameover)
    {
        if (GameoverScreen == null) return;

        // Hide other panels when the game over screen is shown
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
        // Show or hide the game over screen
        GameoverScreen.SetActive(isGameover);
        ApplyInputAndCursorState();
    }
    // SCENE CONTROLS
    public void RestartGame()
    {
        // Reload the first scene to restart the game
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

}
