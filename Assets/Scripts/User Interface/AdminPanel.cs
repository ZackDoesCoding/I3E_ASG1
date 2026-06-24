using TMPro;
using UnityEngine;

public class AdminPanel : MonoBehaviour
{
    // INSPECTOR FIELDS
    public PlayerScript playerScript;
    public UIManager uiManager;
    public TMP_InputField healthInputField;
    
    // UNITY LIFECYCLE
    private void Awake()
    {   
        // Ensure references are set
        if (playerScript == null)
        {
            playerScript = FindFirstObjectByType<PlayerScript>();
        }
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }

        // Sync the input field with the current max health on awake
        SyncInputWithCurrentMaxHealth();
    }

    private void OnEnable() // Called when the admin panel is enabled
    {
        SyncInputWithCurrentMaxHealth();
    }

    // INPUT HANDLING
    // Tries to parse the input field value into an integer
    private bool TryGetInputValue(out int value) 
    {
        value = 0; // Default value in case of failure
        // Check if the input field is null or empty, and if so, use the player's current max health
        if (healthInputField == null) return false;
        if (string.IsNullOrWhiteSpace(healthInputField.text))
        {
            if (playerScript == null) return false;

            value = playerScript.MaxHealth;
            return true;
        }

        // Attempt to parse the input field text into an integer
        return int.TryParse(healthInputField.text, out value);
    }

    // PANEL FLOW
    public void OpenAdminPanel() // Admin panel button functionality
    {
        if (uiManager == null) return;
        // Show the admin panel and hide the normal menu panel
        if (uiManager.AdminPanel != null)
        {
            uiManager.AdminPanel.SetActive(true);
        }
        // Hide the normal menu panel
        if (uiManager.MenuPanel != null)
        {
            uiManager.MenuPanel.SetActive(false);
        }
        // Sync the input field with the current max health when opening the admin panel
        SyncInputWithCurrentMaxHealth();
        uiManager.RefreshUIState();
    }

    public void ReturnToNormalMenu() 
    {
        if (uiManager == null) return;
        // Hide the admin panel
        if (uiManager.AdminPanel != null)
        {
            uiManager.AdminPanel.SetActive(false);
        }
        // Show the normal menu panel
        if (uiManager.MenuPanel != null)
        {
            uiManager.MenuPanel.SetActive(true);
        }
        // Refresh the UI state to reflect the changes
        uiManager.RefreshUIState();
    }
    // HEALTH ADMIN ACTIONS
    public void SyncInputWithCurrentMaxHealth()
    {
        if (healthInputField == null) return;
        if (playerScript == null) return;
        // Update the input field to reflect the player's current max health
        healthInputField.text = playerScript.MaxHealth.ToString();
    }

    public void ConfirmMaxHealthFromInput() // Confirm button functionality
    {
        if (playerScript == null) return;
        if (!TryGetInputValue(out int maxHealthValue)) return;
        // Set the player's max health to the value from the input field
        playerScript.SetMaxHealth(maxHealthValue);
    }

    public void HealToMax() //Heal button functionality
    {
        if (playerScript == null) return;
        // Set the player's health to their max health, effectively healing them to full
        playerScript.SetHealth(playerScript.MaxHealth);
    }

    public void KillPlayer() //Kill button functionality
    {
        if (playerScript == null) return;
        // Set the player's health to zero, effectively "killing" the player
        playerScript.SetHealth(0);
    }

    public void ExitAdminPanel() // Exit button functionality
    {
        // Return to the normal menu and sync the input field with the current max health
        ReturnToNormalMenu();
        SyncInputWithCurrentMaxHealth();
    }

}
