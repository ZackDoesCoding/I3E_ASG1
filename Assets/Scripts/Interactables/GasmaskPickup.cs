using UnityEngine;

public class GasmaskPickup : MonoBehaviour, IInteractable
{
    public int scoreValue = 10;
    public UIMessage uiMessage;

    private bool hasCollected;

    public void Interact(PlayerScript player)
    {
        if (hasCollected || player == null)
        {
            return;
        }

        hasCollected = true;
        player.GasMask = true;

        UIManager uiManager = player.uiManager;
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
            player.uiManager = uiManager;
        }

        if (uiManager != null)
        {
            uiManager.ToggleGasmaskIcon();
            uiManager.RegisterInteraction(scoreValue);
        }

        if (uiMessage == null)
        {
            PlayerInteract playerInteract = player.GetComponent<PlayerInteract>();
            if (playerInteract != null)
            {
                uiMessage = playerInteract.uiMessage;
            }
        }

        if (uiMessage != null)
        {
            uiMessage.ShowGasmaskMessage();
        }

        player.PlayPickupAudio();
        Destroy(gameObject);
    }
}