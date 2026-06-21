using UnityEngine;

public class ScewInteractable : MonoBehaviour, IInteractable
{
    public Animator animator;

    private bool UnScrewed;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void Interact(PlayerScript player)
    {
        if (player == null) return;
        if (!player.Screwdriver) return;

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
