using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void Interact(PlayerScript player);
}

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    public UIManager uiManager;
    public UIMessage uiMessage;
    private PlayerScript playerScript;

    private void Awake()
    {
        playerScript = GetComponent<PlayerScript>();

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void OnInteract(InputValue value)
    {
        if (!value.isPressed) return;

        TryInteract();
    }

    public bool TryInteract()
    {

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (playerCamera == null) return false;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            return false;
        }

        GameObject hitObject = hit.collider.gameObject;

        if (hitObject.CompareTag("screwdriver"))
        {
            if (playerScript == null) return false;

            playerScript.Screwdriver = true;
            Destroy(hitObject);

            if (uiManager != null)
            {
                uiManager.ToggleScrewdriverIcon();
                uiManager.RegisterInteraction();
            }

            if (uiMessage != null)
            {
                uiMessage.ShowScrewdriverMessage();
            }

            playerScript.PlayPickupAudio();
            return true;
        }

        if (hitObject.CompareTag("gasmask"))
        {
            if (playerScript == null) return false;

            playerScript.GasMask = true;
            Destroy(hitObject);

            if (uiManager != null)
            {
                uiManager.ToggleGasmaskIcon();
                uiManager.RegisterInteraction();
            }

            if (uiMessage != null)
            {
                uiMessage.ShowGasmaskMessage();
            }

            playerScript.PlayPickupAudio();
            return true;
        }

        IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
        if (interactable == null) return false;

        interactable.Interact(playerScript);
        return true;
    }
}