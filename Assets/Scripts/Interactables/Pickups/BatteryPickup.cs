using UnityEngine;

public class BatteryPickup : MonoBehaviour, IInteractable
{
    public int scoreValue = 10;
    public int batteryAmount = 1;

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

        // Apply battery gain and interaction score
        if (uiManager != null)
        {
            uiManager.UpdateBattery(batteryAmount);
            uiManager.RegisterInteraction(scoreValue);
        }

        // Play pickup audio and remove object from scene
        player.PlayPickupAudio();
        Destroy(gameObject);
    }
}