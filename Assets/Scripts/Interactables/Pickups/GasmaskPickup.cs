using UnityEngine;

public class GasmaskPickup : MonoBehaviour, IInteractable
{
    public int scoreValue = 10;
    public UIMessage uiMessage;

    private bool hasCollected;

    public void Interact(PlayerScript player)
    {
        // Prevent duplicate pickup handling and invalid interactions
        if (hasCollected || player == null)
        {
            return;
        }

        // Apply pickup state on player
        hasCollected = true;
        player.GasMask = true;

        // Resolve UI manager from player or scene if missing
        UIManager uiManager = player.uiManager;
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
            player.uiManager = uiManager;
        }

        // Update UI icon and add interaction score
        if (uiManager != null)
        {
            uiManager.ToggleGasmaskIcon();
            uiManager.RegisterInteraction(scoreValue);
        }

        // Resolve message source if not set in inspector
        if (uiMessage == null)
        {
            PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
            if (playerInteract != null)
            {
                uiMessage = playerInteract.uiMessage;
            }
        }

        // Show pickup hint to player
        if (uiMessage != null)
        {
            uiMessage.ShowGasmaskMessage();
        }

        // Play pickup feedback and remove object from scene
        player.PlayPickupAudio();
        Destroy(gameObject);
    }
}