using UnityEngine;

public class Healing : MonoBehaviour
{
    public int HealAmount = 10;

    public bool HealPlayer(PlayerScript playerScript)
    {
        // Null check to avoid applying healing to a missing player
        if (playerScript == null) return false;

        // Delegate health change to the player health API
        return playerScript.ChangeHealth(HealAmount);
    }
}
