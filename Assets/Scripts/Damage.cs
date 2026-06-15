using UnityEngine;
using System.Collections.Generic;

public class Damage : MonoBehaviour
{
    public int DamageAmount = 1;
    public float DamageIntervalSeconds = 1f;

    private readonly Dictionary<int, float> nextDamageTimeByPlayer = new Dictionary<int, float>();
    private UIManager uiManager;

    public bool DamagePlayer(PlayerScript playerScript)
    {
        if (playerScript == null || playerScript.Health <= 0) return false;

        int previousHealth = playerScript.Health;
        playerScript.Health = Mathf.Max(playerScript.Health - DamageAmount, 0);

        if (playerScript.Health <= 0)
        {
            Debug.Log("Player died.");
            uiManager = FindFirstObjectByType<UIManager>();
            if (uiManager != null)
            {
                uiManager.toggleGameoverScreen(true);
            }
        }

        return playerScript.Health != previousHealth;
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
