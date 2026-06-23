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
        RemoveScrews();
    }

    public void RemoveScrews()
    {
        if (isRemoving)
        {
            return;
        }

        if (Screw1 == null || Screw2 == null || Screw3 == null || Screw4 == null)
        {
            return;
        }

        if (Screw1.UnScrewed &&
            Screw2.UnScrewed &&
            Screw3.UnScrewed &&
            Screw4.UnScrewed)
        {
            StartCoroutine(RemoveAfterDelay());
        }
    }

    private System.Collections.IEnumerator RemoveAfterDelay()
    {
        isRemoving = true;
        yield return new WaitForSeconds(removeDelay);

        if (Screw1 != null && Screw2 != null && Screw3 != null && Screw4 != null &&
            Screw1.UnScrewed && Screw2.UnScrewed && Screw3.UnScrewed && Screw4.UnScrewed)
        {
            if (ventAudioSource != null && ventRemoveClip != null)
            {
                ventAudioSource.PlayOneShot(ventRemoveClip);
            }

            MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>(true);
            foreach (MeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = false;
            }

            Collider[] colliders = GetComponentsInChildren<Collider>(true);
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }

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
