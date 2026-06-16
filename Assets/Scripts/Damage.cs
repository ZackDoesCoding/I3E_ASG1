using UnityEngine;
using System.Collections.Generic;

public class Damage : MonoBehaviour
{
    public int DamageAmount = 1;
    public float DamageIntervalSeconds = 1f;

    private readonly Dictionary<int, float> nextDamageTimeByPlayer = new Dictionary<int, float>();

    public bool DamagePlayer(PlayerScript playerScript)
    {
        if (playerScript == null) return false;

        return playerScript.ChangeHealth(-DamageAmount);
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
