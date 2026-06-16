using UnityEngine;

public class Healing : MonoBehaviour
{
    public int HealAmount = 10;

    public bool HealPlayer(PlayerScript playerScript)
    {
        if (playerScript == null) return false;

        return playerScript.ChangeHealth(HealAmount);
    }
}
