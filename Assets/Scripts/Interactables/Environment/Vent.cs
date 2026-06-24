using UnityEngine;

public class Vent : MonoBehaviour
{
    public ScewInteractable Screw1;
    public ScewInteractable Screw2;
    public ScewInteractable Screw3;
    public ScewInteractable Screw4;
    public float removeDelay = 1f;
    public AudioSource ventAudioSource;
    public AudioClip ventRemoveClip;
    public UIManager uiManager;
    public int scoreValue = 10;

    private bool isRemoving;
    private bool hasAwardedVentScore;

    private void Update()
    {
        // Continuously check if all screws are unscrewed to initiate vent removal
        RemoveScrews();
    }

    public void RemoveScrews()
    {
        // Prevent starting multiple removal coroutines
        if (isRemoving)
        {
            return;
        }

        // Wait until all screw references are valid
        if (Screw1 == null || Screw2 == null || Screw3 == null || Screw4 == null)
        {
            return;
        }

        // Start vent removal only when all screws are unscrewed
        if (Screw1.UnScrewed &&
            Screw2.UnScrewed &&
            Screw3.UnScrewed &&
            Screw4.UnScrewed)
        {
            StartCoroutine(RemoveAfterDelay());
        }
    }

    // Coroutine to handle vent removal after a delay 
    // (basically a timer to allow for audio playback or other effects)
    private System.Collections.IEnumerator RemoveAfterDelay()
    {
        isRemoving = true;
        // Wait for the specified delay before proceeding with vent removal 
        // (e.g., to allow for audio playback or other effects)
        yield return new WaitForSeconds(removeDelay);

        // Re-check screw state after delay before disabling vent pieces
        if (Screw1 != null && Screw2 != null && Screw3 != null && Screw4 != null &&
            Screw1.UnScrewed && Screw2.UnScrewed && Screw3.UnScrewed && Screw4.UnScrewed)
        {
            // Play vent removal audio if available
            if (ventAudioSource != null && ventRemoveClip != null)
            {
                ventAudioSource.PlayOneShot(ventRemoveClip);
            }

            // Disable all mesh renderers to visually remove the vent
            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = false;
            }
            
            // Disable all colliders to prevent further interaction with the vent
            Collider[] colliders = GetComponentsInChildren<Collider>(true);
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }

            // Award score only once after successful vent removal
            if (!hasAwardedVentScore)
            {
                if (uiManager == null)
                {
                    uiManager = FindFirstObjectByType<UIManager>();
                }

                if (uiManager != null)
                {
                    uiManager.RegisterInteraction(scoreValue);
                    hasAwardedVentScore = true;
                }
            }
        }
        else
        {
            isRemoving = false;
        }
    }

}
