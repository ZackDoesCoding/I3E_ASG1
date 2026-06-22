using UnityEngine;

public class LeverInteractable : MonoBehaviour, IInteractable
{
    public enum LeverType
    {
        Red,
        Blue
    }

    public Animator animator;
    public DoorInteractable linkedDoor;
    public LeverType leverType = LeverType.Red;
    public bool On;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator != null)
        {
            animator.SetBool("On", On);
        }

        NotifyDoor();
    }

    public void Interact(PlayerScript player)
    {
        ToggleLever();
    }

    public void TurnOn()
    {
        if (On) return;

        On = true;

        if (animator != null)
        {
            animator.SetBool("On", true);
        }

        NotifyDoor();
    }

    public void TurnOff()
    {
        if (!On) return;

        On = false;

        if (animator != null)
        {
            animator.SetBool("On", false);
        }

        NotifyDoor();
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

    private void NotifyDoor()
    {
        if (linkedDoor == null)
        {
            return;
        }

        bool isRedLever = leverType == LeverType.Red;
        linkedDoor.SetLeverState(isRedLever, On);
    }
}
