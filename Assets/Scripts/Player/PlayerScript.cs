using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    // INSPECTOR FIELDS
    public UIManager uiManager;
    public AudioSource pickupAudioSource;
    public AudioClip pickupAudioClip;
    public int Health = 100;
    public int MaxHealth = 100;
    public bool Screwdriver = false;
    public bool GasMask = false;
    public int Deaths = 0;
    // INTERNAL STATE
    private bool hasReachedExit = false;
    private CharacterController characterController;
    private Coroutine respawnRoutine;

    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    // UNITY LIFECYCLE
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

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
        // Initialize health and UI
        SetHealth(Health);
    }
    // INPUT
    // Called when the player presses the menu button (M key)
    private void OnMenu(InputValue value) 
    {
        if (!value.isPressed) return;
        if (uiManager == null) return;

        uiManager.ToggleMenu();
    }

    // TRIGGER EVENTS
    // Called when the player enters a trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Handle exit trigger
        if (other.CompareTag("exit"))
        {
            if (hasReachedExit)
            {
                return;
            }

            hasReachedExit = true;

            // UI Manager nullcheck and scene search if not linked
            if (uiManager == null)
            {
                uiManager = FindFirstObjectByType<UIManager>();
            }
            // Show final results if UI manager is available
            if (uiManager != null)
            {
                uiManager.ShowFinalResults();
            }

            return;
        }

        // Handle healing and damage triggers
        if (other.CompareTag("healing"))
        {
            // Attempt to get the Healing component from the trigger object
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
            // Attempt to get the Damage component from the trigger object
            Damage damage = other.GetComponent<Damage>(); 
            if (damage != null)
            {
                damage.DamagePlayerPeriodic(this);
            }
        }
    }

    // Called when the player stays within a trigger collider
    private void OnTriggerStay(Collider other) 
    {
        // Attempt to get the Damage component from the trigger object
        Damage damage = other.GetComponent<Damage>();
        if (damage == null) return;

        // Apply periodic damage to the player
        damage.DamagePlayerPeriodic(this);
    }
    
    // HEALTH API
    public void RefreshHealthUI()
    {
        //nullcheck and scene search if not linked
        if (uiManager == null) 
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }

        // Refreshes the health UI to reflect the current health and max health values
        if (uiManager == null) return;
        uiManager.UpdateHealthUI(Health, MaxHealth);
    }

    public bool SetHealth(int newHealth)
    {
        // Ensure that the new health value is clamped between 0 and the maximum health
        // and handle death if health reaches zero
        // math.max is used to ensure that MaxHealth is at least 1 to avoid invalid health values
        // math.clamp is used to ensure that the new health value is within the valid range
        int safeMaxHealth = Mathf.Max(1, MaxHealth);
        int clampedHealth = Mathf.Clamp(newHealth, 0, safeMaxHealth);
        bool didReachZero = Health > 0 && clampedHealth <= 0;
        bool didChange = Health != clampedHealth;

        // Update the player's health and refresh the UI
        Health = clampedHealth;
        RefreshHealthUI();
        if (didReachZero)
        {
            HandleDeath();
        }
        return didChange;
    }

    // Sets the maximum health of the player
    public bool SetMaxHealth(int newMaxHealth)
    {
        // Ensure that the new maximum health is at least 1 to avoid invalid health values
        int safeMaxHealth = Mathf.Max(1, newMaxHealth);
        bool didChange = MaxHealth != safeMaxHealth;
        MaxHealth = safeMaxHealth;

        // If the current health exceeds the new maximum health, adjust it accordingly
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
            didChange = true;
        }

        RefreshHealthUI(); // Refresh the health UI to reflect the new maximum health
        return didChange;
    }

    public bool ChangeHealth(int delta)
    {
        // Adjusts the player's health by a specified delta value, 
        // which can be positive (healing) or negative (damage)
        return SetHealth(Health + delta);
    }

    private void HandleDeath()
    {
        // Increment the death count and 
        Deaths++;

        if (uiManager == null) //uiManager nullcheck and scene search if not linked
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }

        // Update the UI to reflect the player's death
        if (uiManager != null)
        {
            uiManager.RegisterDeath();
            uiManager.ToggleGameoverScreen(true);
        }
    }
    // AUDIO
    public void PlayPickupAudio()
    {
        // Plays the pickup audio clip using the assigned audio source, if both are available
        if (pickupAudioSource == null || pickupAudioClip == null)
        {
            return;
        }
        pickupAudioSource.PlayOneShot(pickupAudioClip);
    }
    // RESPAWN
    public void Respawn()
    {
        // Restart the respawn flow so repeated calls do not stack
        if (respawnRoutine != null)
        {
            StopCoroutine(respawnRoutine);
        }

        respawnRoutine = StartCoroutine(RespawnCoroutine());
    }

    private System.Collections.IEnumerator RespawnCoroutine()
    {
        // Disable the controller while the player is being repositioned
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Wait for end of frame so movement code does not override the reset position
        yield return new WaitForEndOfFrame();

        // Reset the player's position and rotation to the spawn point
        transform.SetPositionAndRotation(spawnPosition, spawnRotation);

        // Restore full health and update the HUD
        SetHealth(MaxHealth);

        if (uiManager == null) //nullcheck and scene search if not linked
        {
            uiManager = FindFirstObjectByType<UIManager>();
        }
        // Hide the game over screen if the UI manager is available
        if (uiManager != null)
        {
            uiManager.ToggleGameoverScreen(false);
        }

        // Re-enable movement after the respawn position has been applied
        yield return null;
        if (characterController != null)
        {
            characterController.enabled = true;
        }

        respawnRoutine = null;
    }

}
