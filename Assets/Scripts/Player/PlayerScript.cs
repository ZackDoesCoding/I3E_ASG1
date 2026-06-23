using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    public UIManager uiManager;
    public AudioSource pickupAudioSource;
    public AudioClip pickupAudioClip;
    public int Health = 100;
    public int MaxHealth = 100;
    public bool Screwdriver = false;
    public bool GasMask = false;
    public int Deaths = 0;
    private bool hasReachedExit = false;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private void Awake()
    {
        if (GetComponent<PlayerInteract>() == null)
        {
            gameObject.AddComponent<PlayerInteract>();
        }
    }

    private void Start()
    {
        // Try to find spawn point in scene
        GameObject spawnPointObject = GameObject.FindWithTag("spawnpoint");
        if (spawnPointObject != null)
        {
            spawnPosition = spawnPointObject.transform.position;
            spawnRotation = spawnPointObject.transform.rotation;
        }
        else
        {
            // Fall back to current player position
            spawnPosition = transform.position;
            spawnRotation = transform.rotation;
        }

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
        if (other.CompareTag("exit"))
        {
            if (hasReachedExit)
            {
                return;
            }

            hasReachedExit = true;

            if (uiManager == null)
            {
                uiManager = FindFirstObjectByType<UIManager>();
            }

            if (uiManager != null)
            {
                uiManager.ShowFinalResults();
            }

            return;
        }

        if (other.CompareTag("healing"))
        {
            Healing healing = other.GetComponent<Healing>();
            if (healing == null)
            {
                return;
            }

            healing.HealPlayer(this);

            Destroy(other.gameObject);
        }
        else
        {
            Damage damage = other.GetComponent<Damage>();
            if (damage != null)
            {
                damage.DamagePlayerPeriodic(this);
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        Damage damage = other.GetComponent<Damage>();
        if (damage == null) return;

        damage.DamagePlayerPeriodic(this);
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

        if (didReachZero)
        {
            HandleDeath();
        }

        return didChange;
    }

    private void HandleDeath()
    {
        Deaths++;

        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }

        if (uiManager != null)
        {
            uiManager.RegisterDeath();
            uiManager.ToggleGameoverScreen(true);
        }
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

        return didChange;
    }

    public bool ChangeHealth(int delta)
    {
        return SetHealth(Health + delta);
    }

    public void PlayPickupAudio()
    {
        if (pickupAudioSource == null || pickupAudioClip == null)
        {
            return;
        }

        pickupAudioSource.PlayOneShot(pickupAudioClip);
    }



    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private System.Collections.IEnumerator RespawnCoroutine()
    {
        // Wait for end of frame so First Person Controller doesn't override position
        yield return new WaitForEndOfFrame();

        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        
        Health = MaxHealth;
        RefreshHealthUI();

        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }

        if (uiManager != null)
        {
            uiManager.ToggleGameoverScreen(false);
        }
    }

}
