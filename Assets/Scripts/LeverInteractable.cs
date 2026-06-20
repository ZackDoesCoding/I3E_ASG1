using UnityEngine;

public class LeverInteractable : MonoBehaviour, IInteractable
{
    public Animator animator;

    private bool On;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            return;
        }

        animator.SetBool("On", On);
    }

    public void Interact(PlayerScript player)
    {
        ToggleLever();
    }

    public void TurnOn()
    {
        if (On) return;

        if (animator == null) return;

        On = true;
        animator.SetBool("On", true);
    }

    public void TurnOff()
    {
        if (!On) return;

        if (animator == null) return;

        On = false;
        animator.SetBool("On", false);
    }

    public void ToggleLever()
    {
        if (On)
        {
            TurnOff();
            return;
        }
        TurnOn();
    }
}
