using UnityEngine;

public class ScewInteractable : MonoBehaviour, IInteractable
{
    public Animator animator;
    public UIMessage uiMessage;
    public bool UnScrewed;

    private void Awake()
    {
        // Resolve references if not assigned in inspector
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (uiMessage == null)
        {
            uiMessage = FindFirstObjectByType<UIMessage>();
        }
    }

    public void Interact(PlayerScript player)
    {
        // Ignore invalid player interactions
        if (player == null) return;

        // Block interaction when player does not have the screwdriver yet
        if (!player.Screwdriver)
        {
            // Display message to player if they don't have the screwdriver
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
                uiMessage.ShowScrewdriverRequiredMessage();
            }

            return;
        }

        // Apply screw removal animation once requirements are met
        UnScrew();
    }

    public void UnScrew()
    {
        // Prevent multiple unscrew actions and ensure animator is available
        if (UnScrewed) return;
        if (animator == null) return;

        // Trigger the unscrew animation and update state
        UnScrewed = true;
        animator.SetBool("Unscrew", true);
    }
}
