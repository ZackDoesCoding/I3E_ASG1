 using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    // ENUMERATIONS (for easy selection of door types in the inspector)
    public enum DoorState
    {
        Manual,
        Lever,
        Generator
    }

    // INSPECTOR FIELDS
    public Animator animator;
    public DoorState doorState = DoorState.Manual;
    private bool isOpen;
    public bool LeverRed = false;
    public bool LeverBlue = false;
    public UIManager uiManager;
    public UIMessage uiMessage;
    public int scoreValue = 10;
    public AudioSource doorAudioSource;
    public AudioClip manualDoorClip;
    public AudioClip leverDoorClip;
    public AudioClip generatorDoorClip;
    // INTERNAL STATE
    private BoxCollider triggerBox;
    private bool hasAwardedDoorScore;
    
    // UNITY LIFECYCLE
    private void Awake()
    {
        // Null check for animator and audio source
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
    // PLAYER INTERACTION
    public void Interact(PlayerScript player) // Implement the IInteractable interface
    {
        if (player == null)
        {
            return;
        }
        // Handle interaction based on the door's state
        switch (doorState)
        {
            case DoorState.Manual: // Manual doors can be toggled directly
                ToggleDoor();
                break;
            case DoorState.Lever: // Lever doors require both levers to be activated
                if (IsLeverDoorUnlocked())
                {
                    OpenDoor();
                }
                else
                {
                    ShowLeverDoorLockedMessage();
                }
                break;
            case DoorState.Generator: // Generator doors require the player to have enough batteries
                if (uiManager != null && uiManager.HasRequiredBatteries())
                {
                    ToggleDoor();
                }
                else if (uiMessage != null)
                {
                    // Show a message indicating that the generator door needs batteries
                    uiMessage.ShowGeneratorDoorNeedsBatteriesMessage();
                }
                break;
        }
    }
    // DOOR STATE CONTROL
    public void OpenDoor()
    {
        // Prevent opening if already open
        if (isOpen) return;

        isOpen = true;

        // Update the animation
        if (animator != null)
        {
            animator.SetBool("IsOpen", true);
        }

        // Award score for opening the door, but only the first time
        if (!hasAwardedDoorScore)
        {
            if (uiManager == null)
            {
                uiManager = FindFirstObjectByType<UIManager>();
            }
            if (uiManager != null)
            {
                uiManager.RegisterInteraction(scoreValue);
                hasAwardedDoorScore = true;
            }
        }
        // Play the door opening audio
        PlayDoorAudio();
    }

    public void CloseDoor()
    {
        // Prevent closing if already closed
        if (!isOpen) return;

        isOpen = false;

        // Update the animation
        if (animator != null)
        {
            animator.SetBool("IsOpen", false);
        }

        // Play the door closing audio
        PlayDoorAudio();
    }

    public void ToggleDoor()
    {
        // Toggle the door state (open if closed and close if open)
        if (isOpen)
        {
            CloseDoor();
            return;
        }
        OpenDoor();
    }

    public void SetLeverState(bool isRedLever, bool isOn)
    {
        // Update the state of the specified lever 
        // (red or blue) based on the provided parameters
        if (isRedLever)
        {
            LeverRed = isOn;
        }
        else
        {
            LeverBlue = isOn;
        }

        // Update the lever door state if this is a lever door
        if (doorState == DoorState.Lever)
        {
            UpdateLeverDoorState();
        }
    }

    // LEVER LOGIC
    private bool IsLeverDoorUnlocked()
    {
        // check if both levers are activated to unlock the lever door
        return LeverRed && LeverBlue; 
    }

    private void UpdateLeverDoorState()
    {
        // Update the door state based on the lever combination
        if (IsLeverDoorUnlocked())
        {
            if (!isOpen && uiMessage != null)
            {
                // Show a message indicating that both levers are turned on when the door is unlocked
                uiMessage.ShowBothLeversTurnedOnMessage();
            }

            OpenDoor();
            return;
        }

        CloseDoor();
    }

    private void ShowLeverDoorLockedMessage()
    {
        // Only show the message if this is a lever door
        if (doorState != DoorState.Lever)
        {
            return;
        }

        if (uiMessage != null)
        {
            uiMessage.ShowDoorLeverMessage();
        }
    }

    // AUDIO
    private void PlayDoorAudio()
    {
        // Null check for the audio source 
        if (doorAudioSource == null)
        {
            return;
        }

        // Select the appropriate audio clip based on the door's state
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
            // Play the selected audio clip
            doorAudioSource.PlayOneShot(selectedClip);
        }
    }


    // TRIGGER EVENTS
    private void OnTriggerEnter(Collider other)
    {
        // Only auto-open generator doors
        if (doorState != DoorState.Generator)
        {
            return;
        }

        // Check if the player entered and has enough batteries
        if ((other.CompareTag("Player") || other.GetComponent<PlayerScript>() != null) &&
            uiManager != null && uiManager.HasRequiredBatteries())
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
