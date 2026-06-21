using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public UIManager uiManager;
    public int Health = 100;
    public int MaxHealth = 100;
    public bool Screwdriver = false;
    public bool GasMask = false;

    private void Awake()
    {
        if (GetComponent<PlayerInteract>() == null)
        {
            gameObject.AddComponent<PlayerInteract>();
        }
    }

    private void Start()
    {
        SetHealth(Health);
    }

    private void OnMenu(InputValue value)
    {
        if (!value.isPressed) return;
        if (uiManager == null) return;

        uiManager.ToggleMenu();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered with: " + other.gameObject.name + " | tag=" + other.tag);

        if (other.CompareTag("healing"))
        {
            Healing healing = other.GetComponent<Healing>();
            if (healing == null)
            {
                Debug.LogWarning("Healing object is missing Healing on: " + other.gameObject.name);
                return;
            }

            if (healing.HealPlayer(this))
            {
                Debug.Log("Player healed. Current health: " + Health);
            }

            Destroy(other.gameObject);
        }
        else if (other.CompareTag("damage"))
        {
            Damage damage = other.GetComponent<Damage>();
            if (damage == null)
            {
                Debug.LogWarning("Damage object is missing Damage on: " + other.gameObject.name);
                return;
            }

            if (damage.DamagePlayerPeriodic(this))
            {
                Debug.Log("Player damaged. Current health: " + Health);
            }
        }
        else if (other.CompareTag("battery"))
        {
            Debug.Log("Battery collected.");

            if (uiManager != null)
            {
                uiManager.UpdateBattery(1);
            }

            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("damage")) return;

        Damage damage = other.GetComponent<Damage>();
        if (damage == null) return;

        if (damage.DamagePlayerPeriodic(this))
        {
            Debug.Log("Player damaged. Current health: " + Health);
        }
    }

    public void RefreshHealthUI()
    {
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }

        if (uiManager == null) return;

        uiManager.UpdateHealthUI(Health, MaxHealth);
    }

    public bool SetHealth(int newHealth)
    {
        int safeMaxHealth = Mathf.Max(1, MaxHealth);
        int clampedHealth = Mathf.Clamp(newHealth, 0, safeMaxHealth);
        bool didReachZero = Health > 0 && clampedHealth <= 0;
        bool didChange = Health != clampedHealth;

        Health = clampedHealth;
        RefreshHealthUI();
        UpdateGameoverState();

        if (didReachZero)
        {
            Debug.Log("Player died.");
        }

        return didChange;
    }

    public bool SetMaxHealth(int newMaxHealth)
    {
        int safeMaxHealth = Mathf.Max(1, newMaxHealth);
        bool didChange = MaxHealth != safeMaxHealth;

        MaxHealth = safeMaxHealth;

        if (Health > MaxHealth)
        {
            Health = MaxHealth;
            didChange = true;
        }

        RefreshHealthUI();
        UpdateGameoverState();

        return didChange;
    }

    public bool ChangeHealth(int delta)
    {
        return SetHealth(Health + delta);
    }

    private void UpdateGameoverState()
    {
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }

        if (uiManager == null) return;

        uiManager.ToggleGameoverScreen(Health <= 0);
    }

}
