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
        RefreshDisplay(true);
    }

    private void Update()
    {
        RefreshDisplay(false);
    }

    private void RefreshDisplay(bool force)
    {
        bool isOn = lever != null && lever.On;

        if (force || !hasState || lastState != isOn)
        {
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
