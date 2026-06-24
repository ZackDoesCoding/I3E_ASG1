using UnityEngine;

public class LeverStateDisplay : MonoBehaviour
{
    public LeverInteractable lever;
    public GameObject activeText;
    public GameObject inactiveText;

    private bool lastState;
    private bool hasState;

    private void Start()
    {
        // Initial refresh to set the correct display based on the lever's state
        RefreshDisplay(true);
    }

    private void Update()
    {
        // Refresh display every frame to ensure it reflects the current lever state
        RefreshDisplay(false);
    }

    private void RefreshDisplay(bool force)
    {
        // Read current lever state safely even if reference is missing
        bool isOn = lever != null && lever.On;

        // Update UI labels only when forced or when state changed
        if (force || !hasState || lastState != isOn)
        {
            // Update the active and inactive text based on the lever's state
            if (activeText != null)
            {
                activeText.SetActive(isOn);
            }

            if (inactiveText != null)
            {
                inactiveText.SetActive(!isOn);
            }

            lastState = isOn;
            hasState = true;
        }
    }
}
