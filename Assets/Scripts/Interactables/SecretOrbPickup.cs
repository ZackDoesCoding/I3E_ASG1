using UnityEngine;

public class SecretOrbPickup : MonoBehaviour, IInteractable
{
    public int scoreValue = 10;

    private bool hasCollected;

    public void Interact(PlayerScript player)
    {
        if (hasCollected || player == null)
        {
            return;
        }

        hasCollected = true;

        UIManager uiManager = player.uiManager;
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
            player.uiManager = uiManager;
        }

        if (uiManager != null)
        {
            uiManager.RegisterSecretOrb(scoreValue);
        }

        player.PlayPickupAudio();
        Destroy(gameObject);
    }
}