using TMPro;
using UnityEngine;

public class ModifyHealth : MonoBehaviour
{
    public PlayerScript playerScript;
    public UIManager uiManager;
    public TMP_InputField healthInputField;

    private void Awake()
    {
        if (playerScript == null)
        {
            playerScript = FindFirstObjectByType<PlayerScript>();
        }

        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }

        SyncInputWithCurrentMaxHealth();
    }

    private void OnEnable()
    {
        SyncInputWithCurrentMaxHealth();
    }

    private bool TryGetPlayer()
    {
        if (playerScript == null)
        {
            playerScript = FindFirstObjectByType<PlayerScript>();
        }

        return playerScript != null;
    }

    private bool TryGetUIManager()
    {
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }

        return uiManager != null;
    }

    private bool TryGetInputValue(out int value)
    {
        value = 0;

        if (healthInputField == null) return false;

        if (string.IsNullOrWhiteSpace(healthInputField.text))
        {
            if (!TryGetPlayer()) return false;

            value = playerScript.MaxHealth;
            return true;
        }

        return int.TryParse(healthInputField.text, out value);
    }

    private void ReturnToMenuAndRefreshInput()
    {
        ReturnToNormalMenu();

        SyncInputWithCurrentMaxHealth();
    }

    public void OpenAdminPanel()
    {
        if (!TryGetUIManager()) return;

        if (uiManager.AdminPanel != null)
        {
            uiManager.AdminPanel.SetActive(true);
        }

        if (uiManager.MenuPanel != null)
        {
            uiManager.MenuPanel.SetActive(false);
        }

        SyncInputWithCurrentMaxHealth();
        uiManager.RefreshUIState();
    }

    public void ReturnToNormalMenu()
    {
        if (!TryGetUIManager()) return;

        if (uiManager.AdminPanel != null)
        {
            uiManager.AdminPanel.SetActive(false);
        }

        if (uiManager.MenuPanel != null)
        {
            uiManager.MenuPanel.SetActive(true);
        }

        uiManager.RefreshUIState();
    }

    public void ToggleAdminPanel()
    {
        if (!TryGetUIManager()) return;

        bool openAdminPanel = uiManager.AdminPanel != null && !uiManager.AdminPanel.activeSelf;

        if (openAdminPanel)
        {
            OpenAdminPanel();
        }
        else
        {
            ReturnToNormalMenu();
        }
    }

    public void SyncInputWithCurrentMaxHealth()
    {
        if (healthInputField == null) return;
        if (!TryGetPlayer()) return;

        healthInputField.text = playerScript.MaxHealth.ToString();
    }

    public void ModifyHealthFromInput()
    {
        if (!TryGetPlayer()) return;
        if (!TryGetInputValue(out int maxHealthValue)) return;

        playerScript.SetMaxHealth(maxHealthValue);
        ReturnToMenuAndRefreshInput();
    }

    public void HealToMax()
    {
        if (!TryGetPlayer()) return;

        playerScript.SetHealth(playerScript.MaxHealth);
    }

    public void KillPlayer()
    {
        if (!TryGetPlayer()) return;

        playerScript.SetHealth(0);
    }

    public void ExitAdminPanel()
    {
        ReturnToMenuAndRefreshInput();
    }

}
