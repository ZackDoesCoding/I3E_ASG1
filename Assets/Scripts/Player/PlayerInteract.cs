using UnityEngine;
using UnityEngine.InputSystem;

public interface IInteractable
{
    void Interact(PlayerScript player);
}

public class PlayerInteract : MonoBehaviour
{
    // INSPECTOR FIELDS
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float interactDistance = 3f;
    public UIManager uiManager;
    public UIMessage uiMessage;
    // INTERNAL STATE
    private PlayerScript playerScript;
    // UNITY LIFECYCLE
    private void Awake()
    {
        // Cache the PlayerScript component for interaction handling
        playerScript = GetComponent<PlayerScript>();
        // Ensure the playerCamera is assigned, defaulting to the main camera if not set
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }
    // INPUT
    // This method is called when the player presses the interact button (F key)
    private void OnInteract(InputValue value) 
    {
        if (!value.isPressed) return;
        // Attempt to interact with an object in front of the player
        TryInteract();
    }

    // INTERACTION 
    public bool TryInteract()
    {
        // Ensure the playerCamera is assigned, defaulting to the main camera if not set
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        if (playerCamera == null) return false;

        // Create a ray from the camera's position forward to detect interactable objects
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        // Perform a raycast to check for interactable objects within the specified distance
        if (!Physics.Raycast(ray, out RaycastHit hit, interactDistance))
        {
            return false;
        }
        // Attempt to get an IInteractable component from the hit object or its parents
        IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
        if (interactable == null) return false;
        // Call the Interact method on the interactable object, passing in the player's script
        interactable.Interact(playerScript);
        return true;
    }

}