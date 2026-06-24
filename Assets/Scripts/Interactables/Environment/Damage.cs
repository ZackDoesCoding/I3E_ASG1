using UnityEngine;
using System.Collections.Generic;

public class Damage : MonoBehaviour
{
    // Create an enum to define different damage modes for flexibility and 
    // ease of use in the Unity Inspector
    public enum DamageMode
    {
        Normal,
        InstantKill,
        PercentOfMaxHealth
    }
    
    public DamageMode Mode = DamageMode.Normal;
    public int DamageAmount = 10;
    [Range(0f, 100f)] public float DamagePercentOfMaxHealth = 100f;
    public float DamageIntervalSeconds = 1f;

    // Creates a dictionary to track the next allowed damage time for each player
    private readonly Dictionary<int, float> nextDamageTimeByPlayer = new Dictionary<int, float>();

    private int GetDamageAmount(PlayerScript playerScript)
    {
        if (playerScript == null)
        {
            return 0;
        }

        // Choose how damage should be calculated based on configured mode
        switch (Mode)
        {
            case DamageMode.InstantKill:  //instant kill mode, return the player's max health to ensure they are killed
                return playerScript.MaxHealth;
            case DamageMode.PercentOfMaxHealth:  //percent of max health mode, calculate damage based on player's max health
                return Mathf.CeilToInt(playerScript.MaxHealth * Mathf.Clamp01(DamagePercentOfMaxHealth / 100f));
            default:
                return DamageAmount;
        }
    }

    private bool IsImmuneToDamage(PlayerScript playerScript)
    {
        // check if the player has a gas mask and the damage source is poison
        return playerScript != null && playerScript.GasMask && gameObject.CompareTag("poison");
    }

    public bool DamagePlayer(PlayerScript playerScript)
    {
        // Guard against invalid target or poison immunity
        if (playerScript == null) return false;
        if (IsImmuneToDamage(playerScript)) return false;

        // Apply damage through centralized player health API
        int damageAmount = Mathf.Max(0, GetDamageAmount(playerScript));
        return playerScript.ChangeHealth(-damageAmount);
    }

    public bool DamagePlayerPeriodic(PlayerScript playerScript)
    {
        // Skip dead players, invalid targets, and immune states
        if (playerScript == null || playerScript.Health <= 0) return false;
        if (IsImmuneToDamage(playerScript)) return false;

        // Use the player's instance ID to uniquely identify them in the damage tracking dictionary
        int playerId = playerScript.GetInstanceID();
        float now = Time.time;

        // Throttle repeated damage per player using a cooldown window
        if (nextDamageTimeByPlayer.TryGetValue(playerId, out float nextDamageTime) && now < nextDamageTime)
        {
            return false;
        }

        bool didDamage = DamagePlayer(playerScript);

        if (!didDamage) return false;

        // Store next allowed damage time for this player
        float safeInterval = Mathf.Max(0.01f, DamageIntervalSeconds);
        nextDamageTimeByPlayer[playerId] = now + safeInterval;

        return didDamage;
    }

    public void ResetDamageTimer(PlayerScript playerScript)
    {
        if (playerScript == null) return;

        // Remove the player's entry from the damage tracking dictionary 
        // to reset their damage cooldown
        int playerId = playerScript.GetInstanceID();
        if (nextDamageTimeByPlayer.ContainsKey(playerId))
        {
            nextDamageTimeByPlayer.Remove(playerId);
        }
    }
}
