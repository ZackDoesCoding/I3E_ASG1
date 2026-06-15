using UnityEngine;

public class Healing : MonoBehaviour
{
    public int HealAmount = 1;

    public bool HealPlayer(PlayerScript playerScript)
    {
        if (playerScript == null || playerScript.Health >= playerScript.MaxHealth) return false;

        int previousHealth = playerScript.Health;
        playerScript.Health = Mathf.Min(playerScript.Health + HealAmount, playerScript.MaxHealth);
        return playerScript.Health != previousHealth;
    }
}
