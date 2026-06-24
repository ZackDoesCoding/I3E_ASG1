using UnityEngine;

public class LeverInteractable : MonoBehaviour, IInteractable
{
    // ENUMERATIONS (for easy selection of lever types in the inspector)
    public enum LeverType
    {
        Red,
        Blue
    }

    // INSPECTOR FIELDS
    public Animator animator;
    public DoorInteractable linkedDoor;
    public LeverType leverType = LeverType.Red;
    public bool On;
    public int scoreValue = 10;
    public AudioSource leverAudioSource;
    public AudioClip leverToggleClip;
    
    // UNITY LIFECYCLE
    private void Awake()
    {
        // Null check for animator and audio source
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (leverAudioSource == null)
        {
            leverAudioSource = GetComponent<AudioSource>();
        }
        // Ensure the linked door is aware of the lever's initial state
        NotifyDoor(); 
    }
    // INTERACTION API
    public void Interact(PlayerScript player) // Implement the IInteractable interface
    {
        // Toggle the lever state when interacted with
        ToggleLever();

        if (player == null)
        {
            return;
        }

        // find the UIManager instance, either from the player or by searching the scene.
        UIManager uiManager = player.uiManager;
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
            player.uiManager = uiManager;
        }
        // Register the interaction with the UIManager to update the score
        if (uiManager != null)
        {
            uiManager.RegisterInteraction(scoreValue);
        }
    }

    public void TurnOn()
    {
        // Check if the lever is already on; if so, do nothing
        if (On) return;
        On = true;

        // Update the animation
        if (animator != null)
        {
            animator.SetBool("On", true);
        }

        // Play the lever toggle audio and notify the linked door of the change in state
        PlayLeverAudio();
        NotifyDoor();
    }

    public void TurnOff()
    {
        // Check if the lever is already off; if so, do nothing
        if (!On) return;
        On = false;

        // Update the animation
        if (animator != null)
        {
            animator.SetBool("On", false);
        }

        // Play the lever toggle audio and notify the linked door of the change in state
        PlayLeverAudio();
        NotifyDoor();
    }

    public void ToggleLever()
    {
        // Toggle the lever state (turn on if off and turn off if on)
        if (On)
        {
            TurnOff();
            return;
        }
        TurnOn();
    }
    // INTERNAL HELPERS
    private void NotifyDoor()
    {
        // Null check for the linked door
        if (linkedDoor == null)
        {
            return;
        }

        // Determine if this lever is the red lever based on its type 
        // (if not red, it's blue) and notify the linked door of its current state
        // If intend to i add more lever types in the future, this will be changed to a more flexible system.
        bool isRedLever = leverType == LeverType.Red;
        linkedDoor.SetLeverState(isRedLever, On);
    }

    private void PlayLeverAudio()
    {
        // Null check for the audio source and clip, and play the audio if both are available
        if (leverAudioSource != null && leverToggleClip != null)
        {
            leverAudioSource.PlayOneShot(leverToggleClip);
        }
    }

}
