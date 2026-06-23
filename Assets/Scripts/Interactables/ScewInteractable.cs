using UnityEngine;

public class ScewInteractable : MonoBehaviour, IInteractable
{
    public Animator animator;
    public UIMessage uiMessage;
    public bool UnScrewed;

    private void Awake()
    {
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
        if (player == null) return;
        if (!player.Screwdriver)
        {
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

        UnScrew();
    }

    public void UnScrew()
    {
        if (UnScrewed) return;

        if (animator == null) return;

        UnScrewed = true;
        animator.SetBool("Unscrew", true);
    }
}
