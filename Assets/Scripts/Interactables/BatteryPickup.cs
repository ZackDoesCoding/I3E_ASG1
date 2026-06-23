using UnityEngine;

public class BatteryPickup : MonoBehaviour, IInteractable
{
    public int scoreValue = 10;
    public int batteryAmount = 1;

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
            uiManager.UpdateBattery(batteryAmount);
            uiManager.RegisterInteraction(scoreValue);
        }

        player.PlayPickupAudio();
        Destroy(gameObject);
    }
}