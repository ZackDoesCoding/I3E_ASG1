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
    public int scoreValue = 10;
    public AudioSource leverAudioSource;
    public AudioClip leverToggleClip;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (leverAudioSource == null)
        {
            leverAudioSource = GetComponent<AudioSource>();
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

        if (player == null)
        {
            return;
        }

        UIManager uiManager = player.uiManager;
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UIManager>();
            player.uiManager = uiManager;
        }

        if (uiManager != null)
        {
            uiManager.RegisterInteraction(scoreValue);
        }
    }

    public void TurnOn()
    {
        if (On) return;

        On = true;

        if (animator != null)
        {
            animator.SetBool("On", true);
        }

        PlayLeverAudio();

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

        PlayLeverAudio();

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

    private void PlayLeverAudio()
    {
        if (leverAudioSource != null && leverToggleClip != null)
        {
            leverAudioSource.PlayOneShot(leverToggleClip);
        }
    }
}
