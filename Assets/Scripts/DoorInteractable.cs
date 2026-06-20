using UnityEngine;

public class DoorInteractable : MonoBehaviour
{
    public Animator animator;

    private bool isOpen;

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

        animator.SetBool("IsOpen", isOpen);
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        if (animator == null) return;

        isOpen = true;
        animator.SetBool("IsOpen", true);
    }

    public void CloseDoor()
    {
        if (!isOpen) return;

        if (animator == null) return;

        isOpen = false;
        animator.SetBool("IsOpen", false);
    }

    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
            return;
        }

        OpenDoor();
    }
}
