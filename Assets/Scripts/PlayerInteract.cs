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
            Debug.Log($"Raycast hit nothing within {interactDistance} units.");
            return false;
        }

        GameObject hitObject = hit.collider.gameObject;
        Debug.Log($"Raycast hit: {hitObject.name} | tag={hitObject.tag} | layer={LayerMask.LayerToName(hitObject.layer)}");

        if (hitObject.CompareTag("screwdriver"))
        {
            playerScript.Screwdriver = true;
            Destroy(hitObject);
            uiManager.ToggleScrewdriverIcon();
            return true;
        }

        if (hitObject.CompareTag("gasmask"))
        {
            playerScript.GasMask = true;
            Destroy(hitObject);
            uiManager.ToggleGasmaskIcon();
            return true;
        }

        IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
        if (interactable == null) return false;

        interactable.Interact(playerScript);
        return true;
    }
}