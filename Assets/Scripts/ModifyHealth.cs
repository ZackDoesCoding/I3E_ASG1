using UnityEngine;
using System.Collections.Generic;

public class ModifyHealth : MonoBehaviour
{
    public int Damage = 1;
    public int HealAmount = 1;
    public float DamageIntervalSeconds = 1f;

    private readonly Dictionary<int, float> nextDamageTimeByPlayer = new Dictionary<int, float>();
    UIManager uiManager;

    public void Heal(PlayerScript playerScript)
    {
        if (playerScript == null || playerScript.Health >= playerScript.MaxHealth) return;

        playerScript.Health = Mathf.Min(playerScript.Health + HealAmount, playerScript.MaxHealth);
    }

    public void DamagePlayer(PlayerScript playerScript)
    {
        if (playerScript == null || playerScript.Health <= 0) return;

        playerScript.Health = Mathf.Max(playerScript.Health - Damage, 0);

        if (playerScript.Health <= 0)
        {
            Debug.Log("Player died.");
            uiManager = FindFirstObjectByType<UIManager>();
            if (uiManager != null)
            {
                uiManager.toggleGameoverScreen(true);
            }
        }
    }

    public void DamagePlayerPeriodic(PlayerScript playerScript)
    {
        if (playerScript == null || playerScript.Health <= 0) return;

        int playerId = playerScript.GetInstanceID();
        float now = Time.time;

        if (nextDamageTimeByPlayer.TryGetValue(playerId, out float nextDamageTime) && now < nextDamageTime)
        {
            return;
        }

        DamagePlayer(playerScript);

        float safeInterval = Mathf.Max(0.01f, DamageIntervalSeconds);
        nextDamageTimeByPlayer[playerId] = now + safeInterval;
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
