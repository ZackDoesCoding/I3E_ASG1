using UnityEngine;
using System.Collections.Generic;

public class Damage : MonoBehaviour
{
    public enum DamageMode
    {
        Normal,
        InstantKill,
        PercentOfMaxHealth
    }

    public DamageMode Mode = DamageMode.Normal;
    public int DamageAmount = 10;
    [Range(0f, 100f)]
    public float DamagePercentOfMaxHealth = 100f;
    public float DamageIntervalSeconds = 1f;

    private readonly Dictionary<int, float> nextDamageTimeByPlayer = new Dictionary<int, float>();

    private int GetDamageAmount(PlayerScript playerScript)
    {
        if (playerScript == null)
        {
            return 0;
        }
        switch (Mode)
        {
            case DamageMode.InstantKill:
                return playerScript.MaxHealth;
            case DamageMode.PercentOfMaxHealth:
                return Mathf.CeilToInt(playerScript.MaxHealth * Mathf.Clamp01(DamagePercentOfMaxHealth / 100f));
            default:
                return DamageAmount;
        }
    }

    public bool DamagePlayer(PlayerScript playerScript)
    {
        if (playerScript == null) return false;

        int damageAmount = Mathf.Max(0, GetDamageAmount(playerScript));
        return playerScript.ChangeHealth(-damageAmount);
    }

    public bool DamagePlayerPeriodic(PlayerScript playerScript)
    {
        if (playerScript == null || playerScript.Health <= 0) return false;

        int playerId = playerScript.GetInstanceID();
        float now = Time.time;

        if (nextDamageTimeByPlayer.TryGetValue(playerId, out float nextDamageTime) && now < nextDamageTime)
        {
            return false;
        }

        bool didDamage = DamagePlayer(playerScript);

        if (!didDamage) return false;

        float safeInterval = Mathf.Max(0.01f, DamageIntervalSeconds);
        nextDamageTimeByPlayer[playerId] = now + safeInterval;

        return didDamage;
    }

    public void ResetDamageTimer(PlayerScript playerScript)
    {
        if (playerScript == null) return;

        int playerId = playerScript.GetInstanceID();
        if (nextDamageTimeByPlayer.ContainsKey(playerId))
        {
            nextDamageTimeByPlayer.Remove(playerId);
        }
    }
}
