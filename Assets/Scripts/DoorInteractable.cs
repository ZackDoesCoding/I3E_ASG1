 using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public enum DoorState
    {
        Manual,
        Lever,
        Generator
    }
    public Animator animator;
    public DoorState doorState = DoorState.Manual;
    private bool isOpen;
    public bool LeverRed = false;
    public bool LeverBlue = false;
    public UIManager uiManager;
    public UIMessage uiMessage;
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

    public void Interact(PlayerScript player)
    {
        if (player == null)
        {
            return;
        }
        switch (doorState)
        {
            case DoorState.Manual:
                ToggleDoor();
                break;
            case DoorState.Lever:
                if (IsLeverDoorUnlocked())
                {
                    OpenDoor();
                }
                else
                {
                    ShowLeverDoorLockedMessage();
                }
                break;
            case DoorState.Generator:
                if (uiManager != null && uiManager.currentBattery >= 5)
                {
                    ToggleDoor();
                }
                break;
        }
    }

    public void OpenDoor()
    {
        if (isOpen) return;

        isOpen = true;

        if (animator != null)
        {
            animator.SetBool("IsOpen", true);
        }
    }

    public void CloseDoor()
    {
        if (!isOpen) return;

        isOpen = false;

        if (animator != null)
        {
            animator.SetBool("IsOpen", false);
        }
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

    public void SetLeverState(bool isRedLever, bool isOn)
    {
        if (isRedLever)
        {
            LeverRed = isOn;
        }
        else
        {
            LeverBlue = isOn;
        }

        if (doorState == DoorState.Lever)
        {
            UpdateLeverDoorState();
        }
    }

    private bool IsLeverDoorUnlocked()
    {
        return LeverRed && LeverBlue;
    }

    private void UpdateLeverDoorState()
    {
        if (IsLeverDoorUnlocked())
        {
            OpenDoor();
            return;
        }

        CloseDoor();
    }

    private void ShowLeverDoorLockedMessage()
    {
        if (doorState != DoorState.Lever)
        {
            return;
        }

        if (uiMessage != null)
        {
            uiMessage.ShowDoorLeverMessage();
        }
    }
}
