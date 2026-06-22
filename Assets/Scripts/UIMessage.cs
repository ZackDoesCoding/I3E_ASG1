using TMPro;
using UnityEngine;
using StarterAssets;
using UnityEngine.UI;

public class UIMessage : MonoBehaviour
{
    public GameObject MessagePanel;
    public GameObject ScrewdriverRequiredMessage;
    public GameObject GasmaskMessage;
    public GameObject ScrewdriverMessage;
    public GameObject DoorLeverMessage1;
    public GameObject DoorLeverMessage2;
    public float messageDuration = 3f;

    // Coroutine to handle message display timing
    private Coroutine hideCoroutine;

    private void HideAllMessages()
    {
        if (ScrewdriverRequiredMessage != null) ScrewdriverRequiredMessage.SetActive(false);
        if (GasmaskMessage != null) GasmaskMessage.SetActive(false);
        if (ScrewdriverMessage != null) ScrewdriverMessage.SetActive(false);
        if (DoorLeverMessage1 != null) DoorLeverMessage1.SetActive(false);
        if (DoorLeverMessage2 != null) DoorLeverMessage2.SetActive(false);
    }

    private void ShowMessages(params GameObject[] messagesToShow)
    {
        if (MessagePanel == null || messagesToShow == null || messagesToShow.Length == 0) return;

        HideAllMessages();
        MessagePanel.SetActive(true);

        bool hasAnyMessage = false;
        foreach (GameObject message in messagesToShow)
        {
            if (message == null) continue;

            message.SetActive(true);
            hasAnyMessage = true;
        }

        if (!hasAnyMessage)
        {
            MessagePanel.SetActive(false);
            return;
        }

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(HideMessageAfterDelay());
    }

    private System.Collections.IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);

        HideAllMessages();
        if (MessagePanel != null)
        {
            MessagePanel.SetActive(false);
        }

        hideCoroutine = null;
    }

    public void ShowScrewdriverRequiredMessage()
    {
        ShowMessages(ScrewdriverRequiredMessage);
    }

    public void ShowGasmaskMessage()
    {
        ShowMessages(GasmaskMessage);
    }

    public void ShowScrewdriverMessage()
    {
        ShowMessages(ScrewdriverMessage);
    }

    public void ShowDoorLeverMessage()
    {
        ShowMessages(DoorLeverMessage1, DoorLeverMessage2);
    }

}
