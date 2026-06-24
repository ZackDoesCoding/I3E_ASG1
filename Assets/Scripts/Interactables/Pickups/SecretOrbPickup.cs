using UnityEngine;

public class SecretOrbPickup : MonoBehaviour, IInteractable
{
    public int scoreValue = 10;

    private bool hasCollected;

    public void Interact(PlayerScript player)
    {
        // Prevent duplicate pickup handling and invalid interactions
        if (hasCollected || player == null)
        {
            return;
        }

        hasCollected = true;

        // Resolve UI manager from player or scene if missing
        UIManager uiManager = player.uiManager;
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
            player.uiManager = uiManager;
        }

        // Register secret orb collection and score
        if (uiManager != null)
        {
            uiManager.RegisterSecretOrb(scoreValue);
        }

        // Play pickup feedback and remove object from scene
        player.PlayPickupAudio();
        Destroy(gameObject);
    }
}