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
    public AudioSource doorAudioSource;
    public AudioClip manualDoorClip;
    public AudioClip leverDoorClip;
    public AudioClip generatorDoorClip;
    private BoxCollider triggerBox;
    private bool hasAwardedDoorScore;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (doorAudioSource == null)
        {
            doorAudioSource = GetComponent<AudioSource>();
        }

        // Get the BoxCollider and set it as trigger
        triggerBox = GetComponent<BoxCollider>();
        if (triggerBox != null)
        {
            triggerBox.isTrigger = true;
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

        if (!hasAwardedDoorScore)
        {
            if (uiManager == null)
            {
                uiManager = FindFirstObjectByType<UIManager>();
            }

            if (uiManager != null)
            {
                uiManager.RegisterInteraction();
                hasAwardedDoorScore = true;
            }
        }

        PlayDoorAudio();
    }

    public void CloseDoor()
    {
        if (!isOpen) return;

        isOpen = false;

        if (animator != null)
        {
            animator.SetBool("IsOpen", false);
        }

        PlayDoorAudio();
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

    private void PlayDoorAudio()
    {
        if (doorAudioSource == null)
        {
            return;
        }

        AudioClip selectedClip = null;
        switch (doorState)
        {
            case DoorState.Manual:
                selectedClip = manualDoorClip;
                break;
            case DoorState.Lever:
                selectedClip = leverDoorClip;
                break;
            case DoorState.Generator:
                selectedClip = generatorDoorClip;
                break;
        }

        if (selectedClip != null)
        {
            doorAudioSource.PlayOneShot(selectedClip);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only auto-open generator doors
        if (doorState != DoorState.Generator)
        {
            return;
        }

        // Check if the player entered and has enough batteries
        if ((other.CompareTag("Player") || other.GetComponent<PlayerScript>() != null) &&
            uiManager != null && uiManager.currentBattery >= 5)
        {
            OpenDoor();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Only auto-close generator doors
        if (doorState != DoorState.Generator)
        {
            return;
        }

        // Check if the player left the trigger
        if (other.CompareTag("Player") || other.GetComponent<PlayerScript>() != null)
        {
            CloseDoor();
        }
    }
}
